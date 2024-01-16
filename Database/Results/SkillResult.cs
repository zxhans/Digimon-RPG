using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em um objeto Tamer
    public class SkillResult : ISelectResult
    {
        public Skill skill { get; private set; }
        public bool IsValid { get; private set; }

        public void OnExecuted(MySqlDataReader reader)
        {
            int id = 0;

            IsValid = reader.HasRows;
            if (IsValid)
            {
                reader.Read();
                id = reader.GetInt32("id");

                Skill parsedSkill = new Skill(id)
                {
                    Name = reader.GetString("Name"),
                    Lvl = reader.GetInt32("Lvl"),
                    Poder = reader.GetInt32("power"),
                    Units = reader.GetInt32("Units"),
                    Range = reader.GetInt32("range_type"),
                    VP = reader.GetInt32("vp"),
                };

                skill = parsedSkill;
            }
        }
    }
}
