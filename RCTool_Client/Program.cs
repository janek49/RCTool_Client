using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using RCTool_Client.Packet.Inbound;
using RCTool_Client.Packet.Outbound;

namespace RCTool_Client
{
    static class Program
    {
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
        }

    }
}
