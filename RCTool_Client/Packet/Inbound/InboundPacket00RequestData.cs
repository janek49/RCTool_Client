using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Inbound
{
    public class InboundPacket00RequestData : InboundPacket
    {
        public short RequestedDataType;

        public InboundPacket00RequestData() : base(0)
        {
        }

        public override void ReadPacket(BinaryReader reader)
        {
            RequestedDataType = reader.ReadInt16();
        }
    }
}
