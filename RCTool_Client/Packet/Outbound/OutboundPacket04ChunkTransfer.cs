using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Outbound
{
    public class OutboundPacket04ChunkTransfer : OutboundPacket
    {
        public int CurrentPart;
        public int PartAmount;
        public int BufferSize;
        public byte[] Buffer;

        public OutboundPacket04ChunkTransfer() : base(4)
        {
        }

        public override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CurrentPart);
            writer.Write(PartAmount);
            writer.Write(BufferSize);
            writer.Write(Buffer);
        }
    }
}
