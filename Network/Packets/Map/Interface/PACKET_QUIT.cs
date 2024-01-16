using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Package sent eventually by the Map login. 
    public class PACKET_QUIT : OutPacket
    {
        public PACKET_QUIT()
            : base(PacketType.PACKET_QUIT)
        {
            // The package is really empty
            Write(new byte[6]);

        }
    }
}
