using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Managers;
using Digimon_Project.Network;
using Digimon_Project.Network.Listeners;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace Digimon_Project
{
    public class Enviroment
    {
        public bool IsRunning { get; private set; }
        public bool Teste = true;
        public LoginListener LoginListener { get; private set; }
        public MapListener MapListener { get; private set; }
        public DbConnection Database { get; private set; }
        public MapConnections MapConnections { get; private set; }
        public Dictionary<String, ItemCodex> Codex { get; private set; }
        public Dictionary<int, RankConfig> RankConfig { get; set; }
        public Dictionary<string, RankConfig> RankConfigName { get; set; }
        public Dictionary<int, RankConfig> RankConfigSpawnId { get; set; }
        public Dictionary<int, string> CrestUpgrade { get; set; }
        public Dictionary<int, string> CrestType { get; set; }
        public MapZone[] MapZone { get; private set; }
        public double ExpFator = 1;
        public int TimeRunning = 0;
        public string OriginalTitle = null;
        // Flag que define o modo de manutenção
        public bool Manutencao = false;
        public string GMNick = "Homeostasis";
        private System.Timers.Timer aTimer;
        public List<string> IP_Block = new List<string>();

        // Configurações
        public Config Config;

        public Enviroment()
        {
            // Initializeing Console.
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            OriginalTitle = "DbrasilRPG Server Console | v2022-01-09";
            Console.Title = "[Starting server] " + OriginalTitle;

            //Console.WriteLine(new string('_', Console.WindowWidth - 1));
            // Initializeing Classes.
            // Criando os Listeners, responsáveis por receber conexões
            LoginListener = new LoginListener();
            MapListener = new MapListener();
            MapConnections = new MapConnections();
        }

        public bool Start()
        {
            IsRunning = true;

            //Utils.Email.SendMail("boina2018@hotmail.com", "Teste", "Link: <a href='pokemonsekai.com.br'>Link</a>");

            // Carregando tabelas internas
            Console.WriteLine(PacketTable.Instance.ToString());
            Console.WriteLine(NPCATable.Instance.ToString());

            // Configuração do Banco
            /**/ // Base de Produção

            Database = new DbConnection("localhost", 3306, "root", "", "drpg");
            Teste = false;


            //Database = new DbConnection("localhost", 3306, "drpg_user", "PCLxM3LbX0b3o17P", "drpg");


            // Database = new DbConnection("localhost", 3306, "root", "", "drpg");


            /** / // Base de Testes
            Database = new DbConnection("", 3306, "root", "", "tmo");
            //Teste = false;
            /**/

            if (Manutencao)
                Console.WriteLine("ATENCAO: Modo de manutencao ATIVADO!");

            // Carregando tabelas do Banco
            if (Database.Check())
            {
                ItemCodexResult codex = Database.Select<ItemCodexResult>(
                        "c.id AS item_id, c.item_idx AS ItemId"
                        + ", c.item_tag AS ItemTag, c.item_type1 AS ItemType, c.item_use_on AS ItemUseOn"
                        + ", c.default_max_quantity AS ItemQuantMax, c.required_level AS ItemtamerLvl"
                        + ", c.effect_type_1 AS ItemEffect1, c.effect_value_1 AS ItemEffect1Value"
                        + ", c.effect_type_2 AS ItemEffect2, c.effect_value_2 AS ItemEffect2Value"
                        + ", c.effect_type_3 AS ItemEffect3, c.effect_value_3 AS ItemEffect3Value"
                        + ", c.effect_type_4 AS ItemEffect4, c.effect_value_4 AS ItemEffect4Value"
                        + ", c.name AS ItemName, c.item_tab, c.custo"
                        , "item_codex AS c");

                Codex = codex.codex;
                Console.WriteLine("Items Table Loaded. {0} Items Loaded.", Codex.Count);

                // Iniciando MapZones
                Console.WriteLine("Loading Maps...");
                MapZone = new MapZone[256];
                for (int i = 1; i <= 255; i++)
                {
                    MapZone[i] = new MapZone(i);
                    if (!Teste)
                    {
                        MapZone[i].Spawn();
                        MapZone[i].CarregarTeleports();
                    }
                }
                Console.WriteLine("Maps Loaded!");

                // Carregando configurações de Rank
                CarregarRankConfig();
                Console.WriteLine("Rank Settings Loaded!");

                // Configurações
                Config = new Config();

                Manutencao = Config.Manutencao;

                ExpFator = Config.ExpFator;

                //Upgrade de Crests
                CarregarCrests();
                Console.WriteLine("Crests Settings Loaded!");

                // Iniciando temporizador
                SetTimer();

                // Carregando lista de IPs bloqueados
                string textFile = Directory.GetCurrentDirectory() + "/ip_block.txt";
                string[] lines = File.ReadAllLines(textFile);
                foreach (string line in lines)
                {
                    IP_Block.Add(line);
                }

                Console.Title = "[Server started] " + OriginalTitle;
            }

            // Se temos conexão com o banco e os Listeners estão prontos, podemos iniciar o Servidor
            return Database.Check()
                && LoginListener.BindAndListen()
                && MapListener.BindAndListen()
                ;
        }

        public void Run()
        {
            Thread.Sleep(5000);
        }

        public void Stop()
        {
            Console.ReadKey();
        }

        // Carregando configurações de Ranks
        public void CarregarRankConfig()
        {
            RankConfigResult result = Database.Select<RankConfigResult>("*", "rank_config");
            RankConfig = result.rankConfig;
            RankConfigName = result.rankConfigName;
            RankConfigSpawnId = result.rankConfigSpawnId;
        }

        public void CarregarCrests()
        {
            CrestUpgrade = new Dictionary<int, string>();
            CrestType = new Dictionary<int, string>();

            //KNOWLEDGE CREST
            CrestUpgrade.Add(4, "KnowledgeCrest+2");
            CrestUpgrade.Add(140, "KnowledgeCrest+3");
            CrestUpgrade.Add(142, "KnowledgeCrest+4");
            CrestType.Add(4, "simples");
            CrestType.Add(140, "simples");
            CrestType.Add(142, "simples");

            CrestUpgrade.Add(144, "KnowledgeCrest+5");
            CrestUpgrade.Add(146, "KnowledgeCrest+6");
            CrestUpgrade.Add(148, "KnowledgeCrest+7");
            CrestType.Add(144, "booster");
            CrestType.Add(146, "booster");
            CrestType.Add(148, "booster");

            CrestUpgrade.Add(150, "KnowledgeCrest+8");
            CrestUpgrade.Add(152, "KnowledgeCrest+9");
            CrestUpgrade.Add(154, "KnowledgeCrest+10");
            CrestType.Add(150, "blessing");
            CrestType.Add(152, "blessing");
            CrestType.Add(154, "blessing");

            //COURAGE CREST
            CrestUpgrade.Add(1, "CourageCrest+2");
            CrestUpgrade.Add(96, "CourageCrest+3");
            CrestUpgrade.Add(99, "CourageCrest+4");
            CrestType.Add(1, "simples");
            CrestType.Add(96, "simples");
            CrestType.Add(99, "simples");

            CrestUpgrade.Add(102, "CourageCrest+5");
            CrestUpgrade.Add(105, "CourageCrest+6");
            CrestUpgrade.Add(108, "CourageCrest+7");
            CrestType.Add(102, "booster");
            CrestType.Add(105, "booster");
            CrestType.Add(108, "booster");

            CrestUpgrade.Add(111, "CourageCrest+8");
            CrestUpgrade.Add(114, "CourageCrest+9");
            CrestUpgrade.Add(117, "CourageCrest+10");
            CrestType.Add(111, "blessing");
            CrestType.Add(114, "blessing");
            CrestType.Add(117, "blessing");

            //HONEST CREST
            CrestUpgrade.Add(5, "HonestCrest+2");
            CrestUpgrade.Add(158, "HonestCrest+3");
            CrestUpgrade.Add(160, "HonestCrest+4");
            CrestType.Add(5, "simples");
            CrestType.Add(158, "simples");
            CrestType.Add(160, "simples");

            CrestUpgrade.Add(162, "HonestCrest+5");
            CrestUpgrade.Add(164, "HonestCrest+6");
            CrestUpgrade.Add(166, "HonestCrest+7");
            CrestType.Add(162, "booster");
            CrestType.Add(164, "booster");
            CrestType.Add(166, "booster");

            CrestUpgrade.Add(168, "HonestCrest+8");
            CrestUpgrade.Add(170, "HonestCrest+9");
            CrestUpgrade.Add(172, "HonestCrest+10");
            CrestType.Add(168, "blessing");
            CrestType.Add(170, "blessing");
            CrestType.Add(172, "blessing");
        }

        // Temporizador que executa eventos a cada hora
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            //ESTA RODANDO A CADA 10 MINUTOS
            aTimer = new System.Timers.Timer(10000); //  1000 = 1 segundo
                                                     //360000 = uma hora
                                                     //600000 = 10 MINUTOS

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += Hora;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void Hora(Object source, ElapsedEventArgs e)
        {
            // Removendo 1 hora dos banidos --> Uma hora nao corresponde mais ao real, ja que o timer roda a cada 10 minutos
            // CASO USAR: Transformar o tempo de ban em minutos multiplos de 10 e ir tirando 10 min
            //Database.Query("UPDATE users SET ban = ban - 1 WHERE ban > 0", new QueryParameters());

            //up
            Database.Query("UPDATE tamer_inventory SET quantity = quantity - 1 WHERE quantity <= 10 and quantity > 1 and max_quantity >= 1000 and warehouse != 3", new QueryParameters());
            Database.Query("UPDATE tamer_inventory SET quantity = quantity - 10 WHERE quantity > 10 and max_quantity >= 1000 and warehouse != 3", new QueryParameters());
            Console.WriteLine("[CONSOLE_TIMER] [10SEC] [UPDATE] TEMPORARY CLOTHES");
            TimeRunning += 10;
            TimeSpan time = TimeSpan.FromSeconds(TimeRunning);
            Console.Title = "[Up for " + time.ToString(@"d\d\,\ hh\:mm\:ss") + "] " + OriginalTitle;
        }


    }
}
