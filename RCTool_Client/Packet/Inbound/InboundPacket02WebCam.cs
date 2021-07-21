using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Inbound
{
    public class InboundPacket02WebCam : InboundPacket
    {
        public short CommandMode;
        public string CamId;
        public InboundPacket02WebCam() : base(2)
        {
        }

        public override void ReadPacket(BinaryReader reader)
        {
            CommandMode = reader.ReadInt16();
            CamId = reader.ReadString();
        }
    }
}
