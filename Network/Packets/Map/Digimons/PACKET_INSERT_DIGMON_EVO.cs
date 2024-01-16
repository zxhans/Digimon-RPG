using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Responding to the insertion of Digimon in the evolution process
    public class PACKET_INSERT_DIGMON_EVO : OutPacket
    {
        public PACKET_INSERT_DIGMON_EVO(Digimon d)
            : base(PacketType.PACKET_INSERT_DIGMON_EVO)
        {
            Write(new byte[7]); // Fill
            Write((byte)1);

            // Models of evolutionary phases
            Write((short)d.RModel);
            Write((short)d.CModel);
            Write((short)d.UModel);
            Write((short)d.MModel);
        }
    }
}
