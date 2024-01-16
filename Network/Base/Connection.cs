using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Digimon_Project.Game;

namespace Digimon_Project.Network
{
    public class Connection
    {
        private Socket socket = null;
        private int port;
        private Client client;
        private bool isDisconnected = false;
        private byte[] receiveBuffer = new byte[0x3078]; // 12408 bytes
        private byte[] cachedBuffer = new byte[0];
        public string ip;

        public Connection(Socket socket, int port, Client client)
        {
            this.socket = socket;
            this.port = port;
            this.client = client;

            ip = ((IPEndPoint)(socket.RemoteEndPoint)).Address.ToString();
        }

        #region Send

        public void Send(OutPacket p)
        {
            Send(p.GetBuffer());
        }

        public void Send(byte[] buffer)
        {
            if (buffer.Length < 8)
                return; // Nope, client accepts packets with a minimum header of 8 bytes.

            try { socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null); }
            catch { Disconnect(); }
        }

        public void SendCallback(IAsyncResult iAr)
        {
            try { socket.EndSend(iAr); }
            catch { Disconnect(); }
        }

        #endregion

        #region Receive

        public void BeginReceive()
        {
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), null);

            ip = ((IPEndPoint)(socket.RemoteEndPoint)).Address.ToString();
        }

        private void OnReceiveCallback(IAsyncResult iAr)
        {
            try
            {
                int recvLen = socket.EndReceive(iAr);
                if (recvLen <= 0)
                {
                    Disconnect();
                    return;
                }

                byte[] dataBuffer = new byte[recvLen];

                // Grab new received data.
                Array.Copy(receiveBuffer, 0, dataBuffer, 0, dataBuffer.Length);

                // Parse the data.
                byte[] newCacheBuffer = new byte[0];
                ParseData(dataBuffer, cachedBuffer, out newCacheBuffer);
                cachedBuffer = newCacheBuffer;

                // Receive more.
                BeginReceive();
            }
            catch (Exception e) { Console.WriteLine("Exception Caught: {0}", e); Disconnect(); }
        }

        #endregion

        #region Parsing

        private void ParseData(byte[] dataBuffer, byte[] cachedBuffer, out byte[] remainingBuffer)
        {
            byte[] buffer = new byte[dataBuffer.Length + cachedBuffer.Length];
            byte[] packetBuffer;

            bool keepProcessing = (buffer.Length >= 8);
            int offset = 0, unknown, length = 0;

            List<InPacket> receivedPackets = new List<InPacket>();

            Array.Copy(cachedBuffer, 0, buffer, 0, cachedBuffer.Length);
            Array.Copy(dataBuffer, 0, buffer, cachedBuffer.Length, dataBuffer.Length);

            while (keepProcessing)
            {
                unknown = BitConverter.ToInt32(buffer, offset);
                length = BitConverter.ToInt32(buffer, offset + 4);

                if ((buffer.Length - offset) >= length)
                {
                    packetBuffer = new byte[length];

                    Array.Copy(buffer, offset, packetBuffer, 0, packetBuffer.Length);
                    receivedPackets.Add(new InPacket(packetBuffer));

                    offset += packetBuffer.Length;
                }
                else
                {
                    break; // Not enough bytes on the buffer to process this packet.
                }

                keepProcessing = (buffer.Length - offset) >= 8;
            }

            if (offset < buffer.Length)
            {
                remainingBuffer = new byte[buffer.Length - offset];
                Array.Copy(buffer, offset, remainingBuffer, 0, remainingBuffer.Length); // Copy the rest.
            }
            else
            {
                remainingBuffer = new byte[0];
            }

            if (receivedPackets.Count > 0 && OnReceive != null)
            {
                OnReceive(this, new PacketReceivedEventArgs(receivedPackets.ToArray()));
            }
        }

        #endregion

        public void Disconnect()
        {
            if (isDisconnected) return;
            isDisconnected = true;

            Console.WriteLine("Disconnect triggered port: {0}", port);

            if (OnDisconnect != null)
            {
                OnDisconnect(this, new EventArgs());
            }

            try { socket.Close(); }
            catch { }
        }

        #region Events

        public event EventHandler OnDisconnect;
        public event EventHandler<PacketReceivedEventArgs> OnReceive;

        #endregion

    }
}
