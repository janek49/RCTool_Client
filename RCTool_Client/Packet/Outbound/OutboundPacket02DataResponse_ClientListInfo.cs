using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Outbound
{
    public class OutboundPacket02DataResponse_ClientListInfo : OutboundPacket02DataResponse
    {
        public string Username;
        public string OperatingSystem;
        public string RAM;
        public string Language;

        public OutboundPacket02DataResponse_ClientListInfo()
        {
            ResponseDataType = 0;
        }

        public override void WritePacket(BinaryWriter writer)
        {
            base.WritePacket(writer);
            writer.Write(Username);
            writer.Write(OperatingSystem);
            writer.Write(RAM);
            writer.Write(Language);
            writer.Write(DateTime.Now.Ticks);
        }
    }
}
