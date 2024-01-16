﻿using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Package that sends messages in Normal Chat
    public class PACKET_CHAT : OutPacket
    {
        public PACKET_CHAT(string nick, string text, int tamanho)
            : base(PacketType.PACKET_CHAT)
        {
            Write(new byte[6]);
            Write(nick, 21);
            Write(text, tamanho);
        }
    }
}
