using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Pacote que envia mensagens no Normal Chat
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
    }
}
