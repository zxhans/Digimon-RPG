using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Package that sends messages in Normal Chat
    public class PACKET_CHAT_GM : OutPacket
    {
        public PACKET_CHAT_GM(string nick, string text)
            : base(PacketType.PACKET_CHAT_GM)
        {
            Write(new byte[6]);
            Write(nick, 21);
            Write(text, 256);
            Write((byte)5);
        }

        public PACKET_CHAT_GM(string text)
            : base(PacketType.PACKET_CHAT_GM)
        {
            Write(new byte[6]);
            Write(Emulator.Enviroment.GMNick, 21);
            Write(text, 256);
            Write((byte)5);
        }

        public PACKET_CHAT_GM(string text, bool teste)
            : base(PacketType.PACKET_CHAT_GM)
        {
            int year = 0;
            int month = 0;
            int day = 0;
            bool hot = true;
            Write(new byte[6]);
            Write(Utils.StringHex.Hex2Binary(text));
            //Write(new byte[2]);
            Write(year);
            Write(month);
            Write(day);
            //Write(hot);
            Write(Utils.StringHex.Hex2Binary("00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00"));
        }
    }
}
