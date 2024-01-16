using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste 
    public class PACKET_CC68 : OutPacket
    {
        public PACKET_CC68()
            : base(PacketType.PACKET_CC68)
        {
            // O pacote é mesmo vazio
            Write(new byte[6]);

        }
    }
}
