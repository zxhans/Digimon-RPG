using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote que envia item pego no chão
    public class PACKET_GET_ITEM : OutPacket
    {
        public PACKET_GET_ITEM(ItemMap item)
            : base(PacketType.PACKET_GET_ITEM)
        {
            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 8D AA 01 03 00 00")); // Preenchimento

            // Posição do Item no Mapa
            Write((int)item.Location.X);
            Write((int)item.Location.Y);

            // Posição do Item no Inventário
            Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Linha 
            Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Coluna

            // Escrevendo o Item no pacote
            itemWrite.WriteItem(item.Item, this);

            Write(new byte[104]);
        }

        public PACKET_GET_ITEM(Item item, Vector2 Location)
            : base(PacketType.PACKET_GET_ITEM)
        {
            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 8D AA 01 03 00 00")); // Preenchimento

            // Posição do Item no Mapa
            Write((int)Location.X);
            Write((int)Location.Y);

            // Posição do Item no Inventário
            Write(item.GetLinha()); // Linha 
            Write(item.GetColuna()); // Coluna

            // Escrevendo o Item no pacote
            itemWrite.WriteItem(item, this);

            Write(new byte[104]);
        }

        public PACKET_GET_ITEM(Item item, byte op, byte slot, int linha, int coluna)
            : base(PacketType.PACKET_GET_ITEM)
        {
            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 8D AA")); // Preenchimento

            Write(op);
            Write(slot);
            Write(Utils.StringHex.Hex2Binary("00 00")); // Preenchimento

            // Posição do Item no Inventário
            Write(linha);
            Write(coluna);

            Write(new byte[8]);

            // Escrevendo o Item no pacote
            itemWrite.WriteItem(item, this);

            Write(new byte[104]);
        }

        public PACKET_GET_ITEM(Item item, byte op, byte slot, int l1, int c1, int l2, int c2)
            : base(PacketType.PACKET_GET_ITEM)
        {
            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 8D AA")); // Preenchimento

            Write(op);
            Write(slot);
            Write(Utils.StringHex.Hex2Binary("00 00")); // Preenchimento

            // Posição do Item no Inventário
            Write(l1);
            Write(c1);
            Write(l2);
            Write(c2);

            // Escrevendo o Item no pacote
            itemWrite.WriteItem(item, this);

            Write(new byte[104]);
        }

        public PACKET_GET_ITEM(Item item, Item item2, byte op, byte slot, int l1, int c1, int l2, int c2)
            : base(PacketType.PACKET_GET_ITEM)
        {
            if (item == item2) item2 = null;
            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 8D AA")); // Preenchimento

            Write(op);
            Write(slot);
            Write(Utils.StringHex.Hex2Binary("00 00")); // Preenchimento

            // Posição do Item no Inventário
            Write(l1);
            Write(c1);
            Write(l2);
            Write(c2);

            // Escrevendo os Item no pacote
            itemWrite.WriteItem(item, this);

            itemWrite.WriteItem(item2, this);

            Write(new byte[4]);
        }
    }
}
