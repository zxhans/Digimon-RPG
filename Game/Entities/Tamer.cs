using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Digimon_Project.Game.Entities
{
    // Classe que guarda as informações de um tamer
    public class Tamer : SlotEntity
    {
        public Client Client { get; set; }
        public Vector2 Location { get; set; }
        public List<Digimon> Digimon { get; set; } // Lista de Digimons com o Tamer
        public List<Digimon> Digistorage { get; set; } // Lista de Digimons no Digistore
        public Item[] Items;
        public Item[] Cards;
        // Warehouse
        public Item[] WareItems;
        public Item[] WareCards;
        public int Digistore = 0; // Número de slots liberados
        public List<Quest> Quests { get; set; }
        public Party Party { get; set; }
        public int MapId { get; set; }
        public int Sock { get; set; }
        public int Shoes { get; set; }
        public int Pants { get; set; }
        public int Glove { get; set; }
        public int Tshirt { get; set; }
        public int Jacket { get; set; }
        public int Hat { get; set; }
        public int Customer { get; set; }
        public int Aura { get; set; }
        public int Rank { get; set; }
        public long Bits { get; set; }
        public long Coin { get; set; }
        public long Gold { get; set; }
        public int Pet { get; set; }
        public int PetHP { get; set; }
        // Card Slash (Usado em Batalha)
        public int Slash1 { get; set; }
        public int Slash2 { get; set; }
        public int Slash3 { get; set; }

        public int Wins { get; set; }
        public int Battles { get; set; }
        public int Reputation { get; set; }

        public int Authority { get; set; }
        public int ItemStart { get; set; }
        public int ItemTagStart { get; set; }
        public int CardStart { get; set; }
        public int CardTagStart { get; set; }

        // Bônus (Equipamentos)
        public int STRBonus { get; set; }
        public int DEXBonus { get; set; }
        public int CONBonus { get; set; }
        public int INTBonus { get; set; }
        public int AttackIncr { get; set; }
        public int DefenseIncr { get; set; }
        public int BLevelIncr { get; set; }
        public int MaxHPIncr { get; set; }
        public int AttackBonus { get; set; } // %
        public int DefenseBonus { get; set; } // %
        public int BLevelBonus { get; set; } // %
        public int MaxHP { get; set; } // %
        public int MaxVP { get; set; } // %
        public int TamerXPExtra { get; set; }
        public int DigimonIDToBecomeFanglong { get; set; }

        public long GUID { get; set; } // Identificador usado in game

        public Tamer(int slot, int id)
            : base(slot, id)
        {
            MapId = 79; // Default Starter Map.

            ItemStart = 0;
            ItemTagStart = 3;
            CardStart = 0;
            CardTagStart = 2;

            Slash1 = 0;
            Slash2 = 0;
            Slash3 = 0;

            DigimonIDToBecomeFanglong = 0;
        }

        // Função para teleportar este Tamer
        public void Teleport(int map, short x, short y)
        {
            MapZone mapZone = Emulator.Enviroment.MapZone[MapId];
            MapZone newMapZone = Emulator.Enviroment.MapZone[map];
            if (mapZone != null && newMapZone != null)
            {
                mapZone.RemoveClient(Client);
                newMapZone.Add(Client);
                Location = new Vector2(x, y);
                MapId = map;
                SaveMap();
                SaveLocation();
                Client.Connection.Send(new Network.Packets.PACKET_TELEPORT(this));
            }
        }
        public void Teleport(int map)
        {
            MapZone mapZone = Emulator.Enviroment.MapZone[MapId];
            MapZone newMapZone = Emulator.Enviroment.MapZone[map];
            if (mapZone != null && newMapZone != null)
            {
                mapZone.RemoveClient(Client);
                newMapZone.Add(Client);
                MapId = map;
                SaveMap();
                SaveLocation();
                Client.Connection.Send(new Network.Packets.PACKET_TELEPORT(this));
            }
        }
        public void Teleport(short x, short y)
        {
            MapZone mapZone = Emulator.Enviroment.MapZone[MapId];
            if (mapZone != null)
            {
                Location = new Vector2(x, y);
                SaveMap();
                SaveLocation();
                Client.Connection.Send(new Network.Packets.PACKET_TELEPORT(this));
            }
        }
        public void Teleport(Teleport t)
        {
            if (Level >= t.Level && Rank >= t.Rank)
            {
                MapZone mapZone = Emulator.Enviroment.MapZone[MapId];
                MapZone newMapZone = Emulator.Enviroment.MapZone[t.Alvo];
                if (mapZone != null && newMapZone != null)
                {
                    mapZone.RemoveClient(Client);
                    newMapZone.Add(Client);
                    Location = new Vector2(t.AlvoX, t.AlvoY);
                    MapId = t.Alvo;
                    SaveMap();
                    SaveLocation();
                    if (Client.User.Autoridade >= 100)
                    {
                        Utils.Comandos.Send(Client, "ID do portal: " + t.Id);
                    }
                    Client.Connection.Send(new Network.Packets.PACKET_TELEPORT(this));
                }
                else
                {
                    Utils.Comandos.Send(Client, "Level e/ou Rank insuficiente!");
                }
            }
            else
            {
                Utils.Comandos.Send(Client, "Level e/ou Rank insuficiente!");
            }
        }

        // Função para enviar este Tamer para os outros no mapa
        public void SendSelf()
        {
            MapZone map = Emulator.Enviroment.MapZone[MapId];
            if (map != null)
                map.SendTamer(this);
        }

        // Função para incrementar o contador de Batalhas
        public void IcrBattles(byte result)
        {
            Battles++;
            if (result == 1) Wins++;
            SaveBattles();
        }

        // Função para organizar a lista de Digimons na party e digistore
        public void SortDigimonSlots()
        {
            // Party
            for (int i = 0; i <= Digimon.Count - 1; i++)
            {
                if (Digimon[i].Slot != i || Digimon[i].Digistore != 0)
                {
                    Digimon[i].Slot = i;
                    Digimon[i].Digistore = 0;
                    Digimon[i].SaveSlot();
                }
            }
            // Digistore
            for (int i = 0; i <= Digistorage.Count - 1; i++)
            {
                if (Digistorage[i].Slot != i || Digistorage[i].Digistore != 1)
                {
                    Digistorage[i].Slot = i;
                    Digistorage[i].Digistore = 1;
                    Digistorage[i].SaveSlot();
                }
            }
        }

        // Função para incrementar Reputação
        public void AddReputation(int rep)
        {
            Reputation += rep;
            if (Reputation < 0) Reputation = 0;

            Emulator.Enviroment.Database.Update("tamers"
                , new QueryParameters() { { "reputation", Reputation } }
                , "where id = @id"
                , new QueryParameters() { { "@id", Id } }
                , "");
        }

        // Função para salvar o contador de batalhas
        public void SaveBattles()
        {
            Emulator.Enviroment.Database.Update("tamers"
                , new QueryParameters() { { "battles_total", Battles }, { "battles_won", Wins } }
                , "where id = @id"
                , new QueryParameters() { { "@id", Id } }
                , "");
        }

        // Função para salvar posição deste Tamer
        public void SaveLocation()
        {
            Emulator.Enviroment.Database.Update("tamers"
                , new QueryParameters() { { "location_x", (short)Location.X }, { "location_y", (short)Location.Y } }
                , "where id = @id"
                , new QueryParameters() { { "@id", Id } }
                , "");
        }
        public void SaveLocation(int x, int y)
        {
            Emulator.Enviroment.Database.Update("tamers"
                , new QueryParameters() { { "location_x", x }, { "location_y", y } }
                , "where id = @id"
                , new QueryParameters() { { "@id", Id } }
                , "");
        }
        public void SaveMap()
        {
            Emulator.Enviroment.Database.Update("tamers"
                , new QueryParameters() { { "map_id", MapId } }
                , "where id = @id"
                , new QueryParameters() { { "@id", Id } }
                , "");
        }

        // Função para adicionar Bits e salvar no banco
        public void GainBit(int bit)
        {
            Bits += bit;
            Emulator.Enviroment.Database.Update("tamers", new QueryParameters() { { "bits", Bits } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }
        public void GainCoin(int coin)
        {
            Coin += coin;
            Emulator.Enviroment.Database.Update("tamers", new QueryParameters() { { "coins", Coin } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }
        public void GainBit(double bit)
        {
            Bits += (long)bit;
            Emulator.Enviroment.Database.Update("tamers", new QueryParameters() { { "bits", Bits } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }

        // Função para incrementar o HP do Pet
        public void AddPetHP(int hp)
        {
            if (Pet != 0)
            {
                PetHP += hp;
                if (PetHP <= 0)
                {
                    PetHP = 0;
                    Pet = 0;
                    Utils.Comandos.Send(this.Client, "[AVISO] Seu Pet morreu!");
                }
                if (PetHP > 5000) PetHP = 5000;
                SavePet();
                Client.Connection.Send(new Network.Packets.PACKET_PET_ATT(this));
            }
        }

        // Função para salvar o Pet no banco
        public void SavePet()
        {
            Emulator.Enviroment.Database.Update("tamers", new QueryParameters() { { "pet_type", Pet }
            , { "pet_hp", PetHP } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }

        // Função para expandir o Digistore
        public void ExpandirDigistore()
        {
            Digistore++;

            Emulator.Enviroment.Database.Update("tamers", new QueryParameters() { { "digistore", Digistore } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }

        // Função para adicionar XP
        public void GainExp(int xp)
        {
            if (xp > 0 && Level < Utils.XP.MaxTamerLevel())
            {
                XP += xp;
                //if (Client.User.Autoridade >= 500) XP += xp * 4;
                ExecuteExp();
            }
        }
        private void ExecuteExp()
        {
            if (Level >= Utils.XP.MaxTamerLevel())
            {
                Level = (ushort)Utils.XP.MaxTamerLevel();
                XP = 0;
                MaxXP = 0;
            }
            // Passei de level?
            else if (XP >= MaxXP)
            {
                Level++;
                XP = 0;
                MaxXP = Utils.XP.MaxForTamerLevel(Level);
                // Rank (Digivice)
                if (Level >= 11 && Rank < 2) Rank = 2;
                if (Level >= 21 && Rank < 3) Rank = 3;
                if (Level >= 31 && Rank < 4) Rank = 4;
                if (Level >= 41 && Rank < 5) Rank = 5;

                //PREMIOS POR LEVEL
                if (Level == 11)
                {
                    AddCardWarehouse("GDrillX", 50);
                    AddCardWarehouse("TutorialCard", 50);
                }
                else if (Level == 21)
                {
                    AddItemWarehouse("JumpGateX", 50);
                    AddItemWarehouse("FreshCatchNetX", 1);
                }
                else if (Level == 31)
                {
                    AddItemWarehouse("RokieCatchNetX", 1);
                    AddCardWarehouse("WhiteWingX", 20);
                }
                else if (Level == 41)
                {
                    AddCardWarehouse("GDrillX", 50);
                }
                else if (Level == 51)
                {
                    AddItemWarehouse("Evolutor", 5);
                }
                else if (Level == 61)
                {
                    AddItemWarehouse("Evo Return", 1);
                    AddItemWarehouse("Evo Return", 1);
                }
                else if (Level == 71)
                {
                    AddItemWarehouse("Storage Expansion", 1);
                    AddItemWarehouse("Storage Expansion", 1);
                }
                else if (Level == 81)
                {
                    AddCardWarehouse("ExcaliburX", 50);
                }
                else if (Level == 91)
                {
                    AddCardWarehouse("ExaCard", 22);
                }
                else if (Level == 101)
                {
                    AddCardWarehouse("FangCard", 22);
                }
            }
            SaveXP();
        }

        // Função para salvar XP, Level e Rank
        public void SaveXP()
        {
            Emulator.Enviroment.Database.Update("tamers", new QueryParameters() { { "rank", Rank }
            , { "level", Level }, { "current_exp", XP } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }

        // Função para atualizar componentes visuais do Tamer
        public void Atualizar()
        {
            if (Items != null)
            {
                if (Items[(int)Enums.EquipSlots.tshirt - 1] != null)
                    Tshirt = Items[(int)Enums.EquipSlots.tshirt - 1].ItemId;
                if (Items[(int)Enums.EquipSlots.shoes - 1] != null)
                    Shoes = Items[(int)Enums.EquipSlots.shoes - 1].ItemId;
                if (Items[(int)Enums.EquipSlots.pants - 1] != null)
                    Pants = Items[(int)Enums.EquipSlots.pants - 1].ItemId;
                if (Items[(int)Enums.EquipSlots.glove - 1] != null)
                    Glove = Items[(int)Enums.EquipSlots.glove - 1].ItemId;
                if (Items[(int)Enums.EquipSlots.sock - 1] != null)
                    Sock = Items[(int)Enums.EquipSlots.sock - 1].ItemId;
                if (Items[(int)Enums.EquipSlots.jacket - 1] != null)
                    Jacket = Items[(int)Enums.EquipSlots.jacket - 1].ItemId;
                if (Items[(int)Enums.EquipSlots.hat - 1] != null)
                    Hat = Items[(int)Enums.EquipSlots.hat - 1].ItemId;
                if (Items[(int)Enums.EquipSlots.customer - 1] != null)
                    Customer = Items[(int)Enums.EquipSlots.customer - 1].ItemId;
            }
            else
            {
                TamerVisualResult result = Emulator.Enviroment.Database.Select<TamerVisualResult>(
                    "c.item_idx AS value, i.slot"
                    , "tamer_inventory i, item_codex c", "WHERE i.tamer_id=@id AND c.id = i.item_codex_id"
                    + " AND i.slot BETWEEN @slot AND @slotfin"
                    , new QueryParameters() { { "id", Id }, { "slot", Enums.EquipSlots.sock}
                    , { "slotfin", Enums.EquipSlots.customer}});

                Sock = result.sock;
                Shoes = result.shoes;
                Pants = result.pants;
                Glove = result.glove;
                Tshirt = result.tshirt;
                Jacket = result.jacket;
                Hat = result.hat;
                Customer = result.customer;
            }
        }

        // Função para calcular os atributos Bônus dos equipamentos
        public void CalcularEquips()
        {
            STRBonus = 0;
            DEXBonus = 0;
            CONBonus = 0;
            INTBonus = 0;
            AttackIncr = 0;
            DefenseIncr = 0;
            BLevelIncr = 0;
            AttackBonus = 0;
            DefenseBonus = 0;
            BLevelBonus = 0;
            MaxHP = 0;
            MaxHPIncr = 0;
            MaxVP = 0;
            TamerXPExtra = 0;
            Item item2 = Items[(int)Enums.EquipSlots.earring - 1];// earrings
            Item item3 = Items[(int)Enums.EquipSlots.necklace - 1];//neck
            Item item4 = Items[(int)Enums.EquipSlots.ring - 1];//ring
            for (int i = (int)Enums.EquipSlots.crest1 - 1; i < (int)Enums.EquipSlots.ring; i++)
            {
                Item item = Items[i];
                if (item != null && item.ItemQuant > 0 && item.ItemQuantMax < 1000) // Quando a Quantidade do item é > 0, significa que o item também
                                                        // tem tempo, no caso dos itens de tempo.
                {
                    CalcularEfeito(item.ItemEffect1, item.ItemEffect1Value);
                    CalcularEfeito(item.ItemEffect2, item.ItemEffect2Value);
                    CalcularEfeito(item.ItemEffect3, item.ItemEffect3Value);
                    CalcularEfeito(item.ItemEffect4, item.ItemEffect4Value);
                }
            }
            if (item2 != null && item3 != null && item4 != null && item2.ItemId == 19007 && item3.ItemId == 19010 && item4.ItemId == 19013)
            {
                CalcularEfeito(301, 20);
                CalcularEfeito(302, 40);
                CalcularEfeito(303, 150);
                CalcularEfeito(304, 150);
                CalcularEfeito(305, 250);
            }
            else if (item2 != null && item3 != null && item4 != null && item2.ItemId == 19006 && item3.ItemId == 19009 && item4.ItemId == 19012)
            {

                CalcularEfeito(301, 20);
                CalcularEfeito(303, 150);
                CalcularEfeito(304, 150);
                CalcularEfeito(305, 200);
            }
            else if (item2 != null && item3 != null && item4 != null && item2.ItemId == 19005 && item3.ItemId == 19008 && item4.ItemId == 19011)
            {
                CalcularEfeito(303, 150);
                CalcularEfeito(304, 50);
                CalcularEfeito(305, 200);
            }
            if (Digimon != null)
                foreach (Digimon d in Digimon)
                    if (d != null)
                    {
                        d.Calculate();
                        Client.Connection.Send(new Network.Packets.PACKET_DIGIMON_INDIVIDUAL(d));
                    }
        }
        public void CalcularEfeito(int eff, int value)
        {
            switch (eff)
            {
                case 1:
                    STRBonus += value;
                    break;
                case 2:
                    DEXBonus += value;
                    break;
                case 3:
                    CONBonus += value;
                    break;
                case 4:
                    INTBonus += value;
                    break;
                case 6:
                    STRBonus += value;
                    DEXBonus += value;
                    CONBonus += value;
                    INTBonus += value;
                    break;
                case 10:
                    AttackBonus += value;
                    break;
                case 11:
                    DefenseBonus += value;
                    break;
                case 12:
                    BLevelBonus += value;
                    break;
                case 20:
                    MaxHP += value;
                    break;
                case 21:
                    MaxVP += value;
                    break;
                case 217:
                    TamerXPExtra += value;
                    break;
                case 301:
                    AttackIncr += value;
                    break;
                case 302:
                    DefenseIncr += value;
                    break;
                case 304:
                    MaxHPIncr += value;
                    break;
                case 303:
                    BLevelIncr += value;
                    break;
            }
        }

        // Função para adicionar Digimon
        public void AddDigimon(int id)
        {
            int slot = Digimon.Count;
            Emulator.Enviroment.Database.Insert<int>("digimons", new QueryParameters() {
                        { "tamer_id", Id }, { "digimon_id", id }, { "slot", slot } });

        }
        public void AddDigimon(Digimon d)
        {
            int slot = Digimon.Count;
            int new_id = Emulator.Enviroment.Database.Insert<int>("digimons", new QueryParameters() {
                { "tamer_id", Id }, { "digimon_id", d.DigimonId }, { "slot", slot }, { "hp", d.MaxHealth}
                , { "vp", d.MaxVP }, { "evp", d.MaxEV }, { "rookie", d.RookieForm }, { "champ", d.ChampForm }
                , { "ultim", d.UltimForm }, { "mega", d.MegaForm } });

            DigimonsResult digimons = Client.SelectDigimon(new_id, 0);
            Digimon.Add(digimons.digimonList[0]);
            digimons.digimonList[0].Tamer = this;
            digimons.digimonList[0].CarregarEvolutions();
            digimons.digimonList[0].BattleId = d.BattleId;
            digimons.digimonList[0].BattleSufix = d.BattleSufix;
        }
        public Digimon AddDigimon(Spawn s)
        {
            Digimon d = new Digimon(0, 0)
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
                rank = (byte)s.rank,
                iSpawn = true,
                skill1lvl = 1,
                skill2lvl = 1,
            };
            d.Carregar(s.DigimonId);
            int slot = Digimon.Count;
            int lvl = 1;
            if (s.estage == 2) lvl = 11;
            int new_id = Emulator.Enviroment.Database.Insert<int>("digimons", new QueryParameters() {
                { "tamer_id", Id }, { "digimon_id", d.DigimonId }, { "slot", slot }, { "hp", d.MaxHealth}
                , { "vp", d.MaxVP }, { "evp", d.MaxEV }, { "rookie", d.RookieForm }, { "champ", d.ChampForm }
                , { "ultim", d.UltimForm }, { "mega", d.MegaForm }, { "level", lvl } });

            DigimonsResult digimons = Client.SelectDigimon(new_id, 0);
            Digimon.Add(digimons.digimonList[0]);
            digimons.digimonList[0].Tamer = this;
            digimons.digimonList[0].CarregarEvolutions();
            digimons.digimonList[0].BattleId = s.Id;
            digimons.digimonList[0].BattleSufix = s.GUID.ToString();

            return digimons.digimonList[0];
        }

        // Função que salva um item no banco
        public void SaveItem(int slot)
        {
            if (Items[slot] != null)
            {
                Emulator.Enviroment.Database.Update("tamer_inventory",
                    new QueryParameters() { { "item_codex_id", Items[slot].ItemId }
                    , { "item_tag", Items[slot].ItemTag }
                    , { "quantity", Items[slot].ItemQuant }}
                    , "WHERE tamer_id=@tamer_id AND id=@id AND slot=@slot"
                    , new QueryParameters() {
                        { "tamer_id", Id }
                        , { "id", Items[slot].Id}
                        , { "slot", slot + 1 }
                    });
            }
        }
        // Função que salva um Card no banco
        public void SaveCard(int slot)
        {
            if (Cards[slot] != null)
            {
                Emulator.Enviroment.Database.Update("tamer_inventory",
                    new QueryParameters() { { "item_codex_id", Cards[slot].ItemId }
                    , { "item_tag", Cards[slot].ItemTag }
                    , { "quantity", Cards[slot].ItemQuant }}
                    , "WHERE tamer_id=@tamer_id AND id=@id AND slot=@slot"
                    , new QueryParameters() {
                        { "tamer_id", Id }
                        , { "id", Cards[slot].Id}
                        , { "slot", slot + 1 }
                    });
            }
        }

        // Função que verifica se o há espaço no inventário para adicionar um item
        public bool ItemSpace(string name, int quant)
        {
            int espacoSobrando = 0;

            for (int i = 0; i < 24; i++)
            {
                // Se tem espaço vazio, podemos parar por aqui
                if (Items[i] == null) return true;
                else if (Items[i].ItemName == name)
                {
                    // Se não tem espaço vazio, veremos se pelo menos temos o item específico com espaço
                    // para adicionar
                    espacoSobrando += Items[i].ItemQuantMax - Items[i].ItemQuant;
                    if (espacoSobrando >= quant)
                        return true;
                }
            }

            return false;
        }
        public int ItemSpace()
        {
            int espacoSobrando = 0;

            for (int i = 0; i < 24; i++)
            {
                // Se tem espaço vazio, podemos parar por aqui
                if (Items[i] == null) espacoSobrando++;
            }

            return espacoSobrando;
        }
        // Função que verifica se o há espaço no inventário para adicionar um Card
        public bool CardSpace(string name, int quant)
        {
            int espacoSobrando = 0;

            for (int i = 0; i < 24; i++)
            {
                // Se tem espaço vazio, podemos parar por aqui
                if (Cards[i] == null) return true;
                else if (Cards[i].ItemName == name)
                {
                    // Se não tem espaço vazio, veremos se pelo menos temos o item específico com espaço
                    // para adicionar
                    espacoSobrando += Cards[i].ItemQuantMax - Cards[i].ItemQuant;
                    if (espacoSobrando >= quant)
                        return true;
                }
            }

            return false;
        }
        public int CardSpace()
        {
            int espacoSobrando = 0;

            for (int i = 0; i < 24; i++)
            {
                // Se tem espaço vazio, podemos parar por aqui
                if (Cards[i] == null) espacoSobrando++;
            }

            return espacoSobrando;
        }

        public void AddItemWarehouse(string itemName, int quant)
        {
            ItemCodex item = Emulator.Enviroment.Codex[itemName];

            Emulator.Enviroment.Database.Insert<int>("tamer_inventory", new QueryParameters() {
                { "tamer_id", Id }, { "item_codex_id", item.Id }, { "quantity", quant }, { "warehouse", 3 } });
        }

        public void AddCardWarehouse(string itemName, int quant)
        {
            ItemCodex item = Emulator.Enviroment.Codex[itemName];

            Emulator.Enviroment.Database.Insert<int>("tamer_inventory", new QueryParameters() {
                { "tamer_id", Id }, { "item_codex_id", item.Id }, { "quantity", quant }, { "warehouse", 3 } });
        }

        // Função que adiciona um item ao Tamer
        public Item AddItem(string name, int quant, bool juntar)
        {
            Item result = null;
            // Verificando se temos espaço no inventário para a quantidade informada
            // e se o item existe no Codex
            if (ItemSpace(name, quant) && Emulator.Enviroment.Codex.ContainsKey(name))
            {
                int quantRestante = quant;
                ItemCodex item = Emulator.Enviroment.Codex[name];

                /**/
                if (juntar)
                    for (int i = 0; i < 24; i++)
                    {
                        // Se o Tamer já tem o item, e tem espaço sobrando, devemos aproveitar
                        if (Items[i] != null && Items[i].ItemName == name)
                        {
                            //Console.WriteLine("Items[i].ItemName: {0}, name: {1}", Items[i].ItemName, name);
                            if (Items[i].ItemQuantMax <= 100 // Itens com quantMax > 100 são itens de tempo
                                && Items[i].ItemQuant < Items[i].ItemQuantMax && quantRestante > 0)
                            {
                                int espaco = Items[i].ItemQuantMax - Items[i].ItemQuant;
                                if (espaco >= quantRestante)
                                {
                                    Items[i].ItemQuant += quantRestante;
                                    quantRestante = 0;
                                }
                                else
                                {
                                    Items[i].ItemQuant += (byte)espaco;
                                    quantRestante -= (byte)espaco;
                                }
                                Items[i].Save(Id);
                                result = Items[i];
                            }
                        }
                    }
                /**/
                // Se os itens que já tinham no inventário não foram suficientes para depositar a quantidade
                // solicitada, devemos então criar um novo slot com a quantidade restante.
                // Se não, o item já foi adicionado em slots que já existiam. Podemos encerrar a função.
                if (quantRestante > 0)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        if (Items[i] == null)
                        {
                            // Inserindo no Banco
                            Items[i] = new Item(i + 1, Emulator.Enviroment.Database.Insert<int>("tamer_inventory"
                                , new QueryParameters() { { "tamer_id", Id }
                                , { "item_codex_id", item.Id }, { "item_tag", item.ItemTag } , { "max_quantity", item.ItemQuantMax }
                                , { "quantity", quantRestante}, { "slot", i + 1} }));

                            // Salvando Local
                            Items[i].ItemId = item.ItemId;
                            Items[i].ItemTag = item.ItemTag;
                            Items[i].ItemType = item.ItemType;
                            Items[i].ItemUseOn = item.ItemUseOn;
                            Items[i].ItemQuant = quantRestante;
                            Items[i].ItemQuantMax = item.ItemQuantMax;
                            Items[i].ItemtamerLvl = item.ItemtamerLvl;
                            Items[i].ItemEffect1 = item.ItemEffect1;
                            Items[i].ItemEffect2 = item.ItemEffect2;
                            Items[i].ItemEffect3 = item.ItemEffect3;
                            Items[i].ItemEffect4 = item.ItemEffect4;
                            Items[i].ItemEffect1Value = item.ItemEffect1Value;
                            Items[i].ItemEffect2Value = item.ItemEffect2Value;
                            Items[i].ItemEffect3Value = item.ItemEffect3Value;
                            Items[i].ItemEffect4Value = item.ItemEffect4Value;
                            Items[i].ItemName = item.ItemName;
                            Items[i].Custo = item.Custo;

                            return Items[i];
                        }
                    }
                }
                else
                {
                    return result;
                }

            }

            return result;
        }
        // Função que adiciona um Card ao Tamer
        public Item AddCard(string name, int quant, bool juntar)
        {
            Item result = null;
            // Verificando se temos espaço no inventário para a quantidade informada
            // e se o item existe no Codex
            if (CardSpace(name, quant) && Emulator.Enviroment.Codex.ContainsKey(name))
            {
                int quantRestante = quant;
                ItemCodex item = Emulator.Enviroment.Codex[name];

                /**/ //DESATIVO - Evitando bugs
                if (juntar)
                    for (int i = 0; i < 24; i++)
                    {
                        // Se o Tamer já tem o item, e tem espaço sobrando, devemos aproveitar
                        if (Cards[i] != null && Cards[i].ItemName == name)
                        {
                            //Console.WriteLine("Items[i].ItemName: {0}, name: {1}", Items[i].ItemName, name);
                            if (Cards[i].ItemQuant < Cards[i].ItemQuantMax && quantRestante > 0)
                            {
                                int espaco = Cards[i].ItemQuantMax - Cards[i].ItemQuant;
                                if (espaco >= quantRestante)
                                {
                                    Cards[i].ItemQuant += quantRestante;
                                    quantRestante = 0;
                                }
                                else
                                {
                                    Cards[i].ItemQuant += (byte)espaco;
                                    quantRestante -= (byte)espaco;
                                }
                                Cards[i].Save(Id);
                                result = Cards[i];
                            }
                        }
                    }
                /**/
                // Se os itens que já tinham no inventário não foram suficientes para depositar a quantidade
                // solicitada, devemos então criar um novo slot com a quantidade restante.
                // Se não, o item já foi adicionado em slots que já existiam. Podemos encerrar a função.
                if (quantRestante > 0)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        if (Cards[i] == null)
                        {
                            // Inserindo no Banco
                            Cards[i] = new Item(i + 1, Emulator.Enviroment.Database.Insert<int>("tamer_inventory"
                                , new QueryParameters() { { "tamer_id", Id }
                                , { "item_codex_id", item.Id }, { "item_tag", item.ItemTag }
                                , { "quantity", quantRestante}, { "slot", i + 1} }));

                            // Salvando Local
                            Cards[i].ItemId = item.ItemId;
                            Cards[i].ItemTag = item.ItemTag;
                            Cards[i].ItemType = item.ItemType;
                            Cards[i].ItemUseOn = item.ItemUseOn;
                            Cards[i].ItemQuant = quantRestante;
                            Cards[i].ItemQuantMax = item.ItemQuantMax;
                            Cards[i].ItemtamerLvl = item.ItemtamerLvl;
                            Cards[i].ItemEffect1 = item.ItemEffect1;
                            Cards[i].ItemEffect2 = item.ItemEffect2;
                            Cards[i].ItemEffect3 = item.ItemEffect3;
                            Cards[i].ItemEffect4 = item.ItemEffect4;
                            Cards[i].ItemEffect1Value = item.ItemEffect1Value;
                            Cards[i].ItemEffect2Value = item.ItemEffect2Value;
                            Cards[i].ItemEffect3Value = item.ItemEffect3Value;
                            Cards[i].ItemEffect4Value = item.ItemEffect4Value;
                            Cards[i].ItemName = item.ItemName;
                            Cards[i].Custo = item.Custo;

                            return Cards[i];
                        }
                    }
                }
                else
                {
                    return result;
                }

            }

            return result;
        }

        // Função que Remove um item no Inventário
        public void RemoveItem(string name, int quant)
        {
            Debug.Print("ItemCount(name): {0}, Emulator.Enviroment.Codex.ContainsKey(name): {1}"
                , ItemCount(name), Emulator.Enviroment.Codex.ContainsKey(name));
            if (ItemCount(name) >= quant && Emulator.Enviroment.Codex.ContainsKey(name))
            {
                int quantRestante = quant;
                ItemCodex item = Emulator.Enviroment.Codex[name];

                for (int i = 0; i < 24; i++)
                {
                    if (Items[i] != null && Items[i].ItemName == name)
                    {
                        if (Items[i].ItemQuant > 0 && quantRestante > 0)
                        {
                            int remover = quantRestante;
                            if (Items[i].ItemQuant < remover) remover = Items[i].ItemQuant;
                            Items[i].ItemQuant -= remover;
                            Items[i].Save(Id);
                            if (Items[i].ItemQuant <= 0)
                            {
                                Items[i].Delete();
                                Items[i] = null;
                            }
                            quantRestante -= remover;
                            if (quantRestante <= 0) return;
                        }
                    }
                }

            }
        }
        public void RemoveItem(Item item)
        {
            RemoveItem(item, item.ItemQuant);
        }
        public void RemoveItem(Item item, int quant)
        {
            if (ItemCount(item.ItemName) >= quant)
            {
                int quantRestante = quant;

                for (int i = 0; i < 24; i++)
                {
                    if (Items[i] != null && Items[i] == item)
                    {
                        if (Items[i].ItemQuant > 0 && quantRestante > 0)
                        {
                            int remover = quantRestante;
                            if (Items[i].ItemQuant < remover) remover = Items[i].ItemQuant;
                            Items[i].ItemQuant -= remover;
                            Items[i].Save(Id);
                            if (Items[i].ItemQuant <= 0)
                            {
                                Items[i].Delete();
                                Items[i] = null;
                            }
                            quantRestante -= remover;
                            if (quantRestante <= 0) return;
                        }
                    }
                }

            }
        }
        // Função que Remove um card no Inventário
        public void RemoveCard(string name, byte quant)
        {
            if (ItemCount(name) >= quant && Emulator.Enviroment.Codex.ContainsKey(name))
            {
                byte quantRestante = quant;
                ItemCodex item = Emulator.Enviroment.Codex[name];

                for (int i = 0; i < 24; i++)
                {
                    if (Cards[i] != null && Cards[i].ItemName == name)
                    {
                        //Console.WriteLine("Items[i].ItemName: {0}, name: {1}", Items[i].ItemName, name);
                        if (Cards[i].ItemQuant > 0 && quantRestante > 0)
                        {
                            int remover = quantRestante;
                            if (Cards[i].ItemQuant < remover) remover = Cards[i].ItemQuant;
                            Cards[i].ItemQuant -= (byte)remover;
                            Cards[i].Save(Id);
                            if (Cards[i].ItemQuant <= 0)
                            {
                                Cards[i].Delete();
                                Cards[i] = null;
                            }
                            quantRestante -= (byte)remover;
                            if (quantRestante <= 0) return;
                        }
                    }
                }

            }
        }
        public void RemoveCard(Item item)
        {
            RemoveCard(item, (byte)item.ItemQuant);
        }
        public void RemoveCard(Item item, byte quant)
        {
            if (ItemCount(item.ItemName) >= quant)
            {
                byte quantRestante = quant;

                for (int i = 0; i < 24; i++)
                {
                    if (Cards[i] != null && Cards[i] == item)
                    {
                        //Console.WriteLine("Items[i].ItemName: {0}, name: {1}", Items[i].ItemName, name);
                        if (Cards[i].ItemQuant > 0 && quantRestante > 0)
                        {
                            int remover = quantRestante;
                            if (Cards[i].ItemQuant < remover) remover = Cards[i].ItemQuant;
                            Cards[i].ItemQuant -= (byte)remover;
                            Cards[i].Save(Id);
                            if (Cards[i].ItemQuant <= 0)
                            {
                                Cards[i].Delete();
                                Cards[i] = null;
                            }
                            quantRestante -= (byte)remover;
                            if (quantRestante <= 0) return;
                        }
                    }
                }

            }
        }

        // Função que retorna a quantidade de um item no Inventário
        public int ItemCount(string name)
        {
            int count = 0;

            foreach (Item item in Items)
                if (item != null && item.ItemName == name)
                    count += item.ItemQuant;

            foreach (Item item in Cards)
                if (item != null && item.ItemName == name)
                    count += item.ItemQuant;

            return count;
        }

        // Função que envia o inventário atualizado do Tamer
        public void AtualizarInventario()
        {
            if (Client != null)
                Client.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(this));
            CalcularEquips();
        }

        public void AtualizarBits()
        {
            if (Client != null)
                Client.Connection.Send(new Network.Packets.PACKET_BIT_ATT(this));
        }

        // Função que atualiza os Digimons do Tamer
        public void AtualizarDigimon()
        {
            foreach (Digimon d in Digimon)
                if (d != null)
                    Client.Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(d));
        }
        public void AtualizarDigimon(Digimon d)
        {
            if (d != null)
                Client.Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(d));
        }

        // Função que retorna o andamento de uma dada Quest neste Tamer
        public int GetQuestAndamento(string quest)
        {
            foreach (Quest q in Quests)
                if (q.QuestName == quest)
                    return q.Andamento;

            return 0;
        }

        // Função que retorna uma quest especícifa
        public Quest GetQuest(string quest)
        {
            foreach (Quest q in Quests)
                if (q.QuestName == quest)
                    return q;

            return null;
        }

        // Função para adicionar novas quests
        public void NewQuest(string quest, string tipo, string objetivo)
        {
            if (GetQuestAndamento(quest) == 0)
            {
                // Adicionando no Banco
                Quest q = new Quest(Emulator.Enviroment.Database.Insert<int>(
                    "quests"
                    , new QueryParameters() { { "tamer_id", Id }, { "quest", quest }
                    , { "tipo", tipo}, { "objetivo", objetivo} }
                    ));

                // Salvando localmente
                q.QuestName = quest;
                q.Tipo = tipo;
                q.Objetivo = objetivo;
                Quests.Add(q);
            }
        }

        // Função que avança o andamento da Quest
        public void AvancaQuest(string quest)
        {
            Quest q = GetQuest(quest);
            if (q != null)
            {
                q.Andamento++;

                // Salvando no banco
                Emulator.Enviroment.Database.Update("quests", new QueryParameters() {
                    { "andamento", q.Andamento}
                }, "WHERE quest=@quest AND tamer_id=@tamer_id"
                , new QueryParameters() { { "quest", quest }, { "tamer_id", Id } });
            }
        }
        public void AvancaQuest(string quest, string tipo, string objetivo)
        {
            Quest q = GetQuest(quest);
            if (q != null)
            {
                q.Andamento++;
                q.Tipo = tipo;
                q.Objetivo = objetivo;

                // Salvando no banco
                Emulator.Enviroment.Database.Update("quests", new QueryParameters() {
                    { "andamento", q.Andamento}, { "tipo", tipo}, { "objetivo", objetivo}
                }, "WHERE quest=@quest AND tamer_id=@tamer_id"
                , new QueryParameters() { { "quest", quest }, { "tamer_id", Id } });
            }
        }

        // Destrutor
        ~Tamer()
        {
            Debug.Print("Tamer {0} Destruido", Name);
        }
    }
}
