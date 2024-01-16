using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;
using Digimon_Project.Enums;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em uma lista de objetos Tamer
    public class DigimonsResult : ISelectResult
    {
        public readonly List<Digimon> digimonList = new List<Digimon>();

        public void OnExecuted(MySqlDataReader reader)
        {
            int slot = 0;
            int id = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    slot = reader.GetInt32("digimon_slot");

                    Digimon parsedDigimon = new Digimon(slot, id)
                    {
                        Name = reader.GetString("digimon_name"),
                        Model = reader.GetUInt16("digimon_model"),
                        DigimonId = reader.GetInt32("digimon_id"),
                        RookieForm = reader.GetInt32("rookie"),
                        ChampForm = reader.GetInt32("champ"),
                        UltimForm = reader.GetInt32("ultim"),
                        MegaForm = reader.GetInt32("mega"),
                        OriName = reader.GetString("OriName"),

                        Level = reader.GetUInt16("digimon_level"),
                        XP = reader.GetInt64("digimon_xp"),
                        Battles = reader.GetInt32("battles"),
                        BattleWins = reader.GetInt32("wins"),
                        MaxXP = Utils.XP.MaxForDigimonLevel(reader.GetUInt16("digimon_level")),

                        Health = reader.GetInt32("hp"),
                        VP = reader.GetInt32("vp"),
                        EV = reader.GetInt32("evp"),

                        // Stats
                        MyStrength = reader.GetInt16("digimon_strength"),
                        MyDexterity = reader.GetInt16("digimon_dexterity"),
                        MyConstitution = reader.GetInt16("digimon_constitution"),
                        MyIntelligence = reader.GetInt16("digimon_intelligence"),
                        // Champion Stats
                        MyCStrength = reader.GetInt16("digimon_Cstrength"),
                        MyCDexterity = reader.GetInt16("digimon_Cdexterity"),
                        MyCConstitution = reader.GetInt16("digimon_Cconstitution"),
                        MyCIntelligence = reader.GetInt16("digimon_Cintelligence"),
                        // Ultimate Stats
                        MyUStrength = reader.GetInt16("digimon_Ustrength"),
                        MyUDexterity = reader.GetInt16("digimon_Udexterity"),
                        MyUConstitution = reader.GetInt16("digimon_Uconstitution"),
                        MyUIntelligence = reader.GetInt16("digimon_Uintelligence"),
                        // Mega Stats
                        MyMStrength = reader.GetInt16("digimon_Mstrength"),
                        MyMDexterity = reader.GetInt16("digimon_Mdexterity"),
                        MyMConstitution = reader.GetInt16("digimon_Mconstitution"),
                        MyMIntelligence = reader.GetInt16("digimon_Mintelligence"),

                        estage = reader.GetInt16("estage"),

                        skill1 = Utils.Skill.GetSkill(reader.GetInt32("skill1")),
                        skill2 = Utils.Skill.GetSkill(reader.GetInt32("skill2")),
                        skill1lvl = reader.GetInt16("skill1lvl"),
                        skill2lvl = reader.GetInt16("skill2lvl"),
                        // Champion skills
                        Cskill1lvl = reader.GetInt16("Cskill1lvl"),
                        Cskill2lvl = reader.GetInt16("Cskill2lvl"),
                        // Ultimate skills
                        Uskill1lvl = reader.GetInt16("Uskill1lvl"),
                        Uskill2lvl = reader.GetInt16("Uskill2lvl"),
                        // Mega skills
                        Mskill1lvl = reader.GetInt16("Mskill1lvl"),
                        Mskill2lvl = reader.GetInt16("Mskill2lvl"),

                        type = reader.GetInt16("tipo"),

                        // Stats
                        statsLevel = new int[] { reader.GetInt16("str"), reader.GetInt16("dex")
                        , reader.GetInt16("con"), reader.GetInt16("inte")},

                        Digistore = reader.GetInt16("digistore"),

                        // Usado em batalha
                        BattleId = id + (long)Constants.Battle_Id,// Id em batalha (vamos usar o mesmo Id)
                                                                  // A constante somada serve apenas para
                                                                  // diferenciar o ID dos Digimon de Tamers,
                                                                  // do ID de Spawns, que vai sempre de 1 a 5.
                                                                  // Logo, evitaremos bugs caso um Digimon de
                                                                  // Tamer tenha um ID menor que 5.
                        BattleSufix = reader.GetString("digimon_name")
                        .Substring(0, reader.GetString("digimon_name").Length <= 8 
                        ? reader.GetString("digimon_name").Length : 8), // O Sufixo acompanha o Battle_Id
                                                                                        // O ID total tem 16 bytes.
                    };

                    if (parsedDigimon.Level > Utils.XP.MaxDigimonLevel())
                        parsedDigimon.Level = (ushort)Utils.XP.MaxDigimonLevel();

                    digimonList.Add(parsedDigimon);
                }
            }
        }
    }
}
