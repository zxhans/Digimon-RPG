using System;
using System.IO;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Este é apenas um Writer de pacotes com estrutura de digimons, só para não ficar repetindo a estrutura.
    public class PACKET_ITEM_WRITER
    {
        public void WriteItem(Item item, OutPacket p)
        {
            WriteItem(item, 0, p);
        }
        public void WriteItem(Item item, int quantdecr, OutPacket p)
        {
            if (item != null && item.ItemQuant > 0)
            {
                p.Write(item.Id); // Identificador
                p.Write(item.ItemId); // Item ID
                p.Write(item.ItemTag); // Item TAG
                p.Write(item.ItemType); // Item Type (Norm, Rare...)
                p.Write(Utils.StringHex.Hex2Binary("00 00"));
                p.Write(item.ItemQuant - quantdecr); // Quantidade
                p.Write(item.ItemQuantMax); // Quantidade máxima
                p.Write(item.Custo); // Custo do item
                p.Write(item.Custo/2); // Preço de venda do item
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Preenchimento
                p.Write(item.ItemtamerLvl); // Tamer Level
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Preenchimento
                p.Write(item.ItemEffect1); // Effect 1 ID
                p.Write(item.ItemEffect1Value); // Effect 1 Value
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Preenchimento
                p.Write(item.ItemEffect2); // Effect 2 ID
                p.Write(item.ItemEffect2Value); // Effect 2 Value
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Preenchimento
                p.Write(item.ItemEffect3); // Effect 3 ID
                p.Write(item.ItemEffect3Value); // Effect 3 Value
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Preenchimento
                p.Write(item.ItemEffect4); // Effect 4 ID
                p.Write(item.ItemEffect4Value); // Effect 4 Value
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 0F 27 00 7D")); // Finalizador
                p.Write(item.Id + item.ItemId); // Identificador + Item ID
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Preenchimento
            }
            else p.Write(new byte[100]);
        }

        public void WriteCard(Item item, OutPacket p)
        {
            if (item != null && item.ItemQuant > 0)
            {
                p.Write(item.Id); // Identificador
                p.Write(item.ItemId); // Item ID
                p.Write(item.ItemTag); // Item TAG
                p.Write(item.ItemType); // Item Type (Norm, Rare...)
                p.Write(Utils.StringHex.Hex2Binary("00"));
                p.Write(item.ItemUseOn);
                p.Write((byte)item.ItemQuant); // Quantidade
                p.Write((byte)item.ItemQuantMax); // Quantidade máxima
                p.Write(Utils.StringHex.Hex2Binary("00 00")); // Preenchimento
                p.Write(item.Custo); // Custo
                p.Write(item.Custo / 2); // Preço de venda
                p.Write(item.ItemEffect1); // Effect 1 ID
                p.Write(item.ItemEffect1Value); // Effect 1 Value
                p.Write(item.ItemEffect2); // Effect 2 ID
                p.Write(item.ItemEffect2Value); // Effect 2 Value
                p.Write(item.ItemEffect3); // Effect 3 ID
                p.Write(item.ItemEffect3Value); // Effect 3 Value
                p.Write(item.ItemEffect4); // Effect 4 ID
                p.Write(item.ItemEffect4Value); // Effect 4 Value
                p.Write(Utils.StringHex.Hex2Binary("0F 27 00 7D")); // Finalizador
                p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Preenchimento
            }
            else p.Write(new byte[64]);
        }
    }
}
