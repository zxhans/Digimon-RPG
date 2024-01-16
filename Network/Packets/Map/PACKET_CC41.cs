using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_CC41 : OutPacket
    {
        public PACKET_CC41(Tamer tamer, int i)
            : base(PacketType.PACKET_CC41)
        {
            Write(new byte[6]);
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(tamer.Digimon[i], this);

        }

        public PACKET_CC41(Digimon d)
            : base(PacketType.PACKET_CC41)
        {
            Write(new byte[6]);
            Write(Utils.StringHex.Hex2Binary("03 00 00 00 B0 60 08 00 2D 02 00 00 0B 02 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 01 00 00 00 50 C3 00 00 50 C3 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 01 00 00 00 00 00 00 00 3E 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 01 00 01 00"));
            Write(Utils.StringHex.Hex2Binary("DD 62 08 00 00 00 00 00"));
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, this);

        }
    }
}
