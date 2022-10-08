﻿using MineSharp.Core.Logging;
using MineSharp.Core.Types.Enums;
using MineSharp.Data;
using MineSharp.Data.Protocol;
using MineSharp.Data.Protocol.Handshaking.Serverbound;
using MineSharp.Data.Protocol.Login.Serverbound;
using MineSharp.Data.Protocol.Status.Serverbound;
using MineSharp.MojangAuth;
using MineSharp.Protocol.Handlers;
using System.Net;
using System.Net.Sockets;
namespace MineSharp.Protocol
{
    public class MinecraftClient
    {
        public delegate void ClientPacketEvent(MinecraftClient client, IPacketPayload packet);

        public delegate void ClientStringEvent(MinecraftClient client, string message);

        private static readonly Logger Logger = Logger.GetLogger();

        private readonly TcpClient _client;
        private readonly CancellationToken CancellationToken;

        private readonly CancellationTokenSource CancellationTokenSource;
        private readonly PacketFactory PacketFactory;
        private MinecraftStream? _stream;
        private Task? _streamLoopTask;
        internal int CompressionThreshold = -1;

        public IPacketHandler? PacketHandler;

        private readonly Queue<PacketSendTask> PacketQueue;

        private byte[]? sharedSecret;

        public MinecraftClient(string version, Session session, string host, int port)
        {
            this.Version = version;
            this.Session = session;
            this.GameState = GameState.HANDSHAKING;
            this.IPAddress = this.GetIpAddress(host);
            this.Port = port;
            this.CancellationTokenSource = new CancellationTokenSource();
            this.CancellationToken = this.CancellationTokenSource.Token;
            this.PacketQueue = new Queue<PacketSendTask>();
            this.PacketFactory = new PacketFactory(this);
            this.PacketHandler = this.GetPacketHandler(this.GameState);

            this._client = new TcpClient();
        }

        public string Version
        {
            get;
        }
        public GameState GameState { get; private set; }
        public string IPAddress
        {
            get;
        }
        public int Port
        {
            get;
        }

        public Session Session
        {
            get;
        }

        /// <summary>
        ///     Fires when the client Disconnected
        /// </summary>
        public event ClientStringEvent? Disconnected;

        /// <summary>
        ///     Fires when a packet has been received
        /// </summary>
        public event ClientPacketEvent? PacketReceived;

        /// <summary>
        ///     Fires when a packet has been sent
        /// </summary>
        public event ClientPacketEvent? PacketSent;

        private string GetIpAddress(string host)
        {

            var type = Uri.CheckHostName(host);
            return type switch {
                UriHostNameType.Dns =>
                    (Dns.GetHostEntry(host).AddressList.FirstOrDefault()
                     ?? throw new Exception($"Could not find ip for hostname ('{host}')")).ToString(),

                UriHostNameType.IPv4 => host,

                _ => throw new Exception("Hostname not supported: " + host)
            };

        }

        private IPacketHandler? GetPacketHandler(GameState state)
        {
            return state switch {
                GameState.HANDSHAKING => new HandshakePacketHandler(),
                GameState.LOGIN => new LoginPacketHandler(),
                GameState.PLAY => new PlayPacketHandler(),
                _ => null
            };
        }

        /// <summary>
        ///     Connects to the server and making the handshake
        /// </summary>
        /// <param name="nextState">
        ///     In which <see cref="MineSharp.Protocol.GameState" /> the client and server should switch.
        ///     Should be either <see cref="Protocol.GameState.LOGIN" /> or <see cref="Protocol.GameState.STATUS" />
        /// </param>
        /// <returns>A task that is completed as soon as the Handshake with the server is completed</returns>
        public async Task<bool> Connect(GameState nextState)
        {
            Logger.Debug("Connecting...");
            try
            {
                await this._client.ConnectAsync(this.IPAddress, this.Port, this.CancellationToken);

                // Start Reading / Writing Loops
                this._stream = new MinecraftStream(this._client.GetStream());
                this._streamLoopTask = Task.Run(async () => await this.StreamLoop(), this.CancellationToken);

                Logger.Info("Connected, starting handshake");
                await this.MakeHandshake(nextState);

                return true;
            } catch (Exception ex)
            {
                Logger.Error("Could not connect: " + ex);
                return false;
            }
        }

        /// <summary>
        ///     Closes the connection on the client side without
        /// </summary>
        /// <param name="reason"></param>
        public void ForceDisconnect(string reason)
        {
            Logger.Debug("Forcing Disconnect: " + reason);
            this.CancellationTokenSource.Cancel();
            this._streamLoopTask?.Wait();
            this._client.Close();
            this.Disconnected?.Invoke(this, reason);
        }

