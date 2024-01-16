using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Client chocou um Pet Egg
    public class PACKET_PET_ATT : OutPacket
    {
        public PACKET_PET_ATT(Tamer t)
            : base(PacketType.PACKET_PET_ATT)
        {
            Write(new byte[6]); // Preechimento
            Write(t.Pet);
            Write(t.PetHP);
        }
    }
}
