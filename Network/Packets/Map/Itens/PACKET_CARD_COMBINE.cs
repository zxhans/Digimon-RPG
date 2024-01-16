using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_CARD_COMBINE : OutPacket
    {
        public PACKET_CARD_COMBINE(byte result)
            : base(PacketType.PACKET_CARD_COMBINE)
        {
            Write(new byte[6]); // Preenchimento
            Write((byte)result);
        }
    }
}
