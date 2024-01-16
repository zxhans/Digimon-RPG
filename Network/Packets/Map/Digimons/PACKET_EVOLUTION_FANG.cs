using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Responding to the evolution process
    public class PACKET_EVOLUTION_FANG : OutPacket
    {
        public PACKET_EVOLUTION_FANG(byte result, int digimonID)
            : base(PacketType.PACKET_EVOLUTION_FANG)
        {
            Write(new byte[6]); // Fill
            Write((byte)result);
            Write((int)digimonID);

            //PACKET_DIGIMON_WRITER dWriter = new PACKET_DIGIMON_WRITER();
            //dWriter.WriteDigimon(d, this);
        }
    }
}
