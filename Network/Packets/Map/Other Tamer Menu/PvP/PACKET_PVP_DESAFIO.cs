using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer desafiando outro para batalha 
    public class PACKET_PVP_DESAFIO : OutPacket
    {
        public PACKET_PVP_DESAFIO(long GUID, string GUIDSufix, long GUID2, string GUIDSufix2, int op)
            : base(PacketType.PACKET_PVP_DESAFIO)
        {
            Write(new byte[6]); // Preenchimento

            Write(GUID); // GUID do desafiante
            Write(GUIDSufix, 8);

            Write(GUID2); // GUID do desafiado
            Write(GUIDSufix2, 8);

            Write(op);
        }
    }
}
