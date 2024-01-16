using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado durante a batalha
    public class PACKET_BATTLE_CHECK : OutPacket
    {
        public PACKET_BATTLE_CHECK()
            : base(PacketType.PACKET_BATTLE_CHECK)
        {
            // Este pacote é realmente vazio, mas sem ele o client fecha.
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));

        }
    }
}
