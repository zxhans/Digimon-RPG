using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em uma lista de objetos RankConfig
    public class RankConfigResult : ISelectResult
    {
        public readonly Dictionary<int, RankConfig> rankConfig = new Dictionary<int, RankConfig>();
        public readonly Dictionary<string, RankConfig> rankConfigName = new Dictionary<string, RankConfig>();
        public readonly Dictionary<int, RankConfig> rankConfigSpawnId = new Dictionary<int, RankConfig>();

        public void OnExecuted(MySqlDataReader reader)
        {
            int rank = 1, spawn_id = 0;
            string name = "all";

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    rank = reader.GetInt32("rank");
                    spawn_id = reader.GetInt32("spawn_id");
                    name = reader.GetString("digimon");

                    // Configuração geral
                    if (spawn_id == 0 && name == "all")
                    {
                        // Se não existe a configuração do rank, criamos
                        if (!rankConfig.ContainsKey(rank))
                        {
                            RankConfig parse = new RankConfig();
                            rankConfig.Add(rank, parse);
                        }
                    }
                    // Configuração por nome do Digimon
                    if(name != "all")
                    {
                        // Se não existe a configuração do rank, criamos
                        if (!rankConfigName.ContainsKey(name))
                        {
                            RankConfig parse = new RankConfig();
                            rankConfigName.Add(name, parse);
                        }
                    }
                    // Configuração por ID de Spawn
                    if (spawn_id != 0)
                    {
                        // Se não existe a configuração do rank, criamos
                        if (!rankConfigSpawnId.ContainsKey(spawn_id))
                        {
                            RankConfig parse = new RankConfig();
                            rankConfigSpawnId.Add(spawn_id, parse);
                        }
                    }

                    RankConfig Config = rankConfig[rank];
                    if (name != "all") Config = rankConfigName[name];
                    if (spawn_id != 0) Config = rankConfigSpawnId[spawn_id];

                    string parametro = reader.GetString("parametro");
                    int valor = reader.GetInt32("valor");

                    // Assimilando os parâmetros a configuração
                    switch (parametro)
                    {
                        case "tpbar":
                            Config.TPBarSec = valor;
                            break;
                        case "qtmax":
                            Config.QuantMax = valor;
                            break;
                        case "xp":
                            Config.XPPerc = valor;
                            break;
                        case "hp":
                            Config.HPPerc = valor;
                            break;
                        case "atk":
                            Config.ATKPerc = valor;
                            break;
                        case "def":
                            Config.DEFPerc = valor;
                            break;
                        case "bl":
                            Config.BLPerc = valor;
                            break;
                        case "bit":
                            Config.BitPerc = valor;
                            break;
                        case "f2":
                            Config.F2Lvl = valor;
                            break;
                        case "f3":
                            Config.F3Lvl = valor;
                            break;
                        case "fatorhp":
                            Config.FatorHP = valor;
                            break;
                    }

                    name = "all";
                    spawn_id = 0;
                }
            }
        }
    }
}
