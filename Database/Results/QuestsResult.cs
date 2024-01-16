using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em uma lista de objetos Tamer
    public class QuestsResult : ISelectResult
    {
        public readonly List<Quest> questList = new List<Quest>();

        public void OnExecuted(MySqlDataReader reader)
        {
            int id = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32("quest_id");

                    Quest parsedQuest = new Quest(id)
                    {
                        QuestName = reader.GetString("QuestName"),
                        Andamento = reader.GetInt32("Andamento"),
                        Tipo = reader.GetString("Tipo"),
                        Objetivo = reader.GetString("Objetivo"),
                    };

                    questList.Add(parsedQuest);
                }
            }
        }
    }
}
