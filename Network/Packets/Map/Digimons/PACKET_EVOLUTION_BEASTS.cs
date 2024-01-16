using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Responding to the evolution process
    public class PACKET_EVOLUTION_BEASTS : OutPacket
    {
        public PACKET_EVOLUTION_BEASTS(byte type, byte result, int digimonID, short btDigimonID)
            : base(PacketType.PACKET_EVOLUTION_BEASTS)
        {
            Write(new byte[6]); // Fill
            Write(type);
            Write(result);
            Write(btDigimonID);//WORD btDigimonID
            Write(digimonID);
        }
    }
}
