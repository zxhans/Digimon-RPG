using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;
using Digimon_Project.Game;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em um objeto Tamer
    public class TeleportsResult : ISelectResult
    {
        public readonly List<Teleport> teleports = new List<Teleport>();
        public bool IsValid { get; private set; }

        public void OnExecuted(MySqlDataReader reader)
        {
            int id = 0;

            IsValid = reader.HasRows;
            if (IsValid)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32("id");

                    Teleport parsedTp = new Teleport(id)
                    {
                        Location = new Game.Data.Vector2(reader.GetInt16("posx"), reader.GetInt16("posy")),
                        Alvo = reader.GetInt32("alvo"),
                        AlvoX = reader.GetInt32("alvox"),
                        AlvoY = reader.GetInt32("alvoy"),
                        Level = reader.GetInt32("lvl"),
                        Rank = reader.GetInt32("rank"),
                    };

                    teleports.Add(parsedTp);
                }
            }
        }
    }
}
