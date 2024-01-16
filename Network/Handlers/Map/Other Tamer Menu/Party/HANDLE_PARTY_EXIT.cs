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
    // Membro saiu da Party
    [PacketHandler(Type = PacketType.PACKET_PARTY_EXIT, Connection = ConnectionType.Map)]
    public class HANDLE_PARTY_EXIT : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            if (sender.Tamer.Party != null)
                sender.Tamer.Party.TamerExit(sender.Tamer);
        }
    }
}
