using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_DELETE_ITEM : OutPacket
    {
        public PACKET_DELETE_ITEM(byte[] trash, Item item)
            : base(PacketType.PACKET_DELETE_ITEM)
        {
            Write(trash);
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
            itemWriter.WriteItem(item, this);
            Write(new byte[100]);
            Write(1);
        }
    }
}
