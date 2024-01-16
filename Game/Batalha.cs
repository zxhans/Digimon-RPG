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
        private int count = 1;
        private int spawnTp = 0;
        public Queue<Digimon> ListAction = new Queue<Digimon>();
        public bool inAction = false;
        private Spawn Spawn = null;
        public bool Capturado = false, Paused = false;
        public int slash = 0;

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
            // Startando a barra Amarela
            //sender.Tamer.Digimon[0].startTp();
            // Assimilando a Batalha ao Digimon
            sender.Tamer.Digimon[0].batalha = this;

            // Adicionando Digimons do Spawn na lista
            Random r = new Random();
            count = r.Next(s.QuantMin, s.Quant + 1);
            if (count < s.QuantMin) count = s.QuantMin;
            if (count < 1) count = 1;
            if (count > 5) count = 5;
            // Teste
            //count = 5;
            for (int i = 0; i < count; i++)
            {
                Digimon spawn = new Digimon(0, 0)
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
                    skill1 = s.skill1,
                    skill2 = s.skill2,
                    BattleId = r.Next(),
                    BattleSufix = s.Name.Substring(0, s.Name.Length <= 8 ? s.Name.Length : 8),
                    batalha = this,
                    rank = (byte)s.rank,
                    iSpawn = true,
                    skill1lvl = 1 + (s.Level - s.skill1.Lvl < 10 ? (s.Level - s.skill1.Lvl) : 9),
                    skill2lvl = 1 + (s.Level - s.skill2.Lvl < 10 ? (s.Level - s.skill2.Lvl) : 9),
                };
                spawnTp = 50;
                spawn.Carregar(s.DigimonId);
                //spawn.startTp(spawnTp);
                EquipeB[i] = spawn;
            }
            sender.Connection.Send(new Network.Packets.PACKET_BATTLE_CENARY(EquipeB, sender.Tamer));
            //sender.Connection.Send(new Network.Packets.PACKET_BATTLE_CENARY(sender.Tamer));
            SetTimer();
        }

        // Preparando Ação em batalha
        public void Action(byte act, long id, string sufix, long alvo, string alvo_sufix, Client c)
        {
            // Procurando Digimons solicitante e alvo
            Digimon sol = null;
            Digimon alv = null;
            foreach(Digimon d in EquipeA)
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

            if(sol != null)
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
                    if(EquipeA[i].Health > 0)
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

                            // EVP
                            if (!sol.iSpawn && sol.estage > 2)
                                sol.AddEVP(-sol.estage * 5);

                            int unit = 1; // Unidades atingidas
                            int skillId = 0;
                            int extra = 1;
                            switch (digimon.nextAction)
                            {
                                case 0x0B:
                                    if (sol.Level >= sol.skill1.Level && sol.VP >= sol.skill1.VP)
                                    {
                                        skillId = sol.skill1.Id;
                                        unit = sol.skill1.Units;
                                        //extra = (103 * sol.skill1lvl) / 100;
                                        extra = 60 * sol.skill1lvl;
                                        sol.VP -= sol.skill1.VP;
                                    }
                                    break;
                                case 0x0C:
                                    if (sol.Level >= sol.skill2.Level && sol.VP >= sol.skill2.VP)
                                    {
                                        skillId = sol.skill2.Id;
                                        unit = sol.skill2.Units;
                                        //extra = (105 * sol.skill2lvl)/100;
                                        extra = 75 * sol.skill2lvl;
                                        sol.VP -= sol.skill2.VP;
                                    }
                                    break;
                            }
                            // Calculando dano e unidades atingidas
                            // Card Slash do Solicitante
                            if (!sol.iSpawn && sol.Tamer != null)
                            {
                                if(sol.Tamer.Slash1 != 0)
                                {
                                    Item card = sol.Tamer.Cards[sol.Tamer.Slash1];
                                    if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 1)
                                        slash = card.ExecuteCard(sol, ataque, 1);
                                }
                                if (sol.Tamer.Slash2 != 0)
                                {
                                    Item card = sol.Tamer.Cards[sol.Tamer.Slash2];
                                    if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 1)
                                        slash = card.ExecuteCard(sol, ataque, 2);
                                }
                                if (sol.Tamer.Slash3 != 0)
                                {
                                    Item card = sol.Tamer.Cards[sol.Tamer.Slash3];
                                    if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 1)
                                        slash = card.ExecuteCard(sol, ataque, 3);
                                }
                            }
                            int vivos = 0;
                            if (defesa == EquipeA) vivos = Avivos;
                            if (defesa == EquipeB) vivos = Bvivos;
                            double dano = (sol.Attack) + extra;
                            if (sol.ExtraAttack != 0) dano += (dano * sol.ExtraAttack) / 100;
                            Debug.Print("Dano incial: {0}", dano);
                            if (!sol.iSpawn && sol.Tamer != null) dano += sol.Attack * (sol.Tamer.AttackBonus / 100);
                            if (unit > 1)
                            {
                                if (unit > vivos) unit = vivos;
                                dano = dano / unit;
                            }
                            Debug.Print("Dano pós unit: {0} vivos: {1}", dano, unit);
                            for (int i = 0; i < 5; i++)
                            {
                                if (unit > 0)
                                {
                                    if (defesa[alvo_index] != null && defesa[alvo_index].Health > 0)
                                    {
                                        // Card Slash do alvo
                                        if (!defesa[alvo_index].iSpawn && defesa[alvo_index].Tamer != null)
                                        {
                                            if (defesa[alvo_index].Tamer.Slash1 != 0)
                                            {
                                                Item card = defesa[alvo_index].Tamer
                                                    .Cards[defesa[alvo_index].Tamer.Slash1];
                                                if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 2)
                                                    slash = card.ExecuteCard(defesa[alvo_index], defesa, 1);
                                            }
                                            if (defesa[alvo_index].Tamer.Slash2 != 0)
                                            {
                                                Item card = defesa[alvo_index].Tamer
                                                    .Cards[defesa[alvo_index].Tamer.Slash2];
                                                if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 2)
                                                    slash = card.ExecuteCard(defesa[alvo_index], defesa, 2);
                                            }
                                            if (defesa[alvo_index].Tamer.Slash3 != 0)
                                            {
                                                Item card = defesa[alvo_index].Tamer
                                                    .Cards[defesa[alvo_index].Tamer.Slash3];
                                                if (card != null && card.ItemQuant > 0 && card.ItemUseOn == 2)
                                                    slash = card.ExecuteCard(defesa[alvo_index], defesa, 3);
                                            }
                                        }
                                        defesa[alvo_index].atacado = 1;
                                        double fator_defesa = 1.5;
                                        int def_limit = 200;
                                        double DEF = (defesa[alvo_index].Defence);
                                        if (defesa[alvo_index].ExtraDefense != 0)
                                            DEF += (defesa[alvo_index].Defence * defesa[alvo_index].ExtraDefense) / 100;
                                        if (!defesa[alvo_index].iSpawn && defesa[alvo_index].Tamer != null)
                                            DEF += (defesa[alvo_index].Defence * defesa[alvo_index].Tamer.DefenseBonus) / 100;
                                        if (DEF > def_limit) fator_defesa += (DEF - def_limit) / 200;
                                        /**/
                                        int damage = Convert.ToInt32(
                                            Math.Round((dano - (DEF / fator_defesa)) * 0.4));
                                        /**/
                                        // Fraquezas de tipo (Vacine, Data, Virus)
                                        damage = Convert.ToInt32(Math.Round(damage * GetFator(sol.type
                                            , defesa[alvo_index].type)));
                                        
                                        if (sol.ExtraDamage > 0)
                                        damage += (damage * sol.ExtraDamage)/ 100;

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

                                        if (DEFBL > ATKBL)
                                        {
                                            int chance = (DEFBL - ATKBL) / 15;
                                            // % de chance de Miss máxima
                                            if (chance > 60) chance = 60;
                                            Random r = new Random();
                                            if (r.Next(100) <= chance)
                                            {
                                                damage = 0;
                                            }
                                        }

                                        // Efeito de Card que limita o HP do alvo (HP Card, become 1 HP)
                                        if(sol.HPAlvoLimite > 0)
                                        {
                                            if (defesa[alvo_index].Health - damage < sol.HPAlvoLimite)
                                                damage = defesa[alvo_index].Health - 1;
                                        }

                                        // Aplicando dano ao HP
                                        defesa[alvo_index].AddHP(-damage);
                                        if (defesa[alvo_index].Health <= 0)
                                        {
                                            defesa[alvo_index].Health = 0;
                                            // Se o alvo morreu, e era um Spawn, então o outro time ganha XP
                                            if (defesa[alvo_index].iSpawn)
                                            {
                                                int xp = Utils.XP.GainForDigimonLevel(defesa[alvo_index].Level)/Avivos;
                                                if (xp < 1) xp = 1;
                                                double bit = Utils.XP.BitForDigimonLevel(defesa[alvo_index].Level);
                                                foreach (Digimon d in EquipeA)
                                                    if (d != null && d.Health > 0 && d.Level - defesa[alvo_index].Level < 10)
                                                    {
                                                        d.GainExp(xp, bit);
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
                                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId, sol.BattleSufix
                                            , 0, "", 3, 0));
                                    else
                                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_TP(sol.BattleId
                                            , sol.BattleSufix, sol.SpawnTP));

                                    // Executando a ação
                                    int team = 2;
                                    if (defesa == EquipeA) team = 1;
                                    c.Connection.Send(new Network.Packets.PACKET_BATTLE_EXECUTE_ACTION(ataque
                                        , defesa, team, skillId, slash));
                                }
                            }
                            Limpar();
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
                                }
                            }
                            foreach (Client c in Clients)
                            {
                                // Cada Tamer em batalha, também aumenta. O Tamer Level de cada um também afeta.
                                if (c.Tamer.Level >= 11) perc++;
                                if (c.Tamer.Level >= 21) perc++;
                                if (c.Tamer.Level >= 31) perc++;
                                if (c.Tamer.Level >= 41) perc++;
                                if (c.Tamer.Level >= 71) perc++;
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

                            // Todos os fatores que aumentam % de chance até aqui, 
                            // chegam no máximo a 0.54% de chance adicional

                            if (Emulator.Enviroment.Teste)
                                perc += 8000;

                            if (rand.Next(10000) <= perc)
                                result = 1;

                            foreach (Client c in Clients)
                            {
                                if (c != null)
                                {
                                    // Zerando a barra de TP do solicitante
                                    if (!sol.iSpawn)
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
                            }
                        }
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
                        for(int i = 0; i < 5; i++)
                        {
                            if(ataque[i] == null && espaco == -1)
                            {
                                espaco = i;
                                break;
                            }
                        }

                        if(digimon.alvo != null && espaco != -1)
                        foreach (Client c in Clients)
                        {
                            if (c != null)
                            {
                                // Zerando a barra de TP do solicitante
                                if (!sol.iSpawn)
                                    c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId
                                        , sol.BattleSufix, 0, "", 3, 0));

                                ataque[espaco] = digimon.alvo;
                                digimon.alvo.batalha = this;
                                digimon.alvo.restart = true;
                                //digimon.alvo.TP = false;
                                c.Connection.Send(new Network.Packets.PACKET_BATTLE_RECALL(digimon.alvo, espaco));

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
                                sol.BackDigivolve(true);
                                // Zerando a barra de TP do solicitante
                                if (!sol.iSpawn)
                                    c.Connection.Send(new Network.Packets.PACKET_BATTLE_ACTION(sol.BattleId
                                        , sol.BattleSufix, 0, "", 3, 0));

                                c.Connection.Send(new Network.Packets.PACKET_BATTLE_FUGA(sol.BattleId, sol.BattleSufix));

                                // Removendo o Solicitande da batalha
                                for(int i = 0; i < 5; i++)
                                {
                                    if(EquipeA[i] == sol)
                                    {
                                        EquipeA[i].batalha = null;
                                        EquipeA[i].StopBattle();
                                        EquipeA[i] = null;
                                        RestartTP();
                                        break;
                                    }
                                    if (EquipeB[i] == sol)
                                    {
                                        EquipeB[i].batalha = null;
                                        EquipeB[i].StopBattle();
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
            if(!Paused)
            if(spawnTp > 0)
            {
                for(int j = 0; j < 5; j++)
                {
                    Digimon d = EquipeB[j];
                    if (d != null)
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
            // Vacine > Virus
            if (atk == 1 && def == 3) return 1.2;
            // Vacine > Data
            if (atk == 1 && def == 2) return .8;
            // Data > Vacine
            if (atk == 2 && def == 1) return 1.2;
            // Data > Virus
            if (atk == 2 && def == 3) return .8;
            // Virus > Data
            if (atk == 3 && def == 2) return 1.2;
            // Virus > Vacine
            if (atk == 3 && def == 1) return .8;

            return 1;
        }

        // Função para limpar os eventos gravados nos digimons em batalha
        private void Limpar()
        {
            slash = 0;

            foreach (Digimon d in EquipeA)
                if(d != null)
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
                    else
                        d.StopBattle();
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
                    else
                        d.StopBattle();
                }
            }

            // Ningué na EquipeA está vivo
            if (!EquipeAViva)
            {
                foreach(Client c in Clients)
                {
                    if(c != null)
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
                                    d.BackDigivolve(true);
                                }

                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_RESULT(result, c));
                        c.StopBattle();
                    }
                }
            }
            // Ningué na EquipeB está vivo
            if (!EquipeBViva)
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
                            result = 2; // Se o Client é da EquipeB, então ele perdeu a batalhaforeach (Digimon d in c.Tamer.Digimon)
                            
                        }
                        else
                            foreach (Digimon d in c.Tamer.Digimon)
                                if (d != null)
                                {
                                    d.BackDigivolve(true);
                                }

                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_RESULT(result, c));
                        c.StopBattle();
                    }
                }
                // Se a batalha foi contra um Spawn, ele pode ter dropado itens, além da XP do Tamer
                if (Spawn != null && ID != 0 && Map != 0 && Emulator.Enviroment.MapZone[Map] != null)
                {
                    MapZone zone = Emulator.Enviroment.MapZone[Map];
                    string[] drops = Spawn.Drop.Split('/');
                    foreach (string d in drops)
                    {
                        string[] items = d.Split('-');
                        if (items.Length == 3 && Emulator.Enviroment.Codex.ContainsKey(items[0]))
                        {
                            int chance = int.Parse(items[2]);
                            Random r = new Random();
                            if (r.Next(100) <= chance)
                            {
                                int q = r.Next(int.Parse(items[1]));
                                if (q < 1) q = 1;

                                ItemCodex item = Emulator.Enviroment.Codex[items[0]];
                                ItemMap newItem = new ItemMap(item.GetItem(q, r.Next(100) + ID)
                                    , new Data.Vector2(posx, posy), zone, ID);
                                zone.Items.Add(newItem);
                                zone.sendItem(newItem);
                                posx += 1;
                            }
                        }
                    }
                }

                // XP para o Tamer
                foreach (Client c in Clients)
                {
                    foreach (Digimon e in EquipeB)
                        if (c != null)
                            foreach (Digimon d in EquipeA)
                                if (d != null && e != null && e.Level >= d.Level)
                                    d.Tamer.GainExp(1 + (e.Level - d.Level));
                    c.Connection.Send(new Network.Packets.PACKET_TAMER_XP(c.Tamer));
                }

            }
            // Se não tem ninguém na batalha, então devo destruir esta instância
            if (!EquipeAViva || !EquipeBViva)
            {
                foreach (Digimon d in EquipeA)
                    if (d != null)
                        d.batalha = null;
                foreach (Digimon d in EquipeB)
                    if (d != null)
                        d.batalha = null;

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

        // Destructor
        ~Batalha()
        {
            Debug.Print("batalha destruida");
        }
    }
}
