using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Inbound
{
    public abstract class InboundPacket
    {
        public short PacketId;
        public DateTime PacketReceived;

        protected InboundPacket(short id)
        {
            PacketId = id;
        }

        public abstract void ReadPacket(BinaryReader reader);
    }
}
