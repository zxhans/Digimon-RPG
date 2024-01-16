using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_DIGIMON_CATCHED_LOPEN : OutPacket
    {
        public PACKET_DIGIMON_CATCHED_LOPEN(Tamer tamer, int i)
            : base(PacketType.PACKET_DIGIMON_CATCHED_LOPEN)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(tamer.Digimon[i], this);

        }

        public PACKET_DIGIMON_CATCHED_LOPEN(Digimon d)
            : base(PacketType.PACKET_DIGIMON_CATCHED_LOPEN)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, this);

        }
    }
}
