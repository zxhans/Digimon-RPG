using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer convidando outro para Party
    public class PACKET_PARTY_CHECK : OutPacket
    {
        public PACKET_PARTY_CHECK(Tamer t)
            : base(PacketType.PACKET_PARTY_CHECK)
        {
            Write(new byte[6]);
            Write(t.MapId);
            Write(t.Name, 24);
            Write((int)t.Location.X);
            Write((int)t.Location.Y);
        }
    }
}
