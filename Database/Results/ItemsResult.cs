using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;
using Digimon_Project.Enums;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em uma lista de objetos Tamer
    public class ItemsResult : ISelectResult
    {
        //esse seria o max do eSlot? Antes era o ring
        //public readonly Item[] itemList = new Item[(int)EquipSlots.bagexp1 + 1];
        public readonly Item[] itemList = new Item[(int)EquipSlots.ring + 1];
        public int itemCount = 0;

        public void OnExecuted(MySqlDataReader reader)
        {
            int slot = 0;
            int id = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32("item_id");
                    slot = reader.GetInt32("item_slot");

                    Item parsedItem = new Item(slot, id)
                    {
                        ItemId = reader.GetInt32("ItemId"),
                        ItemTag = (byte)reader.GetInt32("ItemTag"),
                        ItemType = (byte)reader.GetInt32("ItemType"),
                        ItemUseOn = (byte)reader.GetInt32("ItemUseOn"),
                        ItemQuant = reader.GetInt32("ItemQuant"),
                        ItemQuantMax = reader.GetInt32("ItemQuantMax"),
                        ItemtamerLvl = reader.GetInt32("ItemtamerLvl"),
                        ItemEffect1 = reader.GetInt32("ItemEffect1"),
                        ItemEffect1Value = reader.GetInt32("ItemEffect1Value"),
                        ItemEffect2 = reader.GetInt32("ItemEffect2"),
                        ItemEffect2Value = reader.GetInt32("ItemEffect2Value"),
                        ItemEffect3 = reader.GetInt32("ItemEffect3"),
                        ItemEffect3Value = reader.GetInt32("ItemEffect3Value"),
                        ItemEffect4 = reader.GetInt32("ItemEffect4"),
                        ItemEffect4Value = reader.GetInt32("ItemEffect4Value"),
                        Custo = reader.GetInt32("custo"),
                        ItemName = reader.GetString("ItemName"),
                    };

                    if (slot > 0 && slot <= itemList.Length)
                    {
                        itemList[slot-1] = parsedItem;
                        itemCount++;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
            }
        }
    }

    public class CashSopWareItemsResult : ISelectResult
    {
        //esse seria o max do eSlot? Antes era o ring
        //public readonly Item[] itemList = new Item[(int)EquipSlots.bagexp1 + 1];
        public readonly Item[] itemList = new Item[(int)EquipSlots.ring + 1];
        public int itemCount = 0;

        public void OnExecuted(MySqlDataReader reader)
        {
            int slot = 0;
            int id = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32("item_id");
                    slot++;

                    Item parsedItem = new Item(slot, id)
                    {
                        ItemId = reader.GetInt32("ItemId"),
                        ItemTag = (byte)reader.GetInt32("ItemTag"),
                        ItemType = (byte)reader.GetInt32("ItemType"),
                        ItemUseOn = (byte)reader.GetInt32("ItemUseOn"),
                        ItemQuant = reader.GetInt32("ItemQuant"),
                        ItemQuantMax = reader.GetInt32("ItemQuantMax"),
                        ItemtamerLvl = reader.GetInt32("ItemtamerLvl"),
                        ItemEffect1 = reader.GetInt32("ItemEffect1"),
                        ItemEffect1Value = reader.GetInt32("ItemEffect1Value"),
                        ItemEffect2 = reader.GetInt32("ItemEffect2"),
                        ItemEffect2Value = reader.GetInt32("ItemEffect2Value"),
                        ItemEffect3 = reader.GetInt32("ItemEffect3"),
                        ItemEffect3Value = reader.GetInt32("ItemEffect3Value"),
                        ItemEffect4 = reader.GetInt32("ItemEffect4"),
                        ItemEffect4Value = reader.GetInt32("ItemEffect4Value"),
                        Custo = reader.GetInt32("custo"),
                        ItemName = reader.GetString("ItemName"),
                    };

                    if (slot > 0 && slot <= itemList.Length)
                    {
                        itemList[slot - 1] = parsedItem;
                        itemCount++;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
            }
        }
    }
}
