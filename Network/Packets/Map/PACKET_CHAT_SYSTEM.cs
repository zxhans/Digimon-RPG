using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{

    // Pacote que envia mensagens no Normal Chat
    public class PACKET_CHAT_SYSTEM : OutPacket
    {
        public PACKET_CHAT_SYSTEM(string text, int tamanho)
            : base(PacketType.PACKET_CHAT_SYSTEM)
        {
            Write(new byte[6]);
            Write((byte)1);
            Write(text, tamanho);
        }
    }
}
