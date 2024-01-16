using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado durante a Digievolução em batalha
    public class PACKET_BATTLE_DIGIEVOLUTION : OutPacket
    {
        public PACKET_BATTLE_DIGIEVOLUTION(Tamer tamer, int i)
            : base(PacketType.PACKET_BATTLE_DIGIEVOLUTION)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(tamer.Digimon[i], this);

        }

        public PACKET_BATTLE_DIGIEVOLUTION(Digimon d)
            : base(PacketType.PACKET_BATTLE_DIGIEVOLUTION)
        {
            Write(new byte[6]);
            Write(new byte[8]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, this);

        }
    }
}
