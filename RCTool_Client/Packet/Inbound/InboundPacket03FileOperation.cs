using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Inbound
{
    public class InboundPacket03FileOperation : InboundPacket
    {
        public enum EnumOperation
        {
            LIST_DIR_CONTENT
        }

        public EnumOperation Operation { get; set; }

        public string FileSystemPath { get; set; }

        public InboundPacket03FileOperation() : base(3)
        {
        }

        public override void ReadPacket(BinaryReader reader)
        {
            this.Operation = (EnumOperation)reader.ReadInt16();
            this.FileSystemPath = reader.ReadString();
        }
    }
}
