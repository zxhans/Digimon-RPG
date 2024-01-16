using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer convidando outro para Party
    public class PACKET_PARTY_INVITY : OutPacket
    {
        public PACKET_PARTY_INVITY(long GUID, string GUIDSufix, long GUID2, string GUIDSufix2, int op)
            : base(PacketType.PACKET_PARTY_INVITY)
        {
            Write(new byte[6]); // Preenchimento

            Write(GUID); // GUID do Líder
            Write(GUIDSufix, 8);

            Write(GUID2); // GUID do Convidado
            Write(GUIDSufix2, 8);

            Write(op);
        }
    }
}
