using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em uma lista de objetos Tamer
    public class TamersResult : ISelectResult
    {
        public readonly List<Tamer> tamerList = new List<Tamer>();
        public int tamerCount = 0;

        public void OnExecuted(MySqlDataReader reader)
        {
            int slot = 0;
            int id = 0;
            int digimon_slot = 0;
            int digimon_id = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32("tamer_id");
                    slot = reader.GetInt32("tamer_slot");

                    digimon_id = reader.GetInt32("d_id");
                    digimon_slot = reader.GetInt32("digimon_slot");

                    Tamer parsedTamer = new Tamer(slot, id)
                    {
                        Name = reader.GetString("tamer_name"),
                        Model = (byte)reader.GetUInt32("tamer_model"),
                        Level = reader.GetUInt16("tamer_level"),

                        Battles = reader.GetInt32("tamer_battles"),
                        Wins = reader.GetInt32("tamer_wins"),

                        MapId = reader.GetInt32("map_id"),
                        Location = new Game.Data.Vector2(reader.GetUInt16("location_x"), reader.GetUInt16("location_y")),

                        Rank = reader.GetInt32("rank"),
                        Bits = reader.GetInt64("bits"),

                        Gold = reader.GetInt64("medals"),
                        Coin = reader.GetInt64("coins"),

                        Digimon = new List<Digimon>() { new Digimon(digimon_slot, digimon_id)
                        {
                            Name = reader.GetString("digimon_name"),
                            OriName = reader.GetString("OriName"),
                            Model = reader.GetUInt16("digimon_model"),
                            Level = reader.GetUInt16("digimon_level")
                        } },
                    };
                    if (parsedTamer.Level > Utils.XP.MaxTamerLevel())
                        parsedTamer.Level = (ushort)Utils.XP.MaxTamerLevel();
                    parsedTamer.Atualizar();
                    tamerList.Add(parsedTamer);
                    /**
                    if (slot > 0 && slot <= 4)
                    {
                        tamerList[slot-1] = parsedTamer;
                        parsedTamer.Atualizar();
                        tamerCount++;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                    /**/
                }
            }
        }
    }
}
