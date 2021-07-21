using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Outbound
{
    public class OutboundPacket999MultiPacket : OutboundPacket
    {
        public string PacketUuid;
        public int ChunkIndex, ChunkAmount;
        public int BufferSize;
        public byte[] Buffer;

        public OutboundPacket999MultiPacket() : base(999)
        {
        }

        public override void WritePacket(BinaryWriter writer)
        {
            writer.Write(PacketUuid);
            writer.Write(ChunkIndex);
            writer.Write(ChunkAmount);
            writer.Write(BufferSize);
            writer.Write(Buffer);
        }
    }
}
