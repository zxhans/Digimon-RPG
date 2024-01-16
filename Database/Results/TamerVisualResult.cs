using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;
using Digimon_Project.Enums;

namespace Digimon_Project.Database.Results
{
    // Classe que usada para obter informações base de um Digimon
    public class TamerVisualResult : ISelectResult
    {
        public int sock = 0;
        public int shoes = 0;
        public int pants = 0;
        public int glove = 0;
        public int tshirt = 0;
        public int jacket = 0;
        public int hat = 0;
        public int customer = 0;

        public void OnExecuted(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int slot = reader.GetInt32("slot");
                    int value = reader.GetInt32("value");
                    switch (slot)
                    {
                        case (int)EquipSlots.sock:
                            sock = value;
                            break;
                        case (int)EquipSlots.shoes:
                            shoes = value;
                            break;
                        case (int)EquipSlots.pants:
                            pants = value;
                            break;
                        case (int)EquipSlots.glove:
                            glove = value;
                            break;
                        case (int)EquipSlots.tshirt:
                            tshirt = value;
                            break;
                        case (int)EquipSlots.jacket:
                            jacket = value;
                            break;
                        case (int)EquipSlots.hat:
                            hat = value;
                            break;
                        case (int)EquipSlots.customer:
                            customer = value;
                            break;
                    }
                }
            }
        }
    }
}
