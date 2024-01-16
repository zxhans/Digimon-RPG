using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_CCF0 : OutPacket
    {
        public PACKET_CCF0()
            : base(PacketType.PACKET_CCF0)
        {
            // O pacote é mesmo vazio
            Write(new byte[908]);

        }
    }
}
