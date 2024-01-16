using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Digimon_Project.Network.Listeners
{
    public abstract class AbstractUDPListener
    {
        private Socket listener;
        private int port;
        private byte[] receiveBuffer = new byte[0x6000];
        private EndPoint endPoint = new IPEndPoint(IPAddress.Any, 1);

        public AbstractUDPListener(int port)
        {
            this.port = port;
        }

        public abstract void OnReceive(byte[] buffer, IPEndPoint endPoint);

        public bool BindAndListen()
        {
            bool result = false;

            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                listener.Bind(new IPEndPoint(IPAddress.Any, port));
                listener.BeginReceiveFrom(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(OnDataReceived), null);
                Console.WriteLine("Listening on UDP port {0}.", port);
                result = true;
            }
            catch { }

            return result;
        }

        private void OnDataReceived(IAsyncResult iAr)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.None, 0);
            int bytesReceived = 0;
            try
            {
                bytesReceived = listener.EndReceiveFrom(iAr, ref endPoint);

                if (bytesReceived > 0)
                {
                    byte[] receivedBuffer = new byte[bytesReceived];
                    Array.Copy(receiveBuffer, 0, receivedBuffer, 0, receivedBuffer.Length);
                    Console.WriteLine("Received: {0} bytes on UDP FROM {1}.", bytesReceived, endPoint);
                    OnReceive(receivedBuffer, endPoint as IPEndPoint);
                }
            }
            catch { }

            listener.BeginReceiveFrom(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(OnDataReceived), null);
        }
    }
}
