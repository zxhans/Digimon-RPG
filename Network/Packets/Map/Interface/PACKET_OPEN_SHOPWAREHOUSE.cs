using System;
using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Authenticating Tradepassword
    public class PACKET_OPEN_SHOPWAREHOUSE : OutPacket
    {
        public PACKET_OPEN_SHOPWAREHOUSE(Tamer tamer)
            : base(PacketType.PACKET_OPEN_SHOPWAREHOUSE)
        {
            Write(new byte[6]);

            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();

            Item[] CashShopItems;
            Item[] CashShopCards;

            // Carregando Inventário de Cards
            CashSopWareItemsResult warecards = Emulator.Enviroment.Database.Select<CashSopWareItemsResult>(
                "i.id AS item_id, i.id AS item_slot, c.item_idx AS ItemId"
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
            //ORGANIZADOR
            for (int i = 0; i < 24; i++)
            {
                if (CashShopCards[i] == null)
                {
                    for (int j=i+1; j < CashShopCards.Length; j++)
                    {
                        if (CashShopCards[j] != null)
                        {
                            CashShopCards[i] = CashShopCards[j];
                            CashShopCards[j] = null;
                            break;
                        }
                    }
                }
            }

            // Carregando inventário da Warehouse
            CashSopWareItemsResult wareitems = Emulator.Enviroment.Database.Select<CashSopWareItemsResult>(
                "i.id AS item_id, i.id AS item_slot, c.item_idx AS ItemId"
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
            //ORGANIZADOR
            for (int i = 0; i < 24; i++)
            {
                if (CashShopItems[i] == null)
                {
                    for (int j = i + 1; j < CashShopItems.Length; j++)
                    {
                        if (CashShopItems[j] != null)
                        {
                            CashShopItems[i] = CashShopItems[j];
                            CashShopItems[j] = null;
                            break;
                        }
                    }
                }
            }

            // Cards
            for (int i = 0; i < 24; i++)
                itemWrite.WriteCard(CashShopCards[i], this);

            // Itens
            for (int i = 0; i < 24; i++)
                itemWrite.WriteItem(CashShopItems[i], this);

            Write(new byte[1]);

        }
    }
}
