using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Package that sends messages in Normal Chat
    public class PACKET_CHAT_SYSTEM : OutPacket
    {
        public PACKET_CHAT_SYSTEM(string text, int tamanho)
            : base(PacketType.PACKET_CHAT_SYSTEM)
        {
            Write(new byte[6]);
            Write((byte)1);
            Write(text, tamanho);
        }

        public PACKET_CHAT_SYSTEM(string text, int tamanho, byte tipo)
            : base(PacketType.PACKET_CHAT_SYSTEM)
        {
            if (tipo == 2)
            {
                Write(new byte[6]);
                Write((byte)2);
                Write(text, tamanho);
            }
            else
            {
                Write(new byte[6]);
                Write((byte)1);
                Write(text, tamanho);
            }
        }

        public PACKET_CHAT_SYSTEM(byte tipo, string text, int tamanho)
            : base(PacketType.PACKET_CHAT_SYSTEM)
        {
            /*
            SYSTEM_MESSAGE_TYPE_NONE		= 0, 
  			SYSTEM_MESSAGE_TYPE_CHAT		= 1,	// whisper rosa
  			SYSTEM_MESSAGE_TYPE_ADMINSHOUT	= 2,	// ¿shout em cima e no chat mesmo
  			SYSTEM_MESSAGE_TYPE_WINDOW		= 3,	// janela bizarra
			SYSTEM_MESSAGE_TYPE_SHUTDOWN	= 4,	// da dc
  			SYSTEM_MESSAGE_TYPE_END,                // nao funcionou
            */
            Write(new byte[6]);
            Write((byte) tipo);
            Write(text, tamanho);
        }
    }
}
