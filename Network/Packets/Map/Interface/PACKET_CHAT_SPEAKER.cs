using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Package that sends messages in Normal Chat
    public class PACKET_CHAT_SPEAKER : OutPacket
    {
        public PACKET_CHAT_SPEAKER(string text)
            : base(PacketType.PACKET_CHAT_SHOUT)
        {
            Write(new byte[6]);
            Write(text, 256);
        }

        public PACKET_CHAT_SPEAKER(string text, bool teste)
            : base(PacketType.PACKET_CHAT_SHOUT)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 64 D5"));
            Write("DIGIMONCENTER", 23);
            Write(text, 258);
        }

        public PACKET_CHAT_SPEAKER(byte tipo, string text, int tamanho)
            : base(PacketType.PACKET_CHAT_SPEAKER)
        {
            /*
            SYSTEM_MESSAGE_TYPE_NONE		= 0,
  			SYSTEM_MESSAGE_TYPE_CHAT		= 1,	// Ã¤ÆÃÃ¢¿¡ º¸¿©Áö´Â ¸Þ½ÃÁö
  			SYSTEM_MESSAGE_TYPE_ADMINSHOUT	= 2,	// ¿î¿µÀÚ ¿ÜÄ¡±â Ã¢¿¡ º¸¿©Áö´Â ¸Þ¼¼Áö
  			SYSTEM_MESSAGE_TYPE_WINDOW		= 3,	// À©µµ¿ì¸¦ ¸¸µé¾î¼­ º¸¿©Áö´Â ¸Þ½ÃÁö
			SYSTEM_MESSAGE_TYPE_SHUTDOWN	= 4,	// ¼Ë´Ù¿î Ãß°¡
  			SYSTEM_MESSAGE_TYPE_END,
            */
            Write(new byte[6]);
            Write(tipo);
            Write(text, tamanho);
        }
    }
}
