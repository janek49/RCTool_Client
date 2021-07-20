using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Inbound
{
    public class InboundPacket01OpenSocket :InboundPacket
    {
        public short ConnectionType;
        public string Uuid;

        public InboundPacket01OpenSocket() : base(1)
        {
        }

        public override void ReadPacket(BinaryReader reader)
        {
            ConnectionType = reader.ReadInt16();
            Uuid = reader.ReadString();
        }
    }
}
