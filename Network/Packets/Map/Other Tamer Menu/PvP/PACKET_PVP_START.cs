using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer desafiando outro para batalha 
    public class PACKET_PVP_START : OutPacket
    {
        public PACKET_PVP_START(long GUID, string GUIDSufix, int op)
            : base(PacketType.HANDLE_PACKET_START_BATTLE)
        {
            Write(new byte[6]); // Preenchimento

            Write(op); // Operação

            Write(GUID); // GUID do desafiante
            Write(GUIDSufix, 8);
        }
    }
}
