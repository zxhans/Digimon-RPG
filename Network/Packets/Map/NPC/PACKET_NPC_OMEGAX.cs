using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Client clicou em alguma opção de algum NPC Digimon ("D" Revive, Heal, Status Reset, Warehouse, etc)
    public class PACKET_NPC_OMEGAX : OutPacket
    {
        public PACKET_NPC_OMEGAX(int op, int op2)
            : base(PacketType.PACKET_NPC_OMEGAX)
        {
            Write(new byte[6]); // Preenchimento
            Write((byte)op);
            Write((byte)op2);
            
            Write((int)435523010);
            Write((byte)1);
            Write((int)0);
            Write((int)0);
            Write((int)0);
            //nao funciona essa bosta, o client nao acieta essa merda
        }
    }


}
