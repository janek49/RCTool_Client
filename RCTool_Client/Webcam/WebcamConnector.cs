using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms.VisualStyles;
using AForge.Video.DirectShow;
using RCTool_Client.Packet.Inbound;
using RCTool_Client.Packet.Outbound;

namespace RCTool_Client.Webcam
{
    public class WebcamConnector
    {
        public string UUID;
        public string IP;
        public int Port;

        private ServerConnection _scon;

        public WebcamConnector(string ip, int port, string uuid)
        {
            this.IP = ip;
            this.Port = port;
            this.UUID = uuid;
        }

        public void Connect()
        {
            _scon?.Disconnect(true);
            _scon = new ServerConnection(IP, Port);
            _scon.PacketHandler = new PacketHandler(_scon);
            _scon.PacketHandler.OnPacketReceivedEvent += PacketHandler_OnPacketReceivedEvent;
            _scon.Connect();
            _scon.PacketHandler.SendPacket(new OutboundPacket01Identify { ConnectionType = 3 });
        }

        private void PacketHandler_OnPacketReceivedEvent(ServerConnection scon, Packet.Inbound.InboundPacket packet)
        {
            Console.WriteLine("Webcam packet " + packet.PacketId);
            if (packet is InboudPacket00RequestData ip0r)
            {
                switch (ip0r.RequestedDataType)
                {
                    case 1:
                        SendWebCamList();
                        break;
                }
            }
        }

        public void Disconnect()
        {
            _scon.Disconnect(true);
        }

        public void SendWebCamList()
        {
            var filter = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            Dictionary<string, string> cams = new Dictionary<string, string>();
            foreach (FilterInfo cam in filter)
            {
                cams.Add(cam.MonikerString, cam.Name);
            }
            OutboundPacket03WebCam packet = new OutboundPacket03WebCam();
            packet.CommandMode = 0;
            packet.WebCamListDictionary = cams;
            _scon.PacketHandler.SendPacket(packet);
        }

        public void TakeAndSendSnapshotInstant(string camId)
        {
            VideoCaptureDevice vcd = new VideoCaptureDevice(camId);
            vcd.NewFrame += (o, e) =>
            {
                vcd.Stop();
                OutboundPacket03WebCam packet = new OutboundPacket03WebCam();
                packet.CommandMode = 1;

                using (var bmp = new Bitmap(e.Frame))
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                    packet.ImageBytes = ms.ToArray();
                }

                _scon.PacketHandler.SendPacket(packet);
            };
            vcd.Start();
        }
    }
}
