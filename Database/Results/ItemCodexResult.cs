using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em uma lista de objetos Tamer
    public class ItemCodexResult : ISelectResult
    {
        public readonly Dictionary<String, ItemCodex> codex = new Dictionary<string, ItemCodex>();

        public void OnExecuted(MySqlDataReader reader)
        {
            int id = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32("item_id");

                    ItemCodex parsedItem = new ItemCodex(id)
                    {
                        ItemId = reader.GetInt32("ItemId"),
                        ItemTag = (byte)reader.GetInt32("ItemTag"),
                        ItemType = (byte)reader.GetInt32("ItemType"),
                        ItemUseOn = (byte)reader.GetInt32("ItemUseOn"),
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
                        ItemTab = reader.GetInt16("item_tab"),
                    };

                    if(!codex.ContainsKey(parsedItem.ItemName))
                        codex.Add(parsedItem.ItemName, parsedItem);
                }
            }
        }
    }
}