        private async Task MakeHandshake(GameState nextState)
        {
            await this.SendPacket(
                new PacketSetProtocol(
                    MinecraftData.ProtocolVersion,
                    this.IPAddress,
                    (ushort)this.Port,
                    (int)nextState));

            await (nextState switch {
                GameState.STATUS => this.SendPacket(new PacketPingStart()),
                GameState.LOGIN => this.SendPacket(new PacketLoginStart(this.Session.Username)),
                _ => throw new InvalidOperationException($"Next state '{nextState}' is not supported")
            });
        }
        public void SetEncryptionKey(byte[] key)
        {
            this.sharedSecret = key;
        }

        public void EnableEncryption() => this._stream!.EnableEncryption(this.sharedSecret!);

        public void SetCompressionThreshold(int compressionThreshold)
        {
            this.CompressionThreshold = compressionThreshold;
        }

        public void SetGameState(GameState newState)
        {
            this.GameState = newState;
            this.PacketHandler = this.GetPacketHandler(newState);
        }


        /// <summary>
        ///     Sends a <see cref="Data.Protocol.Handshaking.Serverbound.Packet" /> to the Minecraft Server
        /// </summary>
        /// <param name="packet">Packet instance that will be sent</param>
        /// <returns>A task that will be completed when the packet has been written into the tcp stream</returns>
        public Task SendPacket(IPacketPayload packet, CancellationToken? cancellation = null)
        {
            if (packet == null) throw new ArgumentNullException();
            //Logger.Debug3("Queueing packet: " + packet.GetType().Name);

            var sendTask = new PacketSendTask(cancellation, packet, new TaskCompletionSource());
            this.PacketQueue.Enqueue(sendTask);

            return sendTask.SendingTsc.Task;
        }


        private async Task StreamLoop()
        {
            Task readPacket()
            {
                var length = this._stream!.ReadVarInt();
                var uncompressedLength = 0;

                if (this.CompressionThreshold > 0)
                {
                    var r = 0;
                    uncompressedLength = this._stream.ReadVarInt(out r);
                    length -= r;
                }
                var data = this._stream.Read(length);
                var packet = this.PacketFactory.BuildPacket(data, uncompressedLength);
                if (packet != null)
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        //Logger.Debug3("Received packet: " + packet.GetType().Name); // Causes cpu usage spikes
                        try
                        {
                            if (this.PacketHandler != null) this.PacketHandler.HandleIncomming(packet, this).Wait(this.CancellationToken);
                            this.PacketReceived?.Invoke(this, packet);
                        } catch (Exception e)
                        {
                            Logger.Error("There occurred an error while handling the packet: \n" + e);
                        }
                    });
                return Task.CompletedTask;
            }

            while (!this.CancellationToken.IsCancellationRequested)
            {
                try
                {

                    if (this.GameState != GameState.PLAY)
                    {
                        if (this._client.Available > 0) await readPacket();
                        await Task.Delay(1, this.CancellationToken);
                    } else
                    {
                        while (this._client.Available > 0) { await readPacket(); }
                        await Task.Delay(1, this.CancellationToken);
                    }

                    if (this.PacketQueue.Count == 0) continue;
                    // Writing
                    var packetTask = this.PacketQueue.Dequeue();

                    if (packetTask.CancellationToken.HasValue && packetTask.CancellationToken.Value.IsCancellationRequested)
                    {
                        packetTask.SendingTsc.SetCanceled(packetTask.CancellationToken.Value);
                        continue;
                    }

                    if (packetTask.Packet == null) // https://github.com/psu-de/MineSharp/issues/8#issue-1315635361
                    {
                        // for now just ignore the packet,
                        // since i have no idea why this happens
                        if (packetTask.SendingTsc != null)
                            packetTask.SendingTsc.TrySetCanceled();
                        continue;
                    }

                    var packetBuffer = this.PacketFactory.WritePacket(packetTask.Packet);

                    this._stream!.DispatchPacket(packetBuffer);

                    packetTask.SendingTsc.TrySetResult();
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            if (this.PacketHandler != null) this.PacketHandler.HandleOutgoing(packetTask.Packet, this).Wait(this.CancellationToken);
                            this.PacketSent?.Invoke(this, packetTask.Packet);
                        } catch (Exception e)
                        {
                            Logger.Error("Error while handling sent event of packet: \n" + e);
                        }
                    });
                } catch (Exception e)
                {
                    Logger.Error("Error in readLoop: " + e);
                }
            }
        }

        private struct PacketSendTask
        {
            public CancellationToken? CancellationToken
            {
                get;
            }
            public IPacketPayload Packet
            {
                get;
            }
            public TaskCompletionSource SendingTsc
            {
                get;
            }

            public PacketSendTask(CancellationToken? cancellationToken, IPacketPayload packet, TaskCompletionSource tsc)
            {
                this.CancellationToken = cancellationToken;
                this.Packet = packet;
                this.SendingTsc = tsc;
            }
        }
    }
}
