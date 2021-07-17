using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Outbound
{
    public class OutboundPacket02DataResponse : OutboundPacket
    {
        public short ResponseDataType = 0;

        public OutboundPacket02DataResponse() : base(2)
        {
        }

        public override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ResponseDataType);
        }
    }
}
