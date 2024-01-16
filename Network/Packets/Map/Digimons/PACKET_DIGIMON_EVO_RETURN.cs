using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_DIGIMON_EVO_RETURN : OutPacket
    {
        public PACKET_DIGIMON_EVO_RETURN(byte result, int itemIdx, int dmSet)
            : base(PacketType.PACKET_DIGIMON_EVO_RETURN)
        {
            Write(new byte[6]);
            byte b = 2;
            Write(result);
            Write(dmSet);
            Write(itemIdx);
            /**/
        }

    }
}
