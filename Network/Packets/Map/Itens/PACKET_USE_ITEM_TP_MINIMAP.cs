using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_USE_ITEM_TP_MINIMAP : OutPacket
    {
        public PACKET_USE_ITEM_TP_MINIMAP(Item i, int x, int y, int op)
            : base(PacketType.PACKET_USE_ITEM_TP_MINIMAP)
        {
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();

            Write(new byte[6]); // Preenchimento
            Write(x);
            Write(y);
            Write(op);
            itemWriter.WriteItem(i, this);
            Write(op);
        }
    }
}
