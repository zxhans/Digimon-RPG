using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Client clicou em alguma opção de algum NPC Digimon ("D" Revive, Heal, Status Reset, Warehouse, etc)
    public class PACKET_NPC_DIGIMON : OutPacket
    {
        public PACKET_NPC_DIGIMON(int op, double bits, Digimon d)
            : base(PacketType.PACKET_NPC_DIGIMON)
        {
            Write(new byte[6]); // Preenchimento
            Write((long)op);
            Write(bits);
            //Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 80 76 40")); // Teste

            PACKET_DIGIMON_WRITER dWriter = new PACKET_DIGIMON_WRITER();
            dWriter.WriteDigimon(d, this);
        }
        public PACKET_NPC_DIGIMON(int op, double bits, int id, Digimon d)
            : base(PacketType.PACKET_NPC_DIGIMON)
        {
            Write(new byte[6]); // Preenchimento
            Write((long)op);
            Write(bits);
            //Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 80 76 40")); // Teste

            PACKET_DIGIMON_WRITER dWriter = new PACKET_DIGIMON_WRITER();
            dWriter.WriteNullId(d, id, this);
        }
    }
}
