using MineSharp.Core.Common;
using MineSharp.Core.Common.Blocks;
using MineSharp.Protocol.Packets.Clientbound.Play;
using MineSharp.World;
using MineSharp.World.Chunks;
using NLog;

namespace MineSharp.Bot.Plugins;

public class WorldPlugin : Plugin
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    
    public IWorld? World { get; private set; }
    
    public WorldPlugin(MinecraftBot bot) : base(bot)
    { }

    protected override Task Init()
    {
        this.World = WorldVersion.CreateWorld(this.Bot.Data);

        this.Bot.Client.On<ChunkDataAndUpdateLightPacket>(HandleChunkDataAndLightUpdatePacket);
        this.Bot.Client.On<UnloadChunkPacket>(this.HandleUnloadChunkPacket);
        this.Bot.Client.On<BlockUpdatePacket>(this.HandleBlockUpdatePacket);
        this.Bot.Client.On<MultiBlockUpdatePacket>(this.HandleMultiBlockUpdatePacket);

        return Task.CompletedTask;
    }

    private Task HandleChunkDataAndLightUpdatePacket(ChunkDataAndUpdateLightPacket packet)
    {
        if (!this.IsEnabled)
            return Task.CompletedTask;
        
        var coords = new ChunkCoordinates(packet.X, packet.Z);
        var chunk = WorldVersion.CreateChunk(this.Bot.Data, coords, packet.BlockEntities);
        chunk.LoadData(packet.ChunkData);
        
        this.World!.LoadChunk(chunk);
        
        return Task.CompletedTask;
    }

    private Task HandleUnloadChunkPacket(UnloadChunkPacket packet)
    {        
        if (!this.IsEnabled)
            return Task.CompletedTask;
        
        var coords = new ChunkCoordinates(packet.X, packet.Z);
        
        this.World!.UnloadChunk(coords);
        return Task.CompletedTask;
    }

    private Task HandleBlockUpdatePacket(BlockUpdatePacket packet)
    {
        if (!this.IsEnabled)
            return Task.CompletedTask;
        
        var blockInfo = this.Bot.Data.Blocks.GetByState(packet.StateId);
        var block = new Block(blockInfo, packet.StateId, packet.Location);

        this.World!.SetBlock(block);
        return Task.CompletedTask;
    }

    private Task HandleMultiBlockUpdatePacket(MultiBlockUpdatePacket packet)
    {
        if (!this.IsEnabled)
            return Task.CompletedTask;
        
        var sectionX = packet.ChunkSection >> 42;
        var sectionY = packet.ChunkSection << 44 >> 44;
        var sectionZ = packet.ChunkSection << 22 >> 42;
        
        if (sectionX > Math.Pow(2, 21)) 
            sectionX -= (int)Math.Pow(2, 22);
        
        if (sectionY > Math.Pow(2, 19)) 
            sectionY -= (int)Math.Pow(2, 20);
        
        if (sectionZ > Math.Pow(2, 21)) 
            sectionZ -= (int)Math.Pow(2, 22);

        var coords = new ChunkCoordinates((int)sectionX, (int)sectionZ);
        var chunk = this.World!.GetChunkAt(coords);

        sectionY += Math.Abs(this.World!.MinY / chunk.Size);
        
        foreach (var l in packet.Blocks)
        {
            var blockZ = (int)(l >> 4 & 0x0F);
            var blockX = (int)(l >> 8 & 0x0F);
            var blockY = (int)(l & 0x0F) + (int)sectionY * chunk.Size;
            var stateId = (int)(l >> 12);

            var blockInfo = this.Bot.Data.Blocks.GetByState(stateId);
            var block = new Block(blockInfo, stateId, new Position(blockX, blockY, blockZ));

            chunk.SetBlock(block);
        }

        return Task.CompletedTask;
    }
}