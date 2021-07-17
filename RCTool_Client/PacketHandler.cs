using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RCTool_Client.Packet.Inbound;
using RCTool_Client.Packet.Outbound;

namespace RCTool_Client
{
    public class PacketHandler
    {
        public ServerConnection ServerConnection;

        private readonly Dictionary<short, Type> _inboudPackeTypeDictionary = new Dictionary<short, Type>
        {
            {0,typeof(InboudPacket00RequestData)}
        };

        public delegate void OnPacketReceived(ServerConnection scon, InboundPacket packet);
        public event OnPacketReceived OnPacketReceivedEvent;

        public PacketHandler(ServerConnection scon)
        {
            this.ServerConnection = scon;
        }

        public void HandleIncomingRawPacket(byte[] bytes)
        {
            if (bytes.Length < 2)
                throw new InvalidOperationException("Invalid packet size.");

            using (BinaryReader reader = new BinaryReader(new MemoryStream(bytes)))
            {
                short packetId = reader.ReadInt16();

                if (!_inboudPackeTypeDictionary.ContainsKey(packetId))
                    throw new InvalidOperationException("Invalid packet id: " + packetId);

                InboundPacket packet = (InboundPacket) Activator.CreateInstance(_inboudPackeTypeDictionary[packetId]);
                packet.ReadPacket(reader);

                OnPacketReceivedEvent?.Invoke(ServerConnection, packet);
            }
        }

        public void SendPacket(OutboundPacket packet)
        {
            using (var ms = new MemoryStream())
            {
                using (var wr = new BinaryWriter(ms))
                {
                    wr.Write(packet.PacketId);
                    packet.WritePacket(wr);
                }
                ServerConnection.SendMessageRaw(ms.ToArray());
            }
        }
    }
}
