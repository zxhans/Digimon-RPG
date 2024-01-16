using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer convidando outro para Party
    public class PACKET_PARTY_LIST : OutPacket
    {
        public PACKET_PARTY_LIST(Party party)
            : base(PacketType.PACKET_PARTY_LIST)
        {
            Write(new byte[6]);

            WriteTamer(party.Lider, party.Lider);

            // Outros integrantes
            for(int i = 0; i < 4; i++)
            {
                if(party.Tamers.Count > i)
                {
                    WriteTamer(party.Tamers[i], party.Lider);
                }
                else
                {
                    WriteTamer(null, party.Lider);
                }
            }

            Write(new byte[4]);
        }

        // Função para escrever os Tamers
        private void WriteTamer(Tamer t, Tamer lider)
        {
            if(t != null)
            {
                Write(t.GUID); // GUID
                Write(t.Name, 8); // GUIDSufix
                Write(t.Name, 21);
                Write((byte)t.Model);
                Write(Utils.StringHex.Hex2Binary("70 00"));
                Write(t == lider ? 1 : 0);
                Write(t.Digimon[0].BattleId); // GUID do Digimon
                Write(t.Digimon[0].BattleSufix, 8);
                Write(t.Digimon[0].Name, 22);
                Write(t.Digimon[0].Model);
                Write((int)t.Digimon[0].Level);
            }
            else
            {
                Write(new byte[88]);
            }
        }
    }
}
