using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote que envia informação de Level UP durante a batalha
    public class PACKET_BATTLE_LVLUP : OutPacket
    {
        public PACKET_BATTLE_LVLUP(Tamer tamer, int i)
            : base(PacketType.PACKET_BATTLE_LVLUP)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(tamer.Digimon[i], this);

        }

        public PACKET_BATTLE_LVLUP(Digimon d)
            : base(PacketType.PACKET_BATTLE_LVLUP)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, this);

        }
    }
}
