using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Package that sends messages in Normal Chat
    public class PACKET_CHAT_WHISPER : OutPacket
    {
        public PACKET_CHAT_WHISPER(string nick, string remetente, string text)
            : base(PacketType.PACKET_CHAT_WHISPER)
        {
            Write(new byte[6]);
            Write(nick, 21);
            Write(remetente, 21);
            Write(text, 256);
        }
    }
}
