using Digimon_Project.Enums;
using Digimon_Project.Game;

using System.Net;
using System.Net.Sockets;

namespace Digimon_Project.Network.Listeners
{
    // Listener que escuta conexões pela porta 7000
    public class LoginListener7000 : AbstractTCPListener
    {
        private static int port = 7000;

        public LoginListener7000()
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
            System.Console.WriteLine("New Connection on Port 7000!!");
            Client c = new Client(connectionSlot, socket, ConnectionType.Other, port);
            return c;
        }
    }
}
