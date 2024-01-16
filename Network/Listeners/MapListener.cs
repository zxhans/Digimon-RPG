using Digimon_Project.Enums;
using Digimon_Project.Game;

using System.Net;
using System.Net.Sockets;

namespace Digimon_Project.Network.Listeners
{
    // Listener que escuta conexões pela porta do Mapa
    // 43 FC FB FF 00 00
    public class MapListener : AbstractTCPListener
    {
        private static int port = 65531;

        public MapListener()
            : base(port, 100, 1000)
        {

        }

        public override bool IsAccepted(EndPoint endPoint)
        {
            return true;
        }

        // Sempre que o listener aceita uma conexão, ele cria um objeto da classe Client
        public override ISocketSession OnAccept(long connectionId, ushort connectionSlot, Socket socket)
        {
            System.Console.WriteLine("New Connection on Map!!");
            Client c = new Client(connectionSlot, socket, ConnectionType.Other, port);
            Clients.Add(c);
            return c;
        }
    }
}
