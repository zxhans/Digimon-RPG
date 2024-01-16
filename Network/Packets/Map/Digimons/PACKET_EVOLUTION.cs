using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Responding to the evolution process
    public class PACKET_EVOLUTION : OutPacket
    {
        public PACKET_EVOLUTION(byte result, int custo)
            : base(PacketType.PACKET_EVOLUTION)
        {
            Write(new byte[7]); // Fill
            Write(result);
            Write(new byte[54]); // Fill
            Write(custo);
        }
    }
}
