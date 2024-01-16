using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Líder passando sua liderança para outro membro da Party
    [PacketHandler(Type = PacketType.PACKET_PARTY_KICK, Connection = ConnectionType.Map)]
    public class HANDLE_PARTY_KICK : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            /**/
            byte[] trash = packet.ReadBytes(6);
            string nick = packet.ReadString(20);
            
            if (sender.Tamer.Party != null && sender.Tamer.Party.Lider == sender.Tamer)
                sender.Tamer.Party.RemoveTamer(nick);
            /**/

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

        }
    }
}
