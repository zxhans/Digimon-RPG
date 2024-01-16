using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Package that sends messages in Normal Chat
    public class PACKET_CHAT_SHOUT : OutPacket
    {
        public PACKET_CHAT_SHOUT(string nick, string text, short Lvl, short unk)
            : base(PacketType.PACKET_CHAT_SHOUT)
        {
            Write(new byte[6]);
            Write(nick, 21);
            Write(Lvl);
            Write(text, 256);
            Write(unk);
        }

        public PACKET_CHAT_SHOUT(string text)
            : base(PacketType.PACKET_CHAT_SHOUT)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 64 D5"));
            Write("DIGIMONCENTER", 23);
            Write(text, 258);
        }
    }
}
