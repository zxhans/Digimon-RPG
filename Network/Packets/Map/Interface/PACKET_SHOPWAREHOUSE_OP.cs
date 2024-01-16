using System;
using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Authenticating Tradepassword
    public class PACKET_SHOPWAREHOUSE_OP : OutPacket
    {
        public PACKET_SHOPWAREHOUSE_OP(byte result)
            : base(PacketType.PACKET_SHOPWAREHOUSE_OP)
        {
            Write(new byte[6]);

            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();

            Write((byte)1);

            Item[] CashShopItems;

            ItemsResult wareitems = Emulator.Enviroment.Database.Select<ItemsResult>(
                "i.id AS item_id, i.slot AS item_slot, c.item_idx AS ItemId"
                + ", i.quantity AS ItemQuant"
                + ", c.item_tag AS ItemTag, c.item_type1 AS ItemType, c.item_use_on AS ItemUseOn"
                + ", c.default_max_quantity AS ItemQuantMax, c.required_level AS ItemtamerLvl"
                + ", c.effect_type_1 AS ItemEffect1, c.effect_value_1 AS ItemEffect1Value"
                + ", c.effect_type_2 AS ItemEffect2, c.effect_value_2 AS ItemEffect2Value"
                + ", c.effect_type_3 AS ItemEffect3, c.effect_value_3 AS ItemEffect3Value"
                + ", c.effect_type_4 AS ItemEffect4, c.effect_value_4 AS ItemEffect4Value"
                + ", c.custo"
                + ", c.name AS ItemName"
                , "tamer_inventory AS i"
                , "JOIN item_codex AS c ON c.id = i.item_codex_id AND c.item_tab = 0"
                + " WHERE i.tamer_id=@tamer_id AND i.warehouse = 3"
                , new QueryParameters() { { "tamer_id", 2 } }
                );
            CashShopItems = wareitems.itemList;

            itemWrite.WriteCard(CashShopItems[0], this);

            Write((byte)1);
        }

        public PACKET_SHOPWAREHOUSE_OP(Client sender, Tamer tamer)
           : base(PacketType.PACKET_SHOPWAREHOUSE_OP)
        {
            Write(new byte[6]);

            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();

            Item[] CashShopItems;
            Item[] CashShopCards;

            // Carregando inventário da Warehouse
            CashSopWareItemsResult wareitems = Emulator.Enviroment.Database.Select<CashSopWareItemsResult>(
                "i.id AS item_id, i.slot AS item_slot, c.item_idx AS ItemId"
                + ", i.quantity AS ItemQuant"
                + ", c.item_tag AS ItemTag, c.item_type1 AS ItemType, c.item_use_on AS ItemUseOn"
                + ", c.default_max_quantity AS ItemQuantMax, c.required_level AS ItemtamerLvl"
                + ", c.effect_type_1 AS ItemEffect1, c.effect_value_1 AS ItemEffect1Value"
                + ", c.effect_type_2 AS ItemEffect2, c.effect_value_2 AS ItemEffect2Value"
                + ", c.effect_type_3 AS ItemEffect3, c.effect_value_3 AS ItemEffect3Value"
                + ", c.effect_type_4 AS ItemEffect4, c.effect_value_4 AS ItemEffect4Value"
                + ", c.custo"
                + ", c.name AS ItemName"
                , "tamer_inventory AS i"
                , "JOIN item_codex AS c ON c.id = i.item_codex_id AND c.item_tab = 0"
                + " WHERE i.tamer_id=@tamer_id AND i.warehouse = 3"
                , new QueryParameters() { { "tamer_id", tamer.Id } }
                );
            CashShopItems = wareitems.itemList;

            // Carregando Inventário de Cards
            CashSopWareItemsResult warecards = Emulator.Enviroment.Database.Select<CashSopWareItemsResult>(
                "i.id AS item_id, i.slot AS item_slot, c.item_idx AS ItemId"
                + ", i.quantity AS ItemQuant"
                + ", c.item_tag AS ItemTag, c.item_type1 AS ItemType, c.item_use_on AS ItemUseOn"
                + ", c.default_max_quantity AS ItemQuantMax, c.required_level AS ItemtamerLvl"
                + ", c.effect_type_1 AS ItemEffect1, c.effect_value_1 AS ItemEffect1Value"
                + ", c.effect_type_2 AS ItemEffect2, c.effect_value_2 AS ItemEffect2Value"
                + ", c.effect_type_3 AS ItemEffect3, c.effect_value_3 AS ItemEffect3Value"
                + ", c.effect_type_4 AS ItemEffect4, c.effect_value_4 AS ItemEffect4Value"
                + ", c.custo"
                + ", c.name AS ItemName"
                , "tamer_inventory AS i"
                , "JOIN item_codex AS c ON c.id = i.item_codex_id"
                + "  AND c.item_tab = 1"
                + " WHERE i.tamer_id=@tamer_id AND i.warehouse = 3"
                , new QueryParameters() { { "tamer_id", tamer.Id } }
                );
            CashShopCards = warecards.itemList;

            // Cards
            for (int i = 0; i < 24; i++)
            {
                itemWrite.WriteCard(CashShopCards[i], this);
            }

            // Itens

            for (int i = 0; i < 24; i++)
            {
                itemWrite.WriteItem(CashShopItems[i], this);
            }

            //Write((byte) 1);
            Write(new byte[100]);
            //Utils.Comandos.Send(sender, "PACKET_SHOPWAREHOUSE_OP");
        }


    }
}
