using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Outbound
{
    public class OutboundPacket00KeepAlive : OutboundPacket
    {
        public OutboundPacket00KeepAlive() : base(0)
        {
        }
        public override void WritePacket(BinaryWriter writer)
        {
            writer.Write(DateTime.Now.Ticks);
        }
    }
}
