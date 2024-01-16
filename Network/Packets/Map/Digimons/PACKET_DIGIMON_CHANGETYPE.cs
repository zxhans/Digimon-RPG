using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_DIGIMON_CHANGETYPE : OutPacket
    {
        public PACKET_DIGIMON_CHANGETYPE(int dmSetId, int dmNewType)
            : base(PacketType.PACKET_DIGIMON_CHANGETYPE)
        {
            Write(new byte[6]);
            Write(dmSetId);
            Write(dmNewType);
            //Write(itemIdx);
            /**/
        }

    }
}
