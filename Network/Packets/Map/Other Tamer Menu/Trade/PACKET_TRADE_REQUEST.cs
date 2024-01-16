using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer solicitando trade com outro tamer
    public class PACKET_TRADE_REQUEST : OutPacket
    {
        public PACKET_TRADE_REQUEST(long GUID, string GUIDSufix, long GUID2, string GUIDSufix2, int op)
            : base(PacketType.PACKET_TRADE_REQUEST)
        {
            Write(new byte[6]); // Preenchimento

            Write(GUID); // GUID do solicitante
            Write(GUIDSufix, 8);

            Write(GUID2); // GUID do solicitado
            Write(GUIDSufix2, 8);

            Write(op);
        }
    }
}
