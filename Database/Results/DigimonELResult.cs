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
    public class DigimonELResult : ISelectResult
    {
        public int[] fases;
        public bool Valid;

        public void OnExecuted(MySqlDataReader reader)
        {
            Valid = reader.HasRows;
            if (Valid)
            {
                reader.Read();

                fases = new int[] { reader.GetInt32("i"), reader.GetInt32("r"), reader.GetInt32("c")
                , reader.GetInt32("u"), reader.GetInt32("m") };
            }
        }
    }
}
