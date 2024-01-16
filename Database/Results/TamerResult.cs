using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em um objeto Tamer
    public class TamerResult : ISelectResult
    {
        public Tamer Tamer { get; private set; }
        public bool IsValid { get; private set; }

        public void OnExecuted(MySqlDataReader reader)
        {
            int slot = 0;
            int id = 0;

            IsValid = reader.HasRows;
            if (IsValid)
            {
                reader.Read();
                id = reader.GetInt32("tamer_id");
                slot = reader.GetInt32("tamer_slot");

                Tamer parsedTamer = new Tamer(slot, id)
                {
                    Name = reader.GetString("tamer_name"),
                    Model = (byte)reader.GetUInt32("tamer_model"),

                    Level = reader.GetUInt16("tamer_level"),
                    XP = reader.GetInt32("tamer_xp"),
                    MaxXP = Utils.XP.MaxForTamerLevel(reader.GetUInt16("tamer_level")),

                    Battles = reader.GetInt32("tamer_battles"),
                    Wins = reader.GetInt32("tamer_wins"),
                    Reputation = reader.GetInt32("reputation"),

                    MapId = reader.GetInt32("map_id"),
                    Location = new Game.Data.Vector2(reader.GetUInt16("location_x"), reader.GetUInt16("location_y")),
                    
                    /**
                    Sock = reader.GetInt32("sock"),
                    Shoes = reader.GetInt32("shoes"),
                    Pants = reader.GetInt32("pants"),
                    Glove = reader.GetInt32("glove"),
                    Tshirt = reader.GetInt32("tshirt"),
                    Jacket = reader.GetInt32("jacket"),
                    Hat = reader.GetInt32("hat"),
                    Customer = reader.GetInt32("customer"),
                    /**/

                    Rank = reader.GetInt32("rank"),
                    Bits = reader.GetInt64("bits"),

                    Gold = reader.GetInt64("medals"),
                    Coin = reader.GetInt64("coins"),

                    Authority = reader.GetInt32("authority"),

                    Pet = reader.GetInt32("pet_type"),
                    PetHP = reader.GetInt32("pet_hp"),

                    Digistore = reader.GetInt32("digistore"),

                    GUID = id,
                };
                if (parsedTamer.Level > Utils.XP.MaxTamerLevel())
                    parsedTamer.Level = (ushort)Utils.XP.MaxTamerLevel();

                Tamer = parsedTamer;
            }
        }
    }
}
