using MineSharp.Core.Common;
using MineSharp.Protocol.Packets.Serverbound.Play;

/*
 * Thanks to https://github.com/MCCTeam/Minecraft-Console-Client
 * Some code has been copied and modified from:
 *  - MinecraftClient/Protocol/Handlers/Protocol18.cs
 */
namespace MineSharp.Bot.Chat;

internal class AcknowledgedMessage
{
    public bool Pending { get; set; }
    public UUID Sender { get; set; }
    public byte[] Signature { get; set; }

    public AcknowledgedMessage(bool pending, UUID sender, byte[] signature)
    {
        this.Pending = pending;
        this.Sender = sender;
        this.Signature = signature;
    }

    public ChatMessagePacket.V1_19.MessageItem ToProtocolMessage()
    {
        return new ChatMessagePacket.V1_19.MessageItem(
            this.Sender, this.Signature);
    }
}
