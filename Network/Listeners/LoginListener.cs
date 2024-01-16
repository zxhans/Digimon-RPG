using Digimon_Project.Enums;
using Digimon_Project.Game;

using System.Net;
using System.Net.Sockets;

namespace Digimon_Project.Network.Listeners
{
    // Listener que escuta conexões da porta de Login
    // BB FA FF 00
    public class LoginListener : AbstractTCPListener
    {
        private static int port = 65530;

        public LoginListener()
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
            Client c = new Client(connectionSlot, socket, ConnectionType.Login, port);
            Clients.Add(c);
            return c;
        }
    }
}
