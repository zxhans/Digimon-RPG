using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_TELEPORT : OutPacket
    {
        public PACKET_TELEPORT(int map, short x, short y)
            : base(PacketType.PACKET_TELEPORT)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 9F 84"));
            Write(map); // Map_ID
            Write(x); // X
            Write(y); // Y

        }
        public PACKET_TELEPORT(Tamer tamer)
            : base(PacketType.PACKET_TELEPORT)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 9F 84"));
            Write(tamer.MapId); // Map_ID
            Write((short)tamer.Location.X); // X
            Write((short)tamer.Location.Y); // Y

        }
    }
}
