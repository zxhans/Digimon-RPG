using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote que envia item no chão
    public class PACKET_CARD_DROP : OutPacket
    {
        public PACKET_CARD_DROP(ItemMap item)
            : base(PacketType.LOGIN_TAMERLIST)
        {
            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 8D AA")); // Preenchimento
            
            // Escrevendo o Item no pacote
            itemWrite.WriteCard(item.Item, this);

            Write((short)item.Location.X); // Pos X
            Write((short)item.Location.Y); // Pos Y
        }

        // Item zerado (item já apanhado)
        public PACKET_CARD_DROP(short X, short Y)
            : base(PacketType.LOGIN_TAMERLIST)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 8D AA")); // Preenchimento

            Write(new byte[64]);

            Write(X); // Pos X
            Write(Y); // Pos Y
        }
    }
}
