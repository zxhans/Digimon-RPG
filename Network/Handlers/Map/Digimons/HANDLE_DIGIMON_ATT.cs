using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client solicita atualização de informação de um de seus digimons
    [PacketHandler(Type = PacketType.PACKET_DIGIMON_ATT, Connection = ConnectionType.Map)]
    public class HANDLE_DIGIMON_ATT : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();
            int Id = packet.ReadInt();
            long BattleId = packet.ReadLong();
            string BattleSufix = packet.ReadString(8);

            byte b; // Lendo o restante do pacote (Apesar de conter informações, praticamente na mesma estrutura
            // inclusive, as informações são irrelevantes. Tudo o que precisamos é do ID para responder o Client)
            while (packet.Remaining > 0) b = packet.ReadByte();

            if(sender.Tamer != null)
            {
                if (Id != 0)
                foreach(Digimon d in sender.Tamer.Digimon)
                {
                    if(d.Id == Id)
                    {
                        sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(d));
                        return;
                    }
                }

                if(Id == 0)
                {
                    Digimon d = sender.Tamer.Digimon[sender.Tamer.Digimon.Count - 1];
                    if (d != null)
                    {
                        sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(d, BattleId, BattleSufix));
                        //sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(d));
                    }
                }
            }
        }
    }
}
