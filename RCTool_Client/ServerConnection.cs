using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RCTool_Client
{
    public class ServerConnection
    {
        public Socket Socket { get; }
        public string Ip;
        public int Port;

        public Queue<byte[]> OutboundMessageQueue = new Queue<byte[]>();
        public Thread OutboundMessageWorkerThread;


        private void OutboundMessageWorker()
        {
            while (true)
                try
                {
                    lock (Socket)
                    {
                        if (!IsConnected())
                            break;

                        if (OutboundMessageQueue.Count > 0)
                        {
                            byte[] message = OutboundMessageQueue.Dequeue();
                            Socket.Send(message);

                            Thread.Sleep(10);
                            Console.WriteLine("Sent Message: " + Encoding.ASCII.GetString(message));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
        }

        public Thread InboundMessageWorkerThread;

        private void InboundMessageWorker()
        {
            while (true)
                try
                {
                    lock (Socket)
                    {
                        if (!IsConnected())
                            break;
                        if (!HasMessageAvail())
                            continue;

                        byte[] message = ReceiveMessageBytes();
                        new Thread(() => PacketHandler.HandleIncomingRawPacket(message)).Start();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
        }

        public PacketHandler PacketHandler;

        public bool IsRunning()
        {
            return InboundMessageWorkerThread.IsAlive && OutboundMessageWorkerThread.IsAlive;
        }

        public ServerConnection(string ip, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Ip = ip;
            this.Port = port;
            OutboundMessageWorkerThread = new Thread(OutboundMessageWorker);
            InboundMessageWorkerThread = new Thread(InboundMessageWorker);
        }

        public void Connect()
        {
            Socket.Connect(Ip, Port);
            OutboundMessageWorkerThread.Start();
            InboundMessageWorkerThread.Start();
        }

        public void Disconnect(bool force = false)
        {
            while (!force && OutboundMessageQueue.Count > 0 && OutboundMessageWorkerThread.IsAlive)
            {

            }
            Socket.Disconnect(true);
            OutboundMessageWorkerThread.Abort();
            InboundMessageWorkerThread.Abort();
        }

        public bool IsConnected()
        {
            bool part1 = Socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (Socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        public bool HasMessageAvail()
        {
            return Socket.Available > 0;
        }

        public byte[] ReceiveMessageBytes()
        {
            return NetUtil.ReceiveAll(Socket);
        }

        public void SendMessage(string message)
        {
            OutboundMessageQueue.Enqueue(Encoding.UTF8.GetBytes(message));

        }

        public void SendMessageRaw(byte[] bytes)
        {
            OutboundMessageQueue.Enqueue(bytes);
        }

    }
}
