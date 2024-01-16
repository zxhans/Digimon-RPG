using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;
using Digimon_Project.Enums;

namespace Digimon_Project.Database.Results
{
    // Classe que usada para obter informações do Evolution Codex
    public class EvolutionCodexResult : ISelectResult
    {
        public bool HasRows;
        public int digimon_id, prev, target, tamer_lvl, tamer_fame, digimon_lvl, battles, win, custo;
        public string[] itens;
        public string digimon_name;

        public void OnExecuted(MySqlDataReader reader)
        {
            HasRows = reader.HasRows;
            if (HasRows)
            {
                reader.Read();

                digimon_id = reader.GetInt32("digimon_id");
                itens = reader.GetString("itens").Split('/');
                prev = reader.GetInt32("prev");
                target = reader.GetInt32("target");
                tamer_lvl = reader.GetInt32("tamer_lvl");
                tamer_fame = reader.GetInt32("tamer_fame");
                digimon_lvl = reader.GetInt32("digimon_lvl");
                battles = reader.GetInt32("battles");
                win = reader.GetInt32("win");
                custo = reader.GetInt32("custo");
                digimon_name = reader.GetString("show_digimon_name");
            }
        }
    }
}
