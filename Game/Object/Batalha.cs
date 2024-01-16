using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Digimon_Project.Game
{
    // Classe que controla as batalhas em andamento
    public class Batalha
    {
        public readonly int Id;
        private Timer aTimer, pTimer;
        public List<Client> Clients = new List<Client>();
        public Digimon[] EquipeA = new Digimon[5];
        public Digimon[] EquipeB = new Digimon[5];
        private int EquipeALevel = 0;
        private int EquipeBLevel = 0;
        private int count = 1;
        private int spawnTp = 0;
        public Queue<Digimon> ListAction = new Queue<Digimon>();
        public bool inAction = false;
        private Spawn Spawn = null;
        public bool Capturado = false, Paused = false;
        public int slash = 0;
        private bool IsPvP = false;
        public int CheckCount = 0, QuantMax = 5, XPPerc = 100, HPPerc = 100, ATKPerc = 100, DEFPerc = 100
            , BLPerc = 100, BitPerc = 100, F2Lvl = 0, F3Lvl = 0;

        // Construtor para batalha de um Tamer contra Spawn
        public Batalha(Client sender, Spawn s)
        {
            // Adicionando o client à lista
            Clients.Add(sender);
            Spawn = s;
            // Assimilando a Batalha ao Client
            sender.Batalha = this;

            // Adicionando o Digimon principal do Tamer na lista
            EquipeA[0] = sender.Tamer.Digimon[0];

            // Assimilando a Batalha ao Digimon
            sender.Tamer.Digimon[0].batalha = this;

            // Se estou em Party, devo incluir os outros membros na batalha
            if (sender.Tamer.Party != null && sender.Tamer.Party.Lider == sender.Tamer)
            {
                int i = 1;
                foreach (Tamer t in sender.Tamer.Party.Tamers)
                    if (t != null && i < 5)
                    {
                        // Adicionando o client à lista
                        Clients.Add(t.Client);
                        t.Client.Team = 2;

                        // Assimilando a Batalha ao Client
                        t.Client.Batalha = this;

                        // Adicionando o Digimon principal do Tamer na lista
                        EquipeA[i] = t.Digimon[0];

                        // Assimilando a Batalha ao Digimon
                        t.Digimon[0].batalha = this;
                        i++;
                    }
            }

            spawnTp = 50;

            ConfigurarRank(s.rank, s.Name, s.Id);

            // Adicionando Digimons do Spawn na lista
            Random r = new Random();
            count = r.Next(s.QuantMin, (s.Quant + 1));
            if (count < s.QuantMin)
            {
                count = s.QuantMin;
            }
            if (count < 1)
            {
                count = 1;
            }
            if (count > QuantMax)
            {
                count = QuantMax;
            }
            // Teste
            //count = 5;  
            for (int i = 0; i < count; i++)
            {
                Digimon spawn = new Digimon(0, s.Id)
                {
                    Model = s.Model,
                    DigimonId = s.DigimonId,
                    Name = s.Name,
                    Level = s.Level,
                    Strength = s.Strength,
                    Dexterity = s.Dexterity,
                    Constitution = s.Constitution,
                    Intelligence = s.Intelligence,
                    type = s.type,
                    Health = s.Health,
                    Attack = s.Attack,
                    Defence = s.Defence,
                    BattleLevel = s.BattleLevel,
                    MaxHealth = s.Health,
                    skill1 = s.skill1 != null ? s.skill1 : null,
                    skill2 = s.skill2 != null ? s.skill2 : null,
                    BattleId = r.Next(),
                    BattleSufix = s.Name.Substring(0, s.Name.Length <= 8 ? s.Name.Length : 8),
                    batalha = this,
                    rank = (byte)s.rank,
                    iSpawn = true,
                    skill1lvl = 1 + (s.skill1 != null ? (s.Level - s.skill1.Lvl < 10 ? (s.Level - s.skill1.Lvl) : 9) : 0),
                    skill2lvl = 1 + (s.skill2 != null ? (s.Level - s.skill2.Lvl < 10 ? (s.Level - s.skill2.Lvl) : 9) : 0),
                };
                spawn.Carregar(s.DigimonId);

                if (F2Lvl > 0)
                    spawn.skill1lvl = F2Lvl;
                if (F3Lvl > 0)
                    spawn.skill2lvl = F3Lvl;

                EquipeB[i] = spawn;
            }

            // Ajudantes, caso tenha
            if (s.Ajudantes != null && s.Ajudantes.Count > 0 && count < 5)
            {
                int next = 0;
                for (int i = count; i < 5; i++)
                {
                    if (s.Ajudantes.Count >= next + 1 && EquipeB[i] == null)
                    {
                        Spawn a = s.Ajudantes[next];
                        next++;
                        Digimon spawn = new Digimon(0, a.Id)
                        {
                            Model = a.Model,
                            DigimonId = a.DigimonId,
                            Name = a.Name,
                            Level = a.Level,
                            Strength = a.Strength,
                            Dexterity = a.Dexterity,
                            Constitution = a.Constitution,
                            Intelligence = a.Intelligence,
                            type = a.type,
                            Health = a.Health,
                            Attack = a.Attack,
                            Defence = a.Defence,
                            BattleLevel = a.BattleLevel,
                            MaxHealth = a.Health,
                            skill1 = a.skill1 != null ? a.skill1 : null,
                            skill2 = a.skill2 != null ? a.skill2 : null,
                            BattleId = r.Next(),
                            BattleSufix = a.Name.Substring(0, a.Name.Length <= 8 ? a.Name.Length : 8),
                            batalha = this,
                            rank = (byte)a.rank,
                            iSpawn = true,
                            skill1lvl = 1 + (a.skill1 != null ? (a.Level - a.skill1.Lvl < 10 ? (a.Level - a.skill1.Lvl) : 9) : 0),
                            skill2lvl = 1 + (a.skill2 != null ? (a.Level - a.skill2.Lvl < 10 ? (a.Level - a.skill2.Lvl) : 9) : 0),
                        };
                        spawn.Carregar(a.DigimonId);

                        if (F2Lvl > 0)
                            spawn.skill1lvl = F2Lvl;
                        if (F3Lvl > 0)
                            spawn.skill2lvl = F3Lvl;

                        EquipeB[i] = spawn;
                    }
                }
            }

            Network.OutPacket res = new Network.Packets.PACKET_BATTLE_CENARY(EquipeB, EquipeA, Clients);
            sender.Connection.Send(res);
            // Se estou em Party, devo enviar o pacote para os outros membros
            if (sender.Tamer.Party != null && sender.Tamer.Party.Lider == sender.Tamer)
            {
                sender.Tamer.Party.SendPacket(res);
            }
            SetTimer();

            // Exibindo configuração para GM
            if (sender.User.Autoridade >= 100)
            {
                Utils.Comandos.Send(sender, string.Format("Info {0} Level {1} / Model {2} / DigimonID {3}", EquipeB[0].Name, EquipeB[0].Level, EquipeB[0].Model, EquipeB[0].DigimonId));
                Utils.Comandos.Send(sender, string.Format("HP {0}: {1}", EquipeB[0].Name, EquipeB[0].MaxHealth));
                Utils.Comandos.Send(sender, string.Format("Attack {0}: {1}", EquipeB[0].Name, EquipeB[0].Attack));
                Utils.Comandos.Send(sender, string.Format("Defence {0}: {1}", EquipeB[0].Name, EquipeB[0].Defence));
                Utils.Comandos.Send(sender, string.Format("BattleLevel {0}: {1}", EquipeB[0].Name, EquipeB[0].BattleLevel));
                Utils.Comandos.Send(sender, string.Format("Total de Inimigos {0}", count));
                Utils.Comandos.Send(sender, string.Format("Info {0} Level {1} / Model {2} / DigimonID {3} / ATK {4} / DEF {5}", EquipeA[0].Name, EquipeA[0].Level,
                    EquipeA[0].Model, EquipeA[0].DigimonId, EquipeA[0].Attack, EquipeA[0].Defence));
            }
        }
        // Construtor de Batalha PvP
        public Batalha(Client sender, Client c)
        {
            IsPvP = true;
            // Adicionando o client à lista
            Clients.Add(sender);
            Clients.Add(c);
            // Assimilando a Batalha ao Client
            sender.Batalha = this;
            c.Batalha = this;

            // Adicionando o Digimon principal do Tamer na lista
            EquipeA[0] = sender.Tamer.Digimon[0];
            sender.Tamer.Digimon[0].batalha = this;
            sender.Team = 2; // Time A
            EquipeALevel += sender.Tamer.Level;

            EquipeB[0] = c.Tamer.Digimon[0];
            c.Tamer.Digimon[0].batalha = this;
            c.Team = 1; // Time B
            EquipeBLevel += c.Tamer.Level;
        }

        // Client pronto para começar a batalha (PvP)
        public void BattleReady(Client c)
        {
            if (c.Batalha == this)
            {
                // Client pronto
                c.BattleReady = true;

                // Se todos estiverem prontos, podemos começar
                bool ok = true;
                foreach (Client client in Clients)
                    if (client != null && !client.BattleReady)
                        ok = false;

                if (ok)
                    foreach (Client client in Clients)
                        if (client != null)
                        {
                            // Toda a batalha é vista pelo mesmo ângulo (baixo pra cima) independente do desafiante e
                            // desafiado. Por isso, devemos inverter a ordem do cenário caso o client esteja na equipe B
                            if (client.Team == 2)
                                client.Connection.Send(new Network.Packets.PACKET_BATTLE_CENARY(EquipeB, EquipeA, Clients));
                            else
                                client.Connection.Send(new Network.Packets.PACKET_BATTLE_CENARY(EquipeA, EquipeB, Clients));
                        }
            }
        }

        // Preparando Ação em batalha
        public void Action(byte act, long id, string sufix, long alvo, string alvo_sufix, Client c)
        {
            // Procurando Digimons solicitante e alvo
            Digimon sol = null;
            Digimon alv = null;
            foreach (Digimon d in EquipeA)
            {
                if (d != null)
                {
                    if (d.BattleId == id) sol = d;
                    if (d.BattleId == alvo) alv = d;
                }
            }
            foreach (Digimon d in EquipeB)
            {
                if (d != null)
                {
                    if (d.BattleId == id) sol = d;
                    if (d.BattleId == alvo) alv = d;
                }
            }

            if (sol != null)
                // Se ambos os Digimons foram encontrados, podemos processar a ação
                switch (act)
                {
                    // Ataque F1, F2, F3
                    case 0x0A:
                    case 0x0B:
                    case 0x0C:
                        if (alv != null)
                        {
                            c.Connection.Send
                                (new Network.Packets.PACKET_BATTLE_ACTION(id, sufix, alvo, alvo_sufix, 1, act));
                            sol.alvo = alv;
                            sol.nextAction = act;
                            sol.execute = true;

                            sol.Executar();
                        }
                        break;
                    // Digievolução
                    case 0x14:
                    case 0x15:
                    case 0x16:
                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(id, sufix, 0, "", 1, act));
                        sol.nextAction = act;
                        sol.execute = true;
                        sol.Executar();
                        break;
                    // Chamando Digimons
                    case 0x02:
                        foreach (Digimon d in c.Tamer.Digimon)
                        {
                            if (d != null && d.BattleId == alvo)
                            {
                                c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(id, sufix, 0, "", 1, act));
                                sol.alvo = d;
                                sol.nextAction = act;
                                sol.execute = true;
                                sol.Executar();
                                return;
                            }
                        }
                        break;
                    // Fuga
                    case 0x04:
                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(id, sufix, 0, "", 1, act));
                        sol.nextAction = act;
                        sol.execute = true;

                        // Se foi o líder quem fugiu, então os partner devem fugir também
                        if (sol == c.Tamer.Digimon[0])
                            foreach (Digimon d in c.Tamer.Digimon)
                                if (d != null)
                                {
                                    d.alvo = alv;
                                    d.nextAction = act;
                                    d.execute = true;
                                    d.Executar();
                                }

                        sol.Executar();
                        break;
                    // Captura
                    case 0x01:
                        //c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(id, sufix, 0, "", 1, act));
                        sol.alvo = alv;
                        sol.nextAction = act;
                        sol.execute = true;
                        sol.Executar();
                        break;
                }
        }

        // Executando a ação em Batalha
        public void ExecuteAction(Digimon digimon)
        {
            Pause(digimon);
            // Procurando Digimons solicitante e alvo
            Digimon sol = null;
            Digimon alv = null;
            Digimon[] ataque = new Digimon[5]; // Equipe do Digimon Atacante
            Digimon[] defesa = new Digimon[5]; // Equipe do Digimon Defensor
            int Avivos = 0; // Quantidade de Digimons vivos em cada equipe
            int Bvivos = 0;
            int alvo_index = 0; // Index na Lista do alvo
            for (int i = 0; i < 5; i++)
            {
                if (EquipeA[i] != null)
                {
                    if (EquipeA[i] == digimon)
                    {
                        sol = EquipeA[i];
                        ataque = EquipeA;
                    }
                    if (EquipeA[i] == digimon.alvo)
                    {
                        alv = EquipeA[i];
                        defesa = EquipeA;
                        alvo_index = i;
                    }
                    if (EquipeA[i].Health > 0)
                        Avivos++;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (EquipeB[i] != null)
                {
                    if (EquipeB[i] == digimon)
                    {
                        sol = EquipeB[i];
                        ataque = EquipeB;
                    }
                    if (EquipeB[i] == digimon.alvo)
                    {
                        alv = EquipeB[i];
                        defesa = EquipeB;
                        alvo_index = i;
                    }
                    if (EquipeB[i].Health > 0)
                        Bvivos++;
                }
            }

            if (sol != null)
                // Se ambos os Digimons foram encontrados, podemos processar a ação
                switch (digimon.nextAction)
                {
                    // Ataque F1, F2, F3
                    case 0x0A:
                    case 0x0B:
                    case 0x0C:
                        if (digimon.alvo != null && digimon.alvo.Health > 0 && alv != null)
                        {
                            Config conf = Emulator.Enviroment.Config;
                            // EVP
                            if (!sol.iSpawn && sol.estage > 2)
                                sol.AddEVP(-(int)(sol.estage * conf.EVPGastoTx));

                            int unit = 1; // Unidades atingidas
                            int skillunit = 1;
                            int skillId = 0;
                            double extra = 1;
                            switch (digimon.nextAction)
                            {
                                case 0x0B:
                                    if (sol.Level >= sol.skill1.Level && sol.VP >= sol.skill1.VP)
                                    {
                                        skillId = sol.skill1.Id;
                                        unit = sol.skill1.Units;
                                        //extra = (103 * sol.skill1lvl) / 100;
                                        extra += conf.F2Tx * sol.skill1lvl;
                                        sol.AddVP(-sol.skill1.VP);
                                    }
                                    break;
                                case 0x0C:
                                    if (sol.Level >= sol.skill2.Level && sol.VP >= sol.skill2.VP)
                                    {
                                        skillId = sol.skill2.Id;
                                        unit = sol.skill2.Units;
                                        //extra = (105 * sol.skill2lvl)/100;
                                        extra += conf.F3Tx * sol.skill2lvl;
                                        sol.AddVP(-sol.skill2.VP);
                                    }
                                    break;
                            }
                            skillunit = unit;
                            // Calculando dano e unidades atingidas
                            // Card Slash do Solicitante
                            if (!sol.iSpawn && sol.Tamer != null)
                            {
                                int slash1atk = 0;
                                int slash2atk = 0;
                                int slash3atk = 0;

                                if (sol.Tamer.Slash1 != 0)
                                {
                                    Item card = sol.Tamer.Cards[sol.Tamer.Slash1];
                                    if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 1)
                                        slash1atk = card.ExecuteCard(sol, ataque, 1);
                                    //Utils.Comandos.Send(sol.Tamer.Client, "Usou CS1");
                                }
                                if (sol.Tamer.Slash2 != 0)
                                {
                                    Item card = sol.Tamer.Cards[sol.Tamer.Slash2];
                                    if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 1)
                                        slash2atk = card.ExecuteCard(sol, ataque, 2);
                                    //Utils.Comandos.Send(sol.Tamer.Client, "Usou CS2");
                                }
                                if (sol.Tamer.Slash3 != 0)
                                {
                                    Item card = sol.Tamer.Cards[sol.Tamer.Slash3];
                                    if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 1)
                                        slash3atk = card.ExecuteCard(sol, ataque, 3);
                                    //Utils.Comandos.Send(sol.Tamer.Client, "Usou CS3");
                                }
                                slash = Math.Max(slash1atk, Math.Max(slash2atk, slash3atk));
                                //sol.SaveFloppy();
                            }
                            int vivos = 0;
                            if (defesa == EquipeA) vivos = Avivos;
                            if (defesa == EquipeB) vivos = Bvivos;
                            double dano = (sol.Attack);
                            if (sol.ExtraAttack != 0) dano += (dano * sol.ExtraAttack) / 100;
                            if (!sol.iSpawn && sol.Tamer != null) dano += (sol.Attack * sol.Tamer.AttackBonus) / 100;
                            for (int i = 0; i < 5; i++)
                            {
                                if (unit > 0)
                                {
                                    if (defesa[alvo_index] != null && defesa[alvo_index].Health > 0)
                                    {
                                        // Card Slash do alvo
                                        if (!defesa[alvo_index].iSpawn && defesa[alvo_index].Tamer != null)
                                        {
                                            int slash1def = 0;
                                            int slash2def = 0;
                                            int slash3def = 0;

                                            if (defesa[alvo_index].Tamer.Slash1 != 0)
                                            {
                                                Item card = defesa[alvo_index].Tamer
                                                    .Cards[defesa[alvo_index].Tamer.Slash1];
                                                if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 2)
                                                    slash1def = card.ExecuteCard(defesa[alvo_index], defesa, 1);
                                            }
                                            if (defesa[alvo_index].Tamer.Slash2 != 0)
                                            {
                                                Item card = defesa[alvo_index].Tamer
                                                    .Cards[defesa[alvo_index].Tamer.Slash2];
                                                if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 2)
                                                    slash2def = card.ExecuteCard(defesa[alvo_index], defesa, 2);
                                            }
                                            if (defesa[alvo_index].Tamer.Slash3 != 0)
                                            {
                                                Item card = defesa[alvo_index].Tamer
                                                    .Cards[defesa[alvo_index].Tamer.Slash3];
                                                if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 2)
                                                    slash3def = card.ExecuteCard(defesa[alvo_index], defesa, 3);
                                            }
                                            slash = Math.Max(slash1def, Math.Max(slash2def, slash3def));

                                        }
                                        defesa[alvo_index].atacado = 1;
                                        // Cálculo de dano
                                        // Considerando que o poder de fogo é 100% do dano que o alvo vai receber, vamos calcular
                                        // o dano considerando a % de dano a ser reduzido baseado na defesa do alvo
                                        //double fator_defesa = 1.5;
                                        int def_limit = 95; // Limite de % a ser reduzida no dano

                                        // Calculando a defesa total
                                        double DEF = (defesa[alvo_index].Defence);
                                        if (defesa[alvo_index].ExtraDefense != 0)
                                            DEF += (defesa[alvo_index].Defence * defesa[alvo_index].ExtraDefense) / 100;
                                        if (!defesa[alvo_index].iSpawn && defesa[alvo_index].Tamer != null)
                                            DEF += (defesa[alvo_index].Defence * defesa[alvo_index].Tamer.DefenseBonus) / 100;
                                        //if (DEF > def_limit) fator_defesa += (DEF - def_limit) / 200;

                                        // Obtendo a redução de %
                                        int redPerc = Convert.ToInt32(DEF * 100 / dano); // Vamos usar apenas metade da redução
                                        // Inserindo a limitação
                                        if (redPerc > def_limit) redPerc = def_limit;

                                        /**/ // Resultante que será subtraída do HP
                                        double danoTx = conf.DanoTx;
                                        double defTx = conf.DefTx;
                                        double damageTx = conf.DamageTx;
                                        //int damage = Convert.ToInt32(Math.Round(((dano * danoTx) - (DEF * defTx)) * damageTx * extra));
                                        int damage = Convert.ToInt32(
                                            Math.Round(((dano * danoTx) * (100 / (100 + (DEF * defTx)))) * damageTx * extra));
                                        /**/
                                        // Fraquezas de tipo (Vacine, Data, Virus)
                                        damage = Convert.ToInt32(Math.Round(damage * GetFator(sol.type
                                            , defesa[alvo_index].type)));

                                        if (sol.ExtraDamage > 0)
                                            damage += (damage * sol.ExtraDamage) / 100;

                                        // G Drill
                                        if (sol.drill && !IsPvP)
                                        {
                                            skillunit = 5;
                                            unit = 5;
                                        }
                                        // Dãno em área
                                        else if (skillunit > 1)
                                        {
                                            if (skillunit > vivos) skillunit = vivos;
                                            if (skillunit > 4)
                                                damage = (int)(damage * conf.DanoArea5);
                                            else if (skillunit > 3)
                                                damage = (int)(damage * conf.DanoArea4);
                                            else if (skillunit > 2)
                                                damage = (int)(damage * conf.DanoArea3);
                                            else if (skillunit > 1)
                                                damage = (int)(damage * conf.DanoArea2);
                                        }
                                        //if (sol.drill && !IsPvP) damage *= skillunit;

                                        // Dano mínimo
                                        if (damage < 1) damage = 1;

                                        // Chance de Miss
                                        int ATKBL = sol.BattleLevel;
                                        if (sol.ExtraBLevel != 0) ATKBL += (ATKBL * sol.ExtraBLevel) / 100;
                                        if (!sol.iSpawn && sol.Tamer != null) ATKBL += (sol.BattleLevel * sol.Tamer.BLevelBonus) / 100;

                                        int DEFBL = defesa[alvo_index].BattleLevel;
                                        if (defesa[alvo_index].ExtraBLevel != 0)
                                            DEFBL += (DEFBL * defesa[alvo_index].ExtraBLevel) / 100;
                                        if (!defesa[alvo_index].iSpawn && defesa[alvo_index].Tamer != null)
                                            DEFBL += (defesa[alvo_index].BattleLevel * defesa[alvo_index].Tamer.BLevelBonus) / 100;

                                        if (DEFBL > ATKBL && (!sol.drill || IsPvP))
                                        {
                                            int chance = (DEFBL - ATKBL) / 50;
                                            // % de chance de Miss máxima
                                            if (chance > 60) chance = 60;
                                            Random r = new Random();
                                            if (r.Next(100) <= chance)
                                            {
                                                damage = 0;
                                            }
                                        }

                                        if (defesa[alvo_index].whitewing)
                                        {
                                            damage = 0;
                                        }

                                        // Efeito de Card que limita o HP do alvo (HP Card, become 1 HP)
                                        if (sol.HPAlvoLimite > 0)
                                        {
                                            if (defesa[alvo_index].Health - damage < sol.HPAlvoLimite)
                                                damage = defesa[alvo_index].Health - 1;
                                        }

                                        //EFEITO HK
                                        if (sol.onehitko == true)
                                        {
                                            damage += defesa[alvo_index].MaxHealth;
                                        }

                                        // Aplicando dano ao HP
                                        defesa[alvo_index].AddHP(-damage);
                                        if (defesa[alvo_index].Health <= 0)
                                        {
                                            defesa[alvo_index].Health = 0;
                                            // Se o alvo morreu, e era um Spawn, então o outro time ganha XP
                                            if (defesa[alvo_index].iSpawn)
                                            {
                                                Config c = Emulator.Enviroment.Config;
                                                long xp = Utils.XP.GainForDigimonLevel(defesa[alvo_index].Level, defesa[alvo_index].rank);
                                                if (Avivos == 2)
                                                    xp = (int)(xp - (xp * c.GainXPBreak2Digimon));
                                                if (Avivos == 3)
                                                    xp = (int)(xp - (xp * c.GainXPBreak3Digimon));
                                                if (Avivos == 4)
                                                    xp = (int)(xp - (xp * c.GainXPBreak4Digimon));
                                                if (Avivos == 5)
                                                    xp = (int)(xp - (xp * c.GainXPBreak5Digimon));

                                                if (xp < 1) xp = 1;
                                                xp *= XPPerc / 100;
                                                //double bit = Utils.XP.BitForDigimonLevel(defesa[alvo_index].Level) / Avivos;
                                                //bit *= BitPerc / 100;

                                                //CALCULO DO BIT BASE
                                                Random r1 = new Random();
                                                double bit = Utils.XP.BitForDigimonLevel(defesa[alvo_index].Level) / Avivos;
                                                //ADICIONA A POSSIBILIDADE DE VARIACAO NO VALOR DO BIT
                                                bit = Math.Ceiling(bit * BitPerc * r1.Next(50, 100) / 10000);
                                                //

                                                foreach (Digimon d in EquipeA)
                                                {
                                                    if (d != null && d.Health > 0)
                                                    {
                                                        long recXp = xp;
                                                        double recBit = bit;

                                                        // Redução por diferença de level (quanto maior o level do atacante, menor EXP/BIT ele receberá. 
                                                        //PARA FINS DE TESTE ESTÁ DESATIVADO

                                                        if (d.Level - defesa[alvo_index].Level > 9)
                                                        {
                                                            recXp = (int)(recXp * c.DigimonXPRed9Level);        //0%
                                                            recBit = (int)(recBit * c.DigimonBitRed9Level);     //25%
                                                        }
                                                        else if (d.Level - defesa[alvo_index].Level > 7)
                                                        {
                                                            recXp = (int)(recXp * c.DigimonXPRed7Level);        //20%
                                                            recBit = (int)(recBit * c.DigimonBitRed7Level);     //35%
                                                        }
                                                        else if (d.Level - defesa[alvo_index].Level > 4)
                                                        {
                                                            recXp = (int)(recXp * c.DigimonXPRed4Level);        //50%
                                                            recBit = (int)(recBit * c.DigimonBitRed4Level);     //65%
                                                        }

                                                        if (d.Tamer.Client.User.Autoridade >= 500)
                                                        {
                                                            //recXp = xp * 5;
                                                            //recBit = bit * 5;
                                                        }
                                                        // Pets
                                                        // Com exceção do Pichimon, todos os Pets aumentam o Bit e XP
                                                        if (d.Tamer.Pet > 0 && d.Tamer.Pet != 5
                                                            && d.Tamer.PetHP > 0)
                                                        {
                                                            recXp = recXp * 2;
                                                            recBit += (recBit * .5);
                                                        }

                                                        //REDUZ GANHO A PARTIR DE 71
                                                        if (d.Level >= 71)
                                                        {
                                                            recXp = recXp / 2;
                                                        }

                                                        //EQUILIBRA GANHO DE EXP A PARTIR DE 81
                                                        if (d.Level >= 81)
                                                        {
                                                            //NERFA RANK B PRA BAIXO
                                                            if (defesa[alvo_index].rank <= 2)
                                                            {
                                                                recXp = recXp / 30;
                                                            }

                                                            //NERFA RANK V PRA BAIXO
                                                            if (defesa[alvo_index].rank <= 3)
                                                            {
                                                                recXp = recXp / 40;
                                                            }
                                                            else
                                                            {
                                                                recXp = recXp / 20;
                                                            }

                                                            //A PARTIR DO 101
                                                            if (d.Level >= 101)
                                                            {
                                                                recXp = recXp / (5 + (d.Level - 101));
                                                            }
                                                        }

                                                        //NERFA AS LILAMON DE LEVEL MT ALTO RANK D/B
                                                        if (defesa[alvo_index].rank <= 2)
                                                        {
                                                            if (recXp >= 40999990)
                                                            {
                                                                recXp = 40999990;
                                                            }
                                                        }

                                                        //PREVINE BUG DE EXP
                                                        if (recXp >= 2000000000)
                                                        {
                                                            recXp = 1999999999;
                                                        }
                                                        else if (recXp < 0)
                                                        {
                                                            //FIX DE XP NEGATIVA
                                                            recXp = 0;
                                                        }

                                                        d.GainExp(recXp, recBit);
                                                        foreach (Client cc in Clients)
                                                        {
                                                            Utils.Comandos.Send(cc, "[DEBUG] XP/BIT Line. EXP " + recXp + " | BIT " + recBit);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        defesa[alvo_index].atacado = 1;
                                        unit--;
                                        defesa[alvo_index].SaveFloppy();
                                    }
                                    alvo_index++;
                                    if (alvo_index > 4) alvo_index = 0;
                                }
                            }
                            sol.drill = false;
                            //RESETA O CARD RK E WHITE WING
                            sol.cardrk = false;
                            sol.whitewing = false;

                            sol.atacando = 1;
                            sol.restart = true;
                            //sol.TP = false;
                            sol.SpawnTP = 0;
                            sol.execute = false;

                            foreach (Client c in Clients)
                            {
                                if (c != null)
                                {
                                    // Zerando a barra de TP do solicitante
                                    if (!sol.iSpawn)
                                    {
                                        if (sol.Tamer != null && sol.Tamer.Client == c)
                                            c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId, sol.BattleSufix
                                                , 0, "", 3, 0));
                                    }
                                    else
                                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_TP(sol.BattleId
                                            , sol.BattleSufix, sol.SpawnTP));

                                    // Executando a ação
                                    int team = 2;
                                    if (defesa == EquipeA) team = 1;
                                    // Em caso de batalhas PvP, que são vistas sempre pela mesma perspectiva independente
                                    // de quem seja o desafiante ou desafiado, precisamos então inverter a variável team
                                    // no caso de quem estiver na EquipeB
                                    if (c.Team == 1)
                                    {
                                        if (team == 2) team = 1;
                                        else team = 2;
                                    }
                                    // Enviando a execução da ação
                                    c.Connection.Send(new Network.Packets.PACKET_BATTLE_EXECUTE_ACTION(ataque
                                        , defesa, team, skillId, slash));
                                }
                            }
                            Limpar();
                        }
                        else
                        {
                            Limpar();
                            Prosseguir();
                        }

                        break;
                    // Digievolução
                    case 0x14:
                    case 0x15:
                    case 0x16:
                        sol.Digivolver(digimon.nextAction);
                        sol.restart = true;
                        //sol.TP = false;
                        sol.SpawnTP = 0;
                        sol.execute = false;
                        foreach (Client c in Clients)
                        {
                            if (c != null)
                            {
                                // Zerando a barra de TP do solicitante
                                if (!sol.iSpawn)
                                    if (sol.Tamer != null && sol.Tamer.Client == c)
                                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId
                                        , sol.BattleSufix, 0, "", 3, 0));

                                c.Connection.Send(new Network.Packets.PACKET_BATTLE_DIGIEVOLUTION(sol));

                            }
                        }
                        Limpar();
                        break;
                    // Captura
                    case 0x01:
                        if (alv.Health > 0)
                        {
                            sol.restart = true;
                            //sol.TP = false;
                            sol.SpawnTP = 0;
                            sol.execute = false;
                            // Pausamos o processo de spawn
                            Pause(3000);

                            // Calculando resultado
                            int result = 2;
                            // A função Random() trabalha com números inteiros. Logo, não seria possível fazer cálculos
                            // em % quebradas, como 0.5%. Então vamos trabalhar com um valor máximo de 10000 em vez de 100.
                            // Dessa forma, cada unidade da variável perc representará 0.01%.
                            Random rand = new Random();
                            int perc = 1; // % inicial, 0.01% de chance

                            // Fatores que aumentam a chance
                            // Equipe aliada
                            foreach (Digimon d in EquipeA)
                            {
                                if (d != null && d.Health > 0)
                                {
                                    // Cada digimon vivo aumenta a perc
                                    perc++;
                                    // Digivolvidos, aumenta mais
                                    if (d.estage > 2) perc++;
                                    if (d.estage > 3) perc++;
                                    if (d.estage > 4) perc++;

                                    perc += d.Level / 200;
                                }
                            }
                            // Equipe Inimiga
                            for (int i = 0; i < 5; i++)
                            {
                                Digimon d = EquipeB[i];
                                if (d != null)
                                {
                                    // Cada Digimon abatido, aumenta a %
                                    if (d.Health <= 0)
                                        perc++;
                                    // Os Digimon das bordas também tem % aumentada em relação ao do meio.
                                    // O da borda direita tem maior %.
                                    if (d == alv) perc += i;
                                }
                            }

                            // Quando o alvo está com HP baixo, a % também aumenta
                            if (alv.Health <= 10) perc++;
                            if (alv.Health <= 4) perc++;
                            if (alv.Health <= 3) perc++;
                            if (alv.Health <= 2) perc++;
                            if (alv.Health == 1) perc++;

                            foreach (Client c in Clients)
                            {
                                // Cada Tamer em batalha, também aumenta. O Tamer Level de cada um também afeta.
                                /**
                                if (c.Tamer.Level >= 11) perc++;
                                if (c.Tamer.Level >= 21) perc++;
                                if (c.Tamer.Level >= 31) perc++;
                                if (c.Tamer.Level >= 41) perc++;
                                if (c.Tamer.Level >= 71) perc++;
                                /**/
                                perc += c.Tamer.Level / 200;
                                // Autoridade alta (Compensa o tempo que não temos pra jogar '-')
                                if (c.User.Autoridade >= 500)
                                    perc *= 1;
                            }

                            // Todos os fatores que aumentam % de chance até aqui, 
                            // chegam no máximo a 0.54% de chance adicional

                            if (Emulator.Enviroment.Teste)
                                perc += 0;

                            if (rand.Next(5000) <= perc) // Normal: 10000
                                result = 1;

                            foreach (Client c in Clients)
                            {
                                if (c != null)
                                {
                                    // Zerando a barra de TP do solicitante
                                    if (!sol.iSpawn)
                                        if (sol.Tamer != null && sol.Tamer.Client == c)
                                            c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId
                                            , sol.BattleSufix, 0, "", 3, 0));

                                    c.Connection.Send(new Network.Packets.PACKET_BATTLE_CAPTURA(sol.Tamer, alv, sol
                                        , result));

                                }
                            }

                            // Captura bem sucedida
                            if (result == 1)
                            {
                                Capturado = true;
                                sol.Tamer.AddDigimon(alv);
                                alv.StopBattle();
                                for (int i = 0; i < 5; i++)
                                    if (EquipeB[i] == alv)
                                        EquipeB[i] = null;

                                Prosseguir();
                            }
                        }
                        else Prosseguir();
                        Limpar();
                        //Prosseguir(); // Esta ação não requer delay. Podemos prosseguir com a batalha imediatamente.
                        break;

                    // Chamando Digimons
                    case 0x02:
                        sol.restart = true;
                        //sol.TP = false;
                        sol.SpawnTP = 0;
                        sol.execute = false;

                        // Procurando espaço disponível na equipe do solicitante
                        int espaco = -1;
                        for (int i = 0; i < 5; i++)
                        {
                            if (ataque[i] == null && espaco == -1)
                            {
                                espaco = i;
                                break;
                            }
                        }

                        if (digimon.alvo != null && espaco != -1)
                            foreach (Client c in Clients)
                            {
                                if (c != null)
                                {
                                    // Zerando a barra de TP do solicitante
                                    if (!sol.iSpawn)
                                        if (sol.Tamer != null && sol.Tamer.Client == c)
                                            c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId
                                            , sol.BattleSufix, 0, "", 3, 0));

                                    ataque[espaco] = digimon.alvo;
                                    digimon.alvo.batalha = this;
                                    digimon.alvo.restart = true;
                                    byte team = 1;
                                    if (digimon.Tamer.Client.Team == c.Team)
                                        team = 2;
                                    //digimon.alvo.TP = false;
                                    c.Connection.Send(new Network.Packets.PACKET_BATTLE_RECALL(digimon.alvo, espaco
                                        , team));

                                }
                            }
                        Limpar();
                        break;
                    // Fuga
                    case 0x04:
                        foreach (Client c in Clients)
                        {
                            if (c != null)
                            {
                                sol.BackDigivolve(true, true);
                                // Zerando a barra de TP do solicitante
                                if (!sol.iSpawn)
                                    if (sol.Tamer != null && sol.Tamer.Client == c)
                                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId
                                        , sol.BattleSufix, 0, "", 3, 0));

                                c.Connection.Send(new Network.Packets.PACKET_BATTLE_FUGA(sol.BattleId, sol.BattleSufix));

                                // Removendo o Solicitande da batalha
                                for (int i = 0; i < 5; i++)
                                {
                                    if (EquipeA[i] == sol)
                                    {
                                        //EquipeA[i].batalha = null;
                                        //EquipeA[i].StopBattle();
                                        EquipeA[i] = null;
                                        RestartTP();
                                        break;
                                    }
                                    if (EquipeB[i] == sol)
                                    {
                                        //EquipeB[i].batalha = null;
                                        //EquipeB[i].StopBattle();
                                        EquipeB[i] = null;
                                        RestartTP();
                                        break;
                                    }
                                }
                            }
                        }
                        Limpar();
                        Prosseguir();
                        break;
                }
        }

        // Controlando Execução de ação na fila
        public void ExecuteinAction()
        {
            if (!inAction && ListAction.Any())
            {
                Digimon d = ListAction.Dequeue();
                if (d != null)
                {
                    if (d.Health > 0)
                    {
                        if ((d.TP || (d.iSpawn && d.SpawnTP >= 500)))
                        {
                            Pause(d);
                            if (!d.iSpawn && d.aTimer != null)
                            {
                                d.aTimer.Close();
                            }
                            inAction = true;
                            d.TP = false;
                            ExecuteAction(d);
                        }
                        else
                            ListAction.Enqueue(d);
                        return;
                    }
                }
                ExecuteinAction();
            }
        }

        // Startando temporizador, que vai processar a batalha a cada segundo
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(1000); // 1000 = 1 segundo
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += Batalhando;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        // Temporizador que pausa a batalha (Spawn)
        private void Pause(int time)
        {
            // Create a timer with a two second interval.
            pTimer = new Timer(time); // 1000 = 1 segundo
            // Hook up the Elapsed event for the timer. 
            pTimer.Elapsed += DePause;
            pTimer.AutoReset = false;
            pTimer.Enabled = true;
            Paused = true;
            inAction = true;
        }

        private void DePause(Object source, ElapsedEventArgs e)
        {
            Paused = false;
            //Prosseguir();
        }

        // Função que movimenta o spawn
        private void Batalhando(Object source, ElapsedEventArgs e)
        {
            if (!Paused)
                if (spawnTp > 0)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Digimon d = EquipeB[j];
                        if (d != null && d.Health > 0)
                        {
                            if (d.SpawnTP >= 500 && !ListAction.Contains(d))
                            {
                                //d.SpawnTP = 0;
                                Random r = new Random(DateTime.Now.Millisecond + ((j + 1) * 1000));
                                // Alvo aleatório
                                Digimon digi = null;
                                int index = r.Next(EquipeA.Length);
                                for (int i = 0; i < 5; i++)
                                {
                                    if (EquipeA[index] != null && digi == null)
                                        digi = EquipeA[index];
                                    else
                                    {
                                        index++;
                                        if (index > 4) index = 0;
                                    }
                                }
                                // Skill aleatória
                                d.nextAction = 10;
                                if (d.Level >= 6)
                                    if (r.Next(100) < 40)
                                        d.nextAction = 11;
                                if (d.Level >= 16)
                                    if (r.Next(100) < 30)
                                        d.nextAction = 12;

                                if (digi != null)
                                {
                                    d.alvo = digi;
                                    ListAction.Enqueue(d);
                                }
                            }
                            else if (!inAction && d.Health > 0)
                            {
                                d.SpawnTP += spawnTp;
                                foreach (Client c in Clients)
                                {
                                    if (c != null)
                                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_TP(d.BattleId
                                            , d.BattleSufix, d.SpawnTP));
                                }
                            }
                        }
                    }
                    ExecuteinAction();
                }
        }

        // Função para calcular o fator de dano quanto a tipos (Vacine, Data e Virus)
        private double GetFator(int atk, int def)
        {
            Config c = Emulator.Enviroment.Config;
            // Vacine > Virus
            if (atk == 1 && def == 3) return c.TypeIncrDamage;
            // Vacine > Data
            if (atk == 1 && def == 2) return c.TypeDecrDamage;
            // Data > Vacine
            if (atk == 2 && def == 1) return c.TypeIncrDamage;
            // Data > Virus
            if (atk == 2 && def == 3) return c.TypeDecrDamage;
            // Virus > Data
            if (atk == 3 && def == 2) return c.TypeIncrDamage;
            // Virus > Vacine
            if (atk == 3 && def == 1) return c.TypeDecrDamage;

            return 1;
        }

        // Função para limpar os eventos gravados nos digimons em batalha
        private void Limpar()
        {
            slash = 0;

            foreach (Digimon d in EquipeA)
                if (d != null)
                    d.Limpar();
            foreach (Digimon d in EquipeB)
                if (d != null)
                    d.Limpar();
        }

        // Função para reiniciar a barra amarela (TP)
        public void RestartTP()
        {
            bool EquipeAViva = false;
            bool EquipeBViva = false;
            foreach (Digimon d in EquipeA)
            {
                if (d != null)
                {
                    if (d.restart)
                    {
                        d.restart = false;
                        d.startTp();
                    }
                    if (d.Health > 0) EquipeAViva = true;
                }
            }
            foreach (Digimon d in EquipeB)
            {
                if (d != null)
                {
                    if (d.restart)
                    {
                        d.restart = false;
                        if (!d.iSpawn)
                            d.startTp();
                    }
                    if (d.Health > 0) EquipeBViva = true;
                }
            }

            // Ningué na EquipeA está vivo
            if (!EquipeAViva)
            {
                foreach (Client c in Clients)
                {
                    if (c != null)
                    {
                        byte result = 1;
                        if (c.Team == 2)
                        {
                            result = 2; // Se o Client é da EquipeA, então ele perdeu a batalha
                        }
                        else
                            foreach (Digimon d in c.Tamer.Digimon)
                                if (d != null)
                                {
                                    d.BackDigivolve(true, true);
                                }

                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_RESULT(result, c, IsPvP));

                        // XP para o Tamer
                        if (result == 1)
                        {
                            Random r = new Random();
                            Config conf = Emulator.Enviroment.Config;
                            //foreach (Digimon e in EquipeA)
                            Digimon e = EquipeA[0];
                            bool exp_extra = false;
                            foreach (Digimon d in EquipeB)
                                if (d != null && e != null && d.Tamer != null && d.Level >= e.Level)
                                {
                                    d.Tamer.GainExp(r.Next(conf.TamerXPGainMin, conf.TamerXPGainMax));
                                    if (!exp_extra)
                                        if (d.Tamer.TamerXPExtra > 0)
                                        {
                                            d.Tamer.GainExp(d.Tamer.TamerXPExtra);
                                            exp_extra = true;
                                        }
                                }
                            Utils.Comandos.Send(c, "T_EXP Line - Equipe B win, Equipe A Lose. MinExp: " + conf.TamerXPGainMin + " | MaxEXP" + conf.TamerXPGainMax);
                            // Reputação
                            if (IsPvP && EquipeALevel >= EquipeBLevel)
                                c.Tamer.AddReputation(10 + EquipeALevel - EquipeBLevel);
                        }
                        else
                        {
                            // Reputação
                            if (IsPvP)
                            {
                                if (EquipeBLevel >= EquipeALevel)
                                    c.Tamer.AddReputation(-10);
                                else
                                    c.Tamer.AddReputation(-10 - (EquipeALevel - EquipeBLevel));
                            }
                        }
                        c.Connection.Send(new Network.Packets.PACKET_TAMER_XP(c.Tamer));
                        c.Tamer.AtualizarDigimon();
                        c.StopBattle();
                    }
                }

                // Se essa batalha foi contra Spawn, os tamers perderam a luta. Se foi contra Boss, devemos
                // desbloquear o spawn
                if (Spawn != null && Spawn.Bloqueado && Spawn.map_id != 0
                    && Emulator.Enviroment.MapZone[Spawn.map_id] != null)
                {
                    MapZone zone = Emulator.Enviroment.MapZone[Spawn.map_id];
                    Spawn.Bloqueado = false;
                    zone.sendSpawn(Spawn);
                }
            }
            // Ningué na EquipeB está vivo
            else if (!EquipeBViva)
            {
                int ID = 0;
                int Map = 0;
                float posx = 0;
                float posy = 0;
                foreach (Client c in Clients)
                {
                    if (c != null)
                    {
                        ID = c.Tamer.Id;
                        Map = c.Tamer.MapId;
                        posx = c.Tamer.Location.X - 1;
                        posy = c.Tamer.Location.Y;
                        byte result = 1;
                        if (c.Team == 1)
                        {
                            result = 2; // Se o Client é da EquipeB, então ele perdeu a batalha

                        }
                        else
                            foreach (Digimon d in c.Tamer.Digimon)
                                if (d != null)
                                {
                                    d.BackDigivolve(true, true);
                                }

                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_RESULT(result, c, IsPvP));

                        // XP para o Tamer
                        if (result == 1)
                        {
                            Random r = new Random();
                            Config conf = Emulator.Enviroment.Config;
                            //foreach (Digimon e in EquipeB)
                            Digimon e = EquipeB[0];
                            bool exp_extra = false;
                            foreach (Digimon d in EquipeA)
                                if (d != null && e != null && d.Tamer != null && e.Level >= d.Level)
                                {
                                    d.Tamer.GainExp(r.Next(conf.TamerXPGainMin, conf.TamerXPGainMax));
                                    if (!exp_extra)
                                        if (d.Tamer.TamerXPExtra > 0)
                                        {
                                            d.Tamer.GainExp(d.Tamer.TamerXPExtra);
                                            exp_extra = true;
                                        }
                                }
                            Utils.Comandos.Send(c, "T_EXP Line - Equipe A win, Equipe B Lose. MinExp: " + conf.TamerXPGainMin + " | MaxEXP" + conf.TamerXPGainMax);
                            // Reputação
                            if (IsPvP && EquipeBLevel >= EquipeALevel)
                                c.Tamer.AddReputation(10 + EquipeBLevel - EquipeALevel);
                        }
                        else
                        {
                            // Reputação
                            if (IsPvP)
                            {
                                if (EquipeALevel >= EquipeBLevel)
                                    c.Tamer.AddReputation(-10);
                                else
                                    c.Tamer.AddReputation(-10 - (EquipeBLevel - EquipeALevel));
                            }
                        }
                        c.Connection.Send(new Network.Packets.PACKET_TAMER_XP(c.Tamer));
                        c.Tamer.AtualizarDigimon();
                        c.StopBattle();
                    }

                }
                // Se a batalha foi contra um Spawn, ele pode ter dropado itens, além da XP do Tamer
                if (Spawn != null && ID != 0 && Map != 0 && Emulator.Enviroment.MapZone[Map] != null)
                {
                    MapZone zone = Emulator.Enviroment.MapZone[Map];

                    //AVISA CASO INV CHEIOS
                    foreach (Client c in Clients)
                    {
                        if (c != null)
                        {
                            if (c.Tamer != null)
                            {
                                int ispacecheck = c.Tamer.ItemSpace();
                                int cspacecheck = c.Tamer.CardSpace();

                                if ((cspacecheck <= 0) && (ispacecheck > 0))
                                {
                                    Utils.Comandos.Send(c, "[AVISO] Mochila de Cards cheio!");
                                }
                                else if ((ispacecheck <= 0) && (cspacecheck > 0))
                                {
                                    Utils.Comandos.Send(c, "[AVISO] Mochila de Itens cheio!");
                                }
                                else if ((ispacecheck <= 0) && (cspacecheck <= 0))
                                {
                                    Utils.Comandos.Send(c, "[AVISO] Mochila de Cards e Itens cheio!");
                                }
                            }
                        }
                    }

                    //[DROP] DROP GLOBAL 
                    string[] drops = Spawn.Drop.Split('/');
                    string[] globalDropRank3 = { "DigiCodeV-1-1" };
                    string[] globalDropRank4 = { "DigiCodeE-1-1", "Evolutor-1-1", "Item Booster-1-1", "Digi Bless-1-1", "Teleport Red-1-1", "PetFood-1-1", "FreshCatchNetX-1-1", "GDrillD-1-1", "BerserkSword-1-1", "OgremonBat-1-1" };
                    string[] globalDropRank5 = { "DigiCodeL-1-1", "Evolutor-1-2", "Item Booster-1-1", "Digi Bless-1-2", "Teleport Red-1-2", "PetFood-1-2", "FreshCatchNetX-1-2", "GDrillD-1-2", "BerserkSword-1-2", "OgremonBat-1-2" };
                    string[] globalDropRank6 = { "DigiCodeH-1-100"};
                    string[] globalDropRank7 = { "DigiCodeH-1-1", "Storage Expansion-1-1", };
                    if (Spawn.rank == 3)//V
                    {
                        //drops[drops.Length] = globalDrop;
                        drops = drops.Concat(globalDropRank3).ToArray();
                    }
                    else if (Spawn.rank == 4)//E
                    {
                        //drops[drops.Length] = globalDrop;
                        drops = drops.Concat(globalDropRank4).ToArray();
                    }
                    else if (Spawn.rank == 5)//L
                    {
                        //drops[drops.Length] = globalDrop;
                        drops = drops.Concat(globalDropRank5).ToArray();
                    }
                    else if (Spawn.rank == 6)//LH - COROA DOURADA
                    {
                        //drops[drops.Length] = globalDrop;
                        drops = drops.Concat(globalDropRank6).ToArray();

                        foreach (Client c in Clients)
                        {
                            if (c != null)
                            {
                                if (c.Tamer != null)
                                {
                                    c.Tamer.GainCoin(1);
                                    Utils.Comandos.Send(c, "Recebeu x1 Coin");
                                    c.Tamer.AtualizarBits();
                                }
                            }
                        }
                    }
                    else if (Spawn.rank == 7)//H COROA DOURADA
                    {
                        //drops[drops.Length] = globalDrop;
                        drops = drops.Concat(globalDropRank7).ToArray();
                    }

                    int j = 1;
                    for (int max = 0; max < drops.Length; max++) // Variável local que delimita a quantidade de itens a cair
                    {
                        string d = drops[max];
                        if (d != null)
                        {
                            string[] items = d.Split('-');
                            if (items.Length == 3 && Emulator.Enviroment.Codex.ContainsKey(items[0]))
                            {
                                //SETA O DROP PRA NULL PRA NAO TER CHANCE DE PEGAR O MESMO DROP
                                //drops[0] = null;

                                string convertChance = "0";
                                if (items[2] == "RobustDropRate")
                                {
                                    items[2] = "50";    //DROP RATE DOS ITENS ROBUST
                                    convertChance = "50";
                                }
                                else
                                {
                                    convertChance = items[2];
                                }
                                int chance = int.Parse(convertChance);
                                //if (Emulator.Enviroment.Teste) chance = 100;
                                Random r = new Random(DateTime.Now.Millisecond + ((j + 1) * 1000));
                                if (r.Next(100) <= chance)
                                {
                                    int q = r.Next(int.Parse(items[1]));
                                    if (q < 1) q = 1;

                                    //FUNCAO QUE ADICIONA O ITEM AO CHAO
                                    /*
                                    ItemCodex item = Emulator.Enviroment.Codex[items[0]];
                                    ItemMap newItem = new ItemMap(item.GetItem(q, r.Next(100) + ID)
                                        , new Data.Vector2(posx, posy), zone, ID);
                                    zone.Items.Add(newItem);
                                    zone.sendItem(newItem);
                                    newItem.Zone = zone;
                                    posx += 1;
                                    */

                                    foreach (Client c in Clients)
                                    {
                                        if (c != null)
                                        {
                                            if (c.Tamer != null)
                                            {
                                                int itemSpace = c.Tamer.ItemSpace();
                                                int cardSpace = c.Tamer.CardSpace();

                                                ItemCodex item = Emulator.Enviroment.Codex[items[0]];

                                                //SE FOR CARD
                                                if (item.ItemTab == 1)
                                                {
                                                    if (cardSpace <= 0)
                                                    {
                                                        //Utils.Comandos.SendGMLayout(c, "AVISO", "Mochila de Cards cheio!");
                                                        //Utils.Comandos.Send(c, "[AVISO] Mochila de Cards cheio!");
                                                    }
                                                    else
                                                    {
                                                        c.Tamer.AddCard(items[0], q, true);
                                                        Utils.Comandos.Send(c, "Recebeu x" + q + " " + items[0]);
                                                    }

                                                }
                                                else
                                                {
                                                    if (itemSpace <= 0)
                                                    {
                                                        //Utils.Comandos.Send(c, "[AVISO] Mochila de Itens cheio!");
                                                    }
                                                    else
                                                    {
                                                        c.Tamer.AddItem(items[0], q, true);
                                                        Utils.Comandos.Send(c, "Recebeu x" + q + " " + items[0]);
                                                    }
                                                }
                                                //Utils.Comandos.Send(c, "Drop Line: " + items[0] );
                                            }
                                        }
                                    }
                                }
                                //Utils.Randomizer.Randomize(drops);
                            }
                            j++;

                        }
                    }
                    //ATUALIZA INV DEPOIS DO DROP
                    foreach (Client c in Clients)
                    {
                        if (c != null)
                        {
                            if (c.Tamer != null)
                            {
                                c.Tamer.AtualizarInventario();
                            }
                        }
                    }

                    if (Spawn.Bloqueado == true)
                    {
                        Spawn.Bloqueado = false;
                        
                    }

                    // Se este foi um Spawn com tempo, devemos enviar o despawn
                    if (Spawn.Tempo > 0)
                    {
                        Utils.Comandos.SendGM("Yggdrasil", Spawn.Name + " foi derrotado!");
                        Spawn.Abatido = true;
                        zone.sendSpawn(Spawn);
                        Spawn.SetRespawnTimer();
                    }
                    else
                    {
                        zone.sendSpawn(Spawn);
                    }


                    //AQUI, CASO SEJA UMA FAKE DG, IRA RETELEPORTAR O TAMER PARA UMA OUTRA AREA
                    foreach (Client c in Clients)
                    {
                        if (c != null)
                        {
                            if (c.Tamer != null)
                            {
                                //CRIA AS CONDICOES E DEPOIS PASSA O TELEPORTE QUE É GG
                                //c.Tamer.Teleport(20, 50, 69);
                            }
                        }
                    }
                }

            }
            // Se não tem ninguém na batalha, então devo destruir esta instância
            if (!EquipeAViva || !EquipeBViva)
            {
                foreach (Digimon d in EquipeA)
                    if (d != null)
                    {
                        d.batalha = null;
                        d.StopBattle();
                    }
                foreach (Digimon d in EquipeB)
                    if (d != null)
                    {
                        d.batalha = null;
                        d.StopBattle();
                    }

                if (aTimer != null)
                    aTimer.Close();
            }
        }

        // Função para pausar o Timer dos Digimon em batalha
        public void Pause(Digimon exceto)
        {
            foreach (Digimon d in EquipeA)
                if (d != null && d != exceto && !d.iSpawn && d.aTimer != null)
                    d.Pause();
            foreach (Digimon d in EquipeB)
                if (d != null && d != exceto && !d.iSpawn && d.aTimer != null)
                    d.Pause();
        }
        // Função para resumir o Timer dos Digimon em batalha
        public void Resume()
        {
            foreach (Digimon d in EquipeA)
                if (d != null && !d.iSpawn && d.aTimer != null)
                    d.Resume();
            foreach (Digimon d in EquipeB)
                if (d != null && !d.iSpawn && d.aTimer != null)
                    d.Resume();
        }

        // Função para reiniciar procedimento após açõ finalizada
        public void Prosseguir()
        {
            Resume();
            inAction = false;
            if (ListAction.Any())
                ExecuteinAction();
            RestartTP();
        }

        // Função para assimilar configuração de Rank (Spawn)
        private void ConfigurarRank(int rank, string name, int spawn_id)
        {
            MapZone zone = Emulator.Enviroment.MapZone[Spawn.map_id];

            if (Spawn.rank == 5 ||  Spawn.rank == 6 || Spawn.rank == 7)
            {
                Spawn.Bloqueado = true;
                zone.sendSpawn(Spawn);
            }

            // Configuração Geral
            if (Emulator.Enviroment.RankConfig.ContainsKey(rank))
            {
                RankConfig Config = Emulator.Enviroment.RankConfig[rank];

                if (Config.TPBarSec != 0)
                    spawnTp = 500 / Config.TPBarSec;
                if (Config.QuantMax != 0)
                    QuantMax = Config.QuantMax;
                if (Config.XPPerc != 0)
                    XPPerc = Config.XPPerc;
                if (Config.HPPerc != 0)
                    HPPerc = Config.HPPerc;
                if (Config.ATKPerc != 0)
                    ATKPerc = Config.ATKPerc;
                if (Config.DEFPerc != 0)
                    DEFPerc = Config.DEFPerc;
                if (Config.BLPerc != 0)
                    BLPerc = Config.BLPerc;
                if (Config.BitPerc != 0)
                    BitPerc = Config.BitPerc;
                if (Config.F2Lvl != 0)
                    F2Lvl = Config.F2Lvl;
                if (Config.F3Lvl != 0)
                    F3Lvl = Config.F3Lvl;
            }
            // Configuração por nome
            if (Emulator.Enviroment.RankConfigName.ContainsKey(name))
            {
                RankConfig Config = Emulator.Enviroment.RankConfigName[name];

                if (Config.TPBarSec != 0)
                    spawnTp = 500 / Config.TPBarSec;
                if (Config.QuantMax != 0)
                    QuantMax = Config.QuantMax;
                if (Config.XPPerc != 0)
                    XPPerc = Config.XPPerc;
                if (Config.HPPerc != 0)
                    HPPerc = Config.HPPerc;
                if (Config.ATKPerc != 0)
                    ATKPerc = Config.ATKPerc;
                if (Config.DEFPerc != 0)
                    DEFPerc = Config.DEFPerc;
                if (Config.BLPerc != 0)
                    BLPerc = Config.BLPerc;
                if (Config.BitPerc != 0)
                    BitPerc = Config.BitPerc;
                if (Config.F2Lvl != 0)
                    F2Lvl = Config.F2Lvl;
                if (Config.F3Lvl != 0)
                    F3Lvl = Config.F3Lvl;
            }
            // Configuração por Spawn ID
            if (Emulator.Enviroment.RankConfigSpawnId.ContainsKey(spawn_id))
            {
                RankConfig Config = Emulator.Enviroment.RankConfigSpawnId[spawn_id];

                if (Config.TPBarSec != 0)
                    spawnTp = 500 / Config.TPBarSec;
                if (Config.QuantMax != 0)
                    QuantMax = Config.QuantMax;
                if (Config.XPPerc != 0)
                    XPPerc = Config.XPPerc;
                if (Config.HPPerc != 0)
                    HPPerc = Config.HPPerc;
                if (Config.ATKPerc != 0)
                    ATKPerc = Config.ATKPerc;
                if (Config.DEFPerc != 0)
                    DEFPerc = Config.DEFPerc;
                if (Config.BLPerc != 0)
                    BLPerc = Config.BLPerc;
                if (Config.BitPerc != 0)
                    BitPerc = Config.BitPerc;
                if (Config.F2Lvl != 0)
                    F2Lvl = Config.F2Lvl;
                if (Config.F3Lvl != 0)
                    F3Lvl = Config.F3Lvl;
            }
        }

        // Destructor
        ~Batalha()
        {

        }
    }
}
