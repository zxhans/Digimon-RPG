using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_CCC4 : OutPacket
    {
        public PACKET_CCC4()
            : base(PacketType.PACKET_CCC4)
        {
            // O pacote é mesmo vazio
            Write(new byte[6]);

        }
    }
}
