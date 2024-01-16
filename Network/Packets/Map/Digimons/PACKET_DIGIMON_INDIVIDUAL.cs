using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_DIGIMON_INDIVIDUAL : OutPacket
    {
        public PACKET_DIGIMON_INDIVIDUAL(Tamer tamer, int i)
            : base(PacketType.PACKET_DIGIMON_INDIVIDUAL)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(tamer.Digimon[i], this);
            /**/
        }

        public PACKET_DIGIMON_INDIVIDUAL(Digimon d)
            : base(PacketType.PACKET_DIGIMON_INDIVIDUAL)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, this);
            /**/
        }

        public PACKET_DIGIMON_INDIVIDUAL(Digimon d, long BattleId, string BattleSufix)
            : base(PacketType.PACKET_DIGIMON_INDIVIDUAL)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, BattleId, BattleSufix, this);
            /**/
        }
    }
}
