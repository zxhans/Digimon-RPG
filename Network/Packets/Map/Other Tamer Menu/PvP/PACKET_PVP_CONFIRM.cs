using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer desafiando outro para batalha 
    public class PACKET_PVP_CONFIRM : OutPacket
    {
        public PACKET_PVP_CONFIRM(long GUID, string GUIDSufix, byte op, byte op2)
            : base(PacketType.PACKET_BATTLE_START)
        {
            Write(new byte[6]); // Preenchimento

            Write(GUID); // GUID do desafiante
            Write(GUIDSufix, 8);

            Write(op);
            Write(op2); // Operação
        }
    }
}
