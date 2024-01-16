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
    public class DigimonBaseResult : ISelectResult
    {
        public Skill skill1;
        public Skill skill2;
        public int[] statsLevel;
        public int estage;
        public int type;
        public int model;
        public int EL;
        public string OriName;

        public void OnExecuted(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                reader.Read();

                skill1 = Utils.Skill.GetSkill(reader.GetInt32("skill1"));
                skill2 = Utils.Skill.GetSkill(reader.GetInt32("skill2"));
                statsLevel = new int[] { reader.GetInt16("str"), reader.GetInt16("dex")
                        , reader.GetInt16("con"), reader.GetInt16("inte")};
                estage = reader.GetInt16("estage");
                type = reader.GetInt16("tipo");
                model = reader.GetInt16("model");
                EL = reader.GetInt32("evolution_line");
                OriName = reader.GetString("OriName");
            }
        }
    }
}
