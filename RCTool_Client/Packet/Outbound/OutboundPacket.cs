using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RCTool_Client.Packet.Outbound
{
    public abstract class OutboundPacket
    {
        public short PacketId;

        protected OutboundPacket(short packetId)
        {
            PacketId = packetId;
        }

        public abstract void WritePacket(BinaryWriter writer);
    }
}
