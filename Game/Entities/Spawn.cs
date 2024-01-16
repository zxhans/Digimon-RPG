using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using Digimon_Project.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace Digimon_Project.Game.Entities
{
    public class Spawn : Entity
    {
        public int Health { get; set; } // Calculate
        public int estage { get; set; }
        public int DigimonId { get; set; }
        public long GUID { get; set; }

        // Controle de tempo de respawn e Boss
        public int Tempo { get; set; } // Tempo que levo para respawnar depois de morrer
        private TimerPlus aTimer; // Temporizador
        public bool Bloqueado = false; // Estou bloquado para batalhar?
        public bool Abatido = false; // Fui abatido?
        public int Fixo { get; set; } // Posso spawnar em pontos aleatórios do mapa?
        public int Lider { get; set; } // ID do meu líder, se tiver
        public List<Spawn> Ajudantes { get; set; } // Lista de ajudantes deste spawn

        public Skill skill1;
        public Skill skill2;

        // Map
        public int type { get; set; }
        public int map_id { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 lastPos { get; set; }
        public int rank { get; set; }
        public int move { get; set; }
        public int Quant { get; set; }
        public int QuantMin { get; set; }
        public string Drop { get; set; }

        // Our calculated points.
        public double Strength { get; set; } // Calculate.
        public double Dexterity { get; set; } // Calculate.
        public double Constitution { get; set; } // Calculate.
        public double Intelligence { get; set; } // Calculate.

        public int Attack { get; set; } // Calculate.
        public int Defence { get; set; } // Calculate.
        public int BattleLevel { get; set; } // Calculate.

        public int[] statsLevel { get; set; }

        public Spawn(int id)
            : base(id)
        {
            // Default Values, Level 1.
            Level = 1;
            Health = 100;
            Random r = new Random(DateTime.Now.Millisecond + (id * 100));
            GUID = r.Next(100);
        }

        private void DefaultStatsForLevel()
        {
            Strength = 10 + (statsLevel[0] * Level);
            Dexterity = 10 + (statsLevel[1] * Level);
            Constitution = 10 + (statsLevel[2] * Level);
            Intelligence = 10 + (statsLevel[3] * Level);
        }

        public void Calculate()
        {
            // Reset for calculation.
            DefaultStatsForLevel();

            // Attributes
            // In Training Base
            int atk_base = 130;
            int hp_base = 75;
            int def_base = 67;
            int bl_base = 195;
            switch (estage)
            {
                // Rookie Base
                case 2:
                    hp_base = 530;
                    atk_base = 345;
                    def_base = 160;
                    bl_base = 410;
                    break;
            }
            Health = hp_base + Convert.ToInt32(Math.Round((Constitution * 3) + (Strength)));
            Attack = atk_base + Convert.ToInt32(Math.Round((Strength) + (Dexterity * .5)));
            Defence = def_base + Convert.ToInt32(Math.Round((Constitution * 1.5) + (Intelligence * .5)));
            BattleLevel = bl_base + Convert.ToInt32(Math.Round((Dexterity * 1.5) + (Intelligence * 5)));

            if (Emulator.Enviroment.RankConfig.ContainsKey(rank))
            {
                RankConfig Config = Emulator.Enviroment.RankConfig[rank];

                Health *= Config.HPPerc / 100;
                Attack *= Config.ATKPerc / 100;
                Defence *= Config.DEFPerc / 100;
                BattleLevel *= Config.BLPerc / 100;
            }
        }

        // Função para coletar os ajudantes deste digimon se tiver
        public void CallAllies()
        {
            SpawnResult spawnResult = Emulator.Enviroment.Database.Select<SpawnResult>("s.id, s.name"
                + ", s.lvl AS level, s.map_id, s.x AS location_x"
                + ", s.y AS location_y, s.rank, s.move, s.quant, s.mquant, s.digimon_id, s.item_drop"
                + ", d.tipo AS type, d.str, d.dex, d.con, d.inte, d.estage, d.skill1, d.skill2, d.model"
                + ", s.tempo, s.fixo, s.lider, s.bloqueado"
                , "spawn_digimons s, digimon d"
                , "WHERE s.digimon_id = d.id AND lider=@id AND dtexclusao IS NULL AND is_deleted = 0"
                , new Database.QueryParameters() { { "id", Id } });

            Ajudantes = spawnResult.spawns;
        }

        // Temporizador que vai respawnar este Digimon quando seu tempo reiniciar
        public void SetRespawnTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new TimerPlus(Tempo * 60000); // 10000 = 10 segundos
                                           // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += Respawnar;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }
        private void Respawnar(Object source, ElapsedEventArgs e)
        {
            Bloqueado = false;
            Abatido = false;
            MapZone zone = Emulator.Enviroment.MapZone[map_id];
            if (this.rank == 6 || this.rank == 7)
            {
                //CASO SEJA LEGENDARY HERO
                Utils.Comandos.SendGM("Yggdrasil", this.Name + " apareceu em algum lugar do DigiMundo!");
            }
            if (zone != null)
                zone.sendSpawn(this);
        }
    }
}
