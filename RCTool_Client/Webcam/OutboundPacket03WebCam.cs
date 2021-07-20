using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RCTool_Client.Packet.Outbound;

namespace RCTool_Client.Webcam
{
    public class OutboundPacket03WebCam : OutboundPacket
    {
        public short CommandMode;

        public Dictionary<string, string> WebCamListDictionary;

        public byte[] ImageBytes;

        public OutboundPacket03WebCam() : base(3)
        {
        }

        public override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CommandMode);
            switch (CommandMode)
            {
                case 0: //get webcam list
                {
                    writer.Write((int)WebCamListDictionary.Count);
                    foreach (var keyPair in WebCamListDictionary)
                    {
                        writer.Write(keyPair.Key);
                        writer.Write(keyPair.Value);
                    }
                    break;
                }
                case 1: //get snapshot
                {
                    writer.Write((int)ImageBytes.Length);
                    writer.Write(ImageBytes);
                    break;
                }
            }
        }
    }
}
