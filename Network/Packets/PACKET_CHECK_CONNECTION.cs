using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{
    // Pacote que envia um Check Connection
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
    }
}
