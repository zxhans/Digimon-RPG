using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer convidando outro para Party
    public class PACKET_PARTY_EXIT : OutPacket
    {
        public PACKET_PARTY_EXIT(Tamer t)
            : base(PacketType.PACKET_PARTY_EXIT)
        {
            Write(new byte[6]);
            Write(t.GUID);
            Write(t.Name, 8);
        }
    }
}
