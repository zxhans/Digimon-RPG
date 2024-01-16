using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_TESTE : OutPacket
    {
        public PACKET_TESTE()
            : base(PacketType.PACKET_TESTE)
        {
            // O pacote é mesmo vazio
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 FC 81"));
            Write(Utils.StringHex.Hex2Binary("87 70 2B C9 EF 2F 4B 8E 6B 93 CD 3E 02 1F 79 8A"));
        }
    }
}
