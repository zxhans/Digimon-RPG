using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_DELETE_CARD : OutPacket
    {
        public PACKET_DELETE_CARD(byte[] trash, Item item, short token)
            : base(PacketType.PACKET_DELETE_CARD)
        {
            Write(trash);
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
            itemWriter.WriteCard(item, this);
            Write(new byte[64]);
            Write((short)1);
            Write(token);
        }
    }
}
