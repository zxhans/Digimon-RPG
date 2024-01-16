using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Client clicou em alguma opção de algum NPC Digimon ("D" Revive, Heal, Status Reset, Warehouse, etc)
    public class PACKET_NPC_SKY : OutPacket
    {
        public PACKET_NPC_SKY(int npcID)
            : base(PacketType.PACKET_NPC_SKY)
        {
            Write(new byte[6]); // Preenchimento
            Write(npcID);
            Write(true);
            Write(new byte[200]); // Preenchimento

        }
    }
}
