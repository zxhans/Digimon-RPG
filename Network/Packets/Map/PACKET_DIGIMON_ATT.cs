using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_DIGIMON_ATT : OutPacket
    {
        public PACKET_DIGIMON_ATT(Tamer tamer, int i)
            : base(PacketType.PACKET_DIGIMON_ATT)
        {
            if (tamer.Client.Batalha == null && tamer.Digimon[i] != null && tamer.Digimon[i].batalha == null)
            {
                Write(new byte[6]);
                PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
                digimonWrite.WriteDigimon(tamer.Digimon[i], this);
            }

        }

        public PACKET_DIGIMON_ATT(Digimon d)
            : base(PacketType.PACKET_DIGIMON_ATT)
        {
            if (d.Tamer.Client.Batalha == null && d.batalha == null)
            {
                Write(new byte[6]);
                PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
                digimonWrite.WriteDigimon(d, this);
            }

        }

        public PACKET_DIGIMON_ATT(Digimon d, long BattleId, string BattleSufix)
            : base(PacketType.PACKET_DIGIMON_ATT)
        {
            if (d.Tamer.Client.Batalha == null && d.batalha == null)
            {
                Write(new byte[6]);
                PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
                digimonWrite.WriteDigimon(d, BattleId, BattleSufix, this);
            }

        }
    }
}
