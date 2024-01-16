using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client solicita lista de Evoluções disponíveis em batalha
    [PacketHandler(Type = PacketType.PACKET_BATTLE_EVO_LIST, Connection = ConnectionType.Map)]
    public class HANDLE_BATTLE_EVO_LIST : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unknown = packet.ReadInt();
            short unk = packet.ReadShort();

            int ID = packet.ReadInt(); // ID do digimon Solicitado

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Respondendo o Client
            foreach (Digimon d in sender.Tamer.Digimon)
            {
                if (d != null && d.Id == ID)
                {
                    sender.Connection.Send(new Packets.PACKET_BATTLE_EVO_LIST(d));
                    return;
                }
            }

        }
    }
}
