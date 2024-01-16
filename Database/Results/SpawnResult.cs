using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em um objeto Tamer
    public class SpawnResult : ISelectResult
    {
        public readonly List<Spawn> spawns = new List<Spawn>();
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

                    Spawn parsedSpawn = new Spawn(id)
                    {
                        Bloqueado = reader.GetBoolean("bloqueado"),

                        Name = reader.GetString("name"),
                        Model = reader.GetUInt16("model"),
                        Level = reader.GetUInt16("level"),
                        DigimonId = reader.GetInt32("digimon_id"),

                        Tempo = reader.GetInt32("tempo"),
                        Fixo = reader.GetInt32("fixo"),
                        Lider = reader.GetInt32("lider"),
                        Ajudantes = new List<Spawn>(),

                        type = reader.GetInt32("type"),
                        estage = reader.GetInt16("estage"),
                        map_id = reader.GetUInt16("map_id"),
                        Location = new Game.Data.Vector2(reader.GetUInt16("location_x"), reader.GetUInt16("location_y")),
                        Pos = new Game.Data.Vector2(reader.GetUInt16("location_x"), reader.GetUInt16("location_y")),
                        lastPos = new Game.Data.Vector2(reader.GetUInt16("location_x"), reader.GetUInt16("location_y")),
                        rank = reader.GetInt32("rank"),
                        move = reader.GetUInt16("move"),
                        Quant = reader.GetInt32("quant"),
                        QuantMin = reader.GetInt32("mquant"),
                        Drop = reader.GetString("item_drop"),

                        skill1 = Utils.Skill.GetSkill(reader.GetInt32("skill1")),
                        skill2 = Utils.Skill.GetSkill(reader.GetInt32("skill2")),

                        // Stats
                        statsLevel = new int[] { reader.GetInt16("str"), reader.GetInt16("dex")
                        , reader.GetInt16("con"), reader.GetInt16("inte")},
                    };

                    if (parsedSpawn.Lider == 0)
                        parsedSpawn.CallAllies();

                    spawns.Add(parsedSpawn);
                }
            }
        }
    }
}
