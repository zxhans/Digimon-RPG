using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado durante a Digievolução em batalha
    public class PACKET_BATTLE_RECALL : OutPacket
    {
        public PACKET_BATTLE_RECALL(Digimon d, int pos)
            : base(PacketType.PACKET_BATTLE_RECALL)
        {
            Write(new byte[6]);
            Write(Utils.StringHex.Hex2Binary("02"));
            Write((byte) pos); // Posição do Digimon no campo de batalha
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, this);

        }
    }
}
