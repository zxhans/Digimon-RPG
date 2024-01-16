using Digimon_Project.Enums;
using Digimon_Project.Game;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 12 Client solicita sua lista de Tamers (Tela de seleção após o login)
    [PacketHandler(Type = PacketType.LOGIN_TAMERLIST, Connection = ConnectionType.Login)]
    public class HANDLE_LOGIN_TAMERLIST : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();
            // Enviando o Packet LOGIN_TAMERLIST
            sender.Connection.Send(new Packets.LOGIN_TAMERLIST(sender.TamerList));
        }
    }
}
