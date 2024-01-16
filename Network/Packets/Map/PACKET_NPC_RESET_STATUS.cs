using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Client usou o Reset Status (NPC D)
    public class PACKET_NPC_RESET_STATUS : OutPacket
    {
        public PACKET_NPC_RESET_STATUS(int id, double bits, int result)
            : base(PacketType.PACKET_NPC_RESET_STATUS)
        {
            Write(new byte[6]); // Preenchimento
            Write(id); // ID do Digimon
            Write(0); // Preenchimento
            Write(bits);
            Write(result); // 1 - Success; 2 - Fail
            Write(0); // Preenchimento
        }
    }
}
