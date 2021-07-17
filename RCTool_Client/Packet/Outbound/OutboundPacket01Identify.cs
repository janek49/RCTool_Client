using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Outbound
{
    public class OutboundPacket01Identify : OutboundPacket
    {
        public short ConnectionType = 0;
        public string ClientId = "";
        public string Password = "123456";

        public OutboundPacket01Identify() : base(1)
        {
        }

        public override void WritePacket(BinaryWriter wr)
        {
            wr.Write(ConnectionType);
            wr.Write(ClientId);
            wr.Write(Password);
        }
    }
}
