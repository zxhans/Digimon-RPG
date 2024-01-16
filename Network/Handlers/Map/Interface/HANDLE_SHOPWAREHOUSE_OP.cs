using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido quando o Clietn tira item do ShopWarehouse
    [PacketHandler(Type = PacketType.PACKET_SHOPWAREHOUSE_OP, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_SHOPWAREHOUSE_OP : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            int id = packet.ReadInt(); // Identificador

            int itemidCard = packet.ReadInt(); // Identificador

            byte[] trash = packet.ReadBytes(60); // Preenchimento

            int itemidItem = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            /*
            Utils.Comandos.Send(sender, "Pacote HANDLE_PACKET_SHOPWAREHOUSE_OP");
            Utils.Comandos.Send(sender, "id (n sei): " + id);
            Utils.Comandos.Send(sender, "itemid (db_id do tamers_inv se Card)" + itemidCard);
            Utils.Comandos.Send(sender, "itemid (db_id do tamers_inv se Item)" + itemidItem);
            */


            if (id == 1)
            {
                //EH CARD
                int db_item = itemidCard;

                Item[] CashShopCards;

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
                + " WHERE i.tamer_id=@tamer_id AND i.warehouse = 3 AND i.id=@db_item"
                , new QueryParameters() { { "tamer_id", sender.Tamer.Id }, { "db_item", db_item } }
                );
                CashShopCards = warecards.itemList;

                //Utils.Comandos.Send(sender, "encontrado " + warecards.itemCount + " ");

                for (int i = 0; i < sender.Tamer.Cards.Length; i++)
                {
                    Item item = sender.Tamer.Cards[i];
                    if (item == null && i < 24)
                    {
                        //Utils.Comandos.Send(sender, "Slot vazio: " + i + "/"+ sender.Tamer.Cards.Length);
                        for (int j = 0; j < 24; j++)
                        {
                            if (CashShopCards[j] != null)
                            {
                                Emulator.Enviroment.Database.Delete("tamer_inventory", "WHERE id=@id", new Database.QueryParameters() { { "id", db_item } });
                                sender.Tamer.AddCard(CashShopCards[j].ItemName, CashShopCards[j].ItemQuant, false);
                                Utils.Comandos.Send(sender, "Item enviado para a Mochila!");
                                break;
                            }
                        }
                        sender.Tamer.AtualizarInventario();
                        break;
                    }
                    else if (i >= 24)
                    {
                        Utils.Comandos.Send(sender, "Mochila de Cards cheia!");
                        break;
                    }
                }
            }
            else if (id == 2)
            {
                //EH ITEM
                int db_item = itemidItem;

                Item[] CashShopItems;

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
                , "JOIN item_codex AS c ON c.id = i.item_codex_id"
                + "  AND c.item_tab = 0"
                + " WHERE i.tamer_id=@tamer_id AND i.warehouse = 3 AND i.id=@db_item"
                , new QueryParameters() { { "tamer_id", sender.Tamer.Id }, { "db_item", db_item } }
                );
                CashShopItems = wareitems.itemList;

                //Utils.Comandos.Send(sender, "encontrado " + warecards.itemCount + " ");

                for (int i = 0; i < sender.Tamer.Items.Length; i++)
                {
                    Item item = sender.Tamer.Items[i];
                    if (item == null && i < 24)
                    {
                        //Utils.Comandos.Send(sender, "Slot vazio item: " + i + "/"+ sender.Tamer.Items.Length);
                        //Utils.Comandos.Send(sender, " " + CashShopItems.Length);
                        for (int j = 0; j < 24; j++)
                        {
                            if (CashShopItems[j] != null)
                            {
                                Emulator.Enviroment.Database.Delete("tamer_inventory", "WHERE id=@id", new Database.QueryParameters() { { "id", db_item } });
                                if (CashShopItems[j].ItemQuantMax >= 1000)
                                {
                                    //ADD ITENS DE TEMPO
                                    sender.Tamer.AddItem(CashShopItems[j].ItemName, (CashShopItems[j].ItemQuantMax + 7500), false);
                                }
                                else
                                {
                                    sender.Tamer.AddItem(CashShopItems[j].ItemName, CashShopItems[j].ItemQuant, false);
                                }
                                Utils.Comandos.Send(sender, "Item enviado para a Mochila!");
                                //Utils.Comandos.Send(sender, "Tempo Max" + CashShopItems[j].ItemQuantMax);
                                break;
                            }
                        }
                        sender.Tamer.AtualizarInventario();
                        break;
                    }
                    else if (i >= 24)
                    {
                        Utils.Comandos.Send(sender, "Mochila de Items cheia!");
                        break;
                    }
                }
            }


            sender.Connection.Send(new Packets.PACKET_OPEN_SHOPWAREHOUSE(sender.Tamer));
            sender.Connection.Send(new Packets.PACKET_SHOPWAREHOUSE_OP(sender, sender.Tamer));
            //sender.Connection.Send(new Packets.PACKET_CHECK_TRADE_PASS("teste", "teste", 1, 2));
        }
    }
}
