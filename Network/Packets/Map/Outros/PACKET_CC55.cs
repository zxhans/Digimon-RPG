using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_CC55 : OutPacket
    {
        public PACKET_CC55(int id, byte[] trash2)
            : base(PacketType.PACKET_CC55)
        {
            // O pacote é mesmo vazio
            Write(new byte[10]);
            Write(id);
            Write(trash2);

        }
    }
}
