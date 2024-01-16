using Digimon_Project.Enums;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // CC 14 Pacote que envia um Check Connection
    public class PACKET_CHECK_CONNECTION : OutPacket
    {
        public PACKET_CHECK_CONNECTION(string username, bool first)
            : base(PacketType.PACKET_CHECK_CONNECTION)
        {
            Write(new byte[4]);
            Write(new byte[] { 0x5D, 0xBF});///Unkow 2 bytes
            Write(username, 21);
            // Boolean que diz se o client deve criar sua tradepassword
            Write(first);
        }

        public PACKET_CHECK_CONNECTION(Tamer tamer)
            : base(PacketType.PACKET_CHECK_CONNECTION)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 73 88"));
            Write(tamer.MapId);
            PACKET_TAMER_DIGIMON_WRITER tamerWriter = new PACKET_TAMER_DIGIMON_WRITER();
            tamerWriter.WriteTamer(tamer, this);
        }
    }
}
