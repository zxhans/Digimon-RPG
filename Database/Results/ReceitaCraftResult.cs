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
    public class ReceitaCraftResult : ISelectResult
    {
        public string titulo;
        public string receita;
        public bool HasRows;

        public void OnExecuted(MySqlDataReader reader)
        {
            HasRows = reader.HasRows;
            if (reader.HasRows)
            {
                reader.Read();

                titulo = reader.GetString("titulo");
                receita = reader.GetString("receita");
            }
        }
    }
}
