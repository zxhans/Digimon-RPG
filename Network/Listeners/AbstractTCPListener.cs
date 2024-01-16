using Digimon_Project.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Digimon_Project.Network.Listeners
{
    public abstract class AbstractTCPListener
    {
        private Socket listener = null;
        private Boolean isActive = false;
        private int listenPort = 0;
        private int maxBacklog = 0;
        private int maxActiveSockets = 0;
        private long totalConnections = 0;
        private long acceptedConnections = 0;

        public readonly List<Client> Clients = new List<Client>();
        public readonly ConcurrentDictionary<uint, ISocketSession> ActiveConnections;

        public AbstractTCPListener(int port, int backlog, int socketLimit)
        {
            ActiveConnections = new ConcurrentDictionary<uint, ISocketSession>();
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenPort = port;
            maxBacklog = backlog;
            maxActiveSockets = socketLimit;
        }

        public bool BindAndListen()
        {
            try
            {
                listener.Bind(new IPEndPoint(IPAddress.Any, listenPort));
                listener.Listen(maxBacklog);
                isActive = true;
                listener.BeginAccept(new AsyncCallback(OnAcceptSocket), null);
                Console.WriteLine("Listening on TCP port {0}.", listenPort);
                return true;
            }
            catch { }
            return false;
        }

        public void Free(ushort slot)
        {
            ISocketSession session = null;
            if (ActiveConnections.ContainsKey(slot))
            {
                ActiveConnections.TryRemove(slot, out session);
            }
        }

        private void OnAcceptSocket(IAsyncResult iAr)
        {
            Socket s = null;

            try { s = listener.EndAccept(iAr); }
            catch { s = null; }

            try
            {
                if (s != null)
                {
                    totalConnections++;

                    if (maxActiveSockets > 0 && ActiveConnections.Count >= maxActiveSockets)
                    {
                        throw new Exception("Socket limit reached.");
                    }

                    if (!IsAccepted(s.RemoteEndPoint))
                    {
                        throw new Exception("Socket rejected by server.");
                    }

                    ushort connectionSlot = 0;
                    while (ActiveConnections.ContainsKey(connectionSlot))
                    {
                        connectionSlot++;
                    }

                    ISocketSession socketSession = OnAccept(acceptedConnections++, connectionSlot, s);
                    ActiveConnections.TryAdd(connectionSlot, socketSession);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket Expcetion: " + e.Message);
                try { s.Close(); }
                catch { }
                finally { s = null; }
            }


            if (listener != null && isActive)
            {
                listener.BeginAccept(new AsyncCallback(OnAcceptSocket), null);
            }
        }

        public abstract bool IsAccepted(EndPoint endPoint);
        public abstract ISocketSession OnAccept(long connectionId, ushort connectionSlot, Socket socket);
    }
}
