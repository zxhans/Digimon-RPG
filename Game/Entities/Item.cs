using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Digimon_Project.Game.Entities
{
    // Classe que guarda as informações de um tamer
    public class Item : SlotEntity
    {
        public int ItemId { get; set; }
        public byte ItemTag { get; set; }
        public byte ItemType { get; set; }
        /*
        IG_NORMAL = 1, 
        IG_RARE,
        IG_UNIQUE,
        IG_SPECIALL,
        IG_QUEST = 5,
        IG_EVENT = 6,
        IG_SET	 = 7,
         * */

        public byte ItemUseOn { get; set; }
        public int ItemQuant { get; set; }
        public int TradeQuant { get; set; } // Usado no processo de Trade
        public int ItemQuantMax { get; set; }
        public int ItemtamerLvl { get; set; }
        public int ItemEffect1 { get; set; }
        public int ItemEffect1Value { get; set; }
        public int ItemEffect2 { get; set; }
        public int ItemEffect2Value { get; set; }
        public int ItemEffect3 { get; set; }
        public int ItemEffect3Value { get; set; }
        public int ItemEffect4 { get; set; }
        public int ItemEffect4Value { get; set; }
        public int Custo { get; set; }
        public string ItemName { get; set; }
        public int ItemTab = 0;

        public Item(int slot, int id)
            : base(slot, id)
        {

        }

        // Função que salva o item no banco
        public void Save(int tamer_id)
        {
            if (Emulator.Enviroment.Codex.ContainsKey(ItemName))
            {
                ItemCodex item = Emulator.Enviroment.Codex[ItemName];
                Emulator.Enviroment.Database.Update("tamer_inventory", new Database.QueryParameters() {
                { "item_codex_id",  item.Id}, { "item_tag", ItemTag }, { "quantity", ItemQuant } , { "max_quantity", ItemQuantMax }
                , { "slot", Slot }, { "tamer_id", tamer_id }
                }, "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }
        public void Save()
        {
            if (Emulator.Enviroment.Codex.ContainsKey(ItemName))
            {
                ItemCodex item = Emulator.Enviroment.Codex[ItemName];
                Emulator.Enviroment.Database.Update("tamer_inventory", new Database.QueryParameters() {
                { "item_codex_id",  item.Id}, { "item_tag", ItemTag }, { "quantity", ItemQuant }  , { "max_quantity", ItemQuantMax }
                , { "slot", Slot }
                }, "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função que deleta este item no banco
        public void Delete()
        {
            Emulator.Enviroment.Database.Delete("tamer_inventory", "WHERE id=@id", new Database.QueryParameters() {
                { "id", Id } });
        }

        // Função para setar o campo WAREHOUSE no banco
        public void SetWarehouse(int ware)
        {
            // 0 - Não está na warehouse
            // 1 - Está na Warehouse
            Emulator.Enviroment.Database.Update("tamer_inventory", new Database.QueryParameters() {
                { "warehouse", ware } }, "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
        }

        // Função para deminuir uma unidade deste item
        public void Consumir()
        {
            ItemQuant--;
            if (ItemQuant <= 0) Delete();
            else Save();
        }

        // Funções para retornar a posição do item no inventário em coordenadas (linha, coluna)
        public int GetLinha()
        {
            if (Slot >= 7 && Slot <= 12) return 1;
            if (Slot >= 13 && Slot <= 18) return 2;
            if (Slot >= 19 && Slot <= 24) return 3;
            return 0;
        }
        public int GetColuna()
        {
            if (Slot == 2 || Slot == 8 || Slot == 14 || Slot == 20) return 1;
            if (Slot == 3 || Slot == 9 || Slot == 15 || Slot == 21) return 2;
            if (Slot == 4 || Slot == 10 || Slot == 16 || Slot == 22) return 3;
            if (Slot == 5 || Slot == 11 || Slot == 17 || Slot == 23) return 4;
            if (Slot == 6 || Slot == 12 || Slot == 18 || Slot == 24) return 5;
            return 0;
        }

        // Verificando se este item possui certo Effect ID
        public bool CheckEffect(int eff)
        {
            if (ItemEffect1 == eff) return true;
            if (ItemEffect2 == eff) return true;
            if (ItemEffect3 == eff) return true;
            if (ItemEffect4 == eff) return true;
            return false;
        }

        // Processamento -- Executando os efeitos do item
        // Executando efeitos dos Itens / Cards e Itens podem ter o mesmo Effect ID, mas que tem os efeitos diferentes.
        public int ExecuteItem(Digimon d)
        {
            return ExecuteItem(new Digimon[] { d });
        }
        public int ExecuteItem(Digimon[] digimon)
        {
            int ok1 = ItemEffect(digimon, ItemEffect1, ItemEffect1Value);
            int ok2 = ItemEffect(digimon, ItemEffect2, ItemEffect2Value);
            int ok3 = ItemEffect(digimon, ItemEffect3, ItemEffect3Value);
            int ok4 = ItemEffect(digimon, ItemEffect4, ItemEffect4Value);

            /**
            if (ok1 || ok2 || ok3 || ok4)
            {
                return 1;
            }
            /**/

            if (this.ItemId == 914)
            {
                Utils.Comandos.Send(digimon[0].Tamer.Client, "Usou " + this.ItemName);
            }
            else
            {
                Utils.Comandos.Send(digimon[0].Tamer.Client, "Usou " + this.ItemName);
            }

            return Math.Max(Math.Max(Math.Max(ok3, ok4), ok2), ok1);
        }
        private int ItemEffect(Digimon d, int eff, int value)
        {
            return ItemEffect(new Digimon[] { d }, eff, value);
        }
        private int ItemEffect(Digimon[] digimon, int eff, int value)
        {
            int consumivel = 0;
            if (digimon[0].Tamer.Level >= ItemtamerLvl)
                switch (eff)
                {
                    case 30:
                        digimon[0].AddHP(value);
                        digimon[0].SaveFloppy();
                        consumivel = 1;
                        break;
                    case 31:
                        digimon[0].AddVP(value);
                        digimon[0].SaveFloppy();
                        consumivel = 1;
                        break;
                    case 32:
                        digimon[0].AddEVP(value);
                        digimon[0].SaveFloppy();
                        consumivel = 1;
                        break;
                    case 40:
                        foreach (Digimon d in digimon)
                            if (d != null && d.Health <= 0)
                                d.Health = d.MaxHealth / 10;
                        digimon[0].SaveFloppy();
                        consumivel = 1;
                        break;
                    case 63:
                        digimon[0].AddHP((digimon[0].MaxHealth * 100) / value);
                        digimon[0].SaveFloppy();
                        consumivel = 1;
                        break;
                    case 64:
                        digimon[0].AddVP((digimon[0].MaxVP * 100) / value);
                        digimon[0].SaveFloppy();
                        consumivel = 1;
                        break;
                    case 65:
                        digimon[0].AddEVP((digimon[0].MaxEV * 100) / value);
                        digimon[0].SaveFloppy();
                        consumivel = 1;
                        break;
                    case 72:
                        //PET FOOD GERAL
                        if ((digimon[0].Tamer.Pet >= 1) && (digimon[0].Tamer.Pet <= 4))
                        {
                            digimon[0].Tamer.AddPetHP(50);
                            consumivel = 1;
                        }
                        break;
                    case 79:
                        //PET FOOD GERAL
                        if (digimon[0].Tamer.Pet == 5)
                        {
                            digimon[0].Tamer.AddPetHP(50);
                            consumivel = 1;
                        }
                        break;
                    case 100:
                        short x = 50;
                        short y = 69;
                        switch (value)
                        {
                            case 1:
                                x = 58;
                                y = 82;
                                break;
                            case 5:
                                x = 68;
                                y = 99;
                                break;
                            case 34:
                                x = 28;
                                y = 40;
                                break;
                            case 36:
                                x = 15;
                                y = 16;
                                break;
                            case 40:
                                x = 83;
                                y = 172;
                                break;
                            case 60:
                                x = 94;
                                y = 97;
                                break;
                        }
                        digimon[0].Tamer.Teleport(value, x, y);
                        consumivel = 2;
                        break;
                    case 200:
                        string name = "Evo Card Megidramon 3";
                        int qntd = 1;
                        ItemCodex item = Emulator.Enviroment.Codex[name];
                        digimon[0].Tamer.AddItem(name, qntd, false);
                        Utils.Comandos.Send(digimon[0].Tamer.Client, "Recebeu x" + qntd.ToString() + " " + name + "");
                        digimon[0].Tamer.Client.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(digimon[0].Tamer));
                        break;
                }

            return consumivel;
        }
        // Executando efeitos dos Cards / Cards e Itens podem ter o mesmo Effect ID, mas que tem os efeitos diferentes.
        public int ExecuteCard(Digimon d, Digimon[] digimons, int pos)
        {
            int returnFinalSlash = 1;
            if (d != null && d.Health > 0)
            {
                int slash1return;
                int slash2return;
                int slash3return;
                int slash4return;

                slash1return = CardEffect(d, digimons, ItemEffect1, ItemEffect1Value);
                slash2return = CardEffect(d, digimons, ItemEffect2, ItemEffect2Value);
                slash3return = CardEffect(d, digimons, ItemEffect3, ItemEffect3Value);
                slash4return = CardEffect(d, digimons, ItemEffect4, ItemEffect4Value);

                ItemQuant--;
                if (ItemQuant <= 0) Delete();
                else Save();

                //d.Tamer.Client.Connection.Send(new Network.Packets.PACKET_CCC8(this));

                switch (pos)
                {
                    case 1:
                        d.card1 = this;
                        break;
                    case 2:
                        d.card2 = this;
                        break;
                    case 3:
                        d.card3 = this;
                        break;
                }

                returnFinalSlash = Math.Max(slash1return, Math.Max(slash2return, Math.Max(slash3return, slash4return)));
            }

            return returnFinalSlash;
            //RETORNO SERA DIFERENTE PARA CADA TIPO DE CARD SLASH
            /*
            CARD_SLASH_NOEFFECT = 0,	// Ä«µå ½½·¡½¬ ¾øÀ½
		    CARD_SLASH_NORMAL = 1,	// ÀÏ¹Ý Ä«µå ½½·¡½¬
		    CARD_SLASH_SPECIAL = 2,	// ½ºÆä¼È Ä«µå(´ë¿ø ºÎ½ºÅÍ Ä«µå) ½½·¡½¬
		    CARD_SLASH_ALL_DELETE = 11,	// ALL DELETE
		    CARD_SLASH_ULTIMATE_KING_DRAGON = 12,	// ±Ã±Ø¼ºÁø ¿Õ·æ°Ë
		    CARD_SLASH_FINAL_ELYSIAN = 13,	// ÆÄÀÌ³Î ¿¡¸®½Ã¿Â
		    CARD_AVALONS_GATE = 14,	//¾Æ¹ß·ÐÁî °ÔÀÌÆ®
		    CARD_EMPEROR_CANINE = 15,   //È²Á¦ÀÇ ¼Û°÷´Ï
		    CARD_DARKNESS_ZONE = 16,   //´ÙÅ©´Ï½º Á¸
                */

        }
        private int CardEffect(Digimon digimon, Digimon[] digimons, int eff, int value)
        {
            int returnSlash = 1;
            switch (eff)
            {
                case 1:
                    digimon.ExtraBLevel += value;
                    break;
                case 2:
                    digimon.ExtraBLevel -= value;
                    break;
                case 3:
                    digimon.ExtraAttack += value;
                    break;
                case 4:
                    digimon.ExtraAttack -= value;
                    break;
                case 5:
                    digimon.ExtraDefense += value;
                    break;
                case 6:
                    digimon.ExtraDefense -= value;
                    break;
                case 7:
                    digimon.ExtraDamage += value;
                    break;
                case 8:
                    digimon.ExtraDamage -= value;
                    break;
                case 20:
                    digimon.Health += value;
                    if (digimon.Health > digimon.MaxHealth) digimon.Health = digimon.MaxHealth;
                    break;
                case 21:
                    foreach (Digimon d in digimons)
                    {
                        if (d != null && d.Health > 0)
                        {
                            d.Health += value;
                            if (d.Health > d.MaxHealth) d.Health = d.MaxHealth;
                        }
                    }
                    break;
                case 22:
                    digimon.VP += value;
                    if (digimon.VP > digimon.MaxVP) digimon.VP = digimon.MaxVP;
                    break;
                case 23:
                    foreach (Digimon d in digimons)
                    {
                        if (d != null && d.Health > 0)
                        {
                            d.VP += value;
                            if (d.VP > d.MaxVP) d.VP = d.MaxVP;
                        }
                    }
                    break;
                case 24:
                    digimon.EV += value;
                    if (digimon.EV > digimon.MaxEV) digimon.EV = digimon.MaxEV;
                    break;
                case 25:
                    foreach (Digimon d in digimons)
                    {
                        if (d != null && d.Health > 0)
                        {
                            d.EV += value;
                            if (d.EV > d.MaxEV) d.EV = d.MaxEV;
                        }
                    }
                    break;
                case 30:
                    digimon.Health += digimon.MaxHealth * (value / 100);
                    if (digimon.Health > digimon.MaxHealth) digimon.Health = digimon.MaxHealth;
                    break;
                case 31:
                    foreach (Digimon d in digimons)
                    {
                        if (d != null && d.Health > 0)
                        {
                            d.Health += d.MaxHealth * (value / 100);
                            if (d.Health > d.MaxHealth) d.Health = d.MaxHealth;
                        }
                    }
                    break;
                case 32:
                    digimon.VP += digimon.MaxVP * (value / 100);
                    if (digimon.VP > digimon.MaxVP) digimon.VP = digimon.MaxVP;
                    break;
                case 33:
                    foreach (Digimon d in digimons)
                    {
                        if (d != null && d.Health > 0)
                        {
                            d.VP += d.MaxVP * (value / 100);
                            if (d.VP > d.MaxVP) d.VP = d.MaxVP;
                        }
                    }
                    break;
                case 34:
                    digimon.EV += digimon.MaxEV * (value / 100);
                    if (digimon.EV > digimon.MaxEV) digimon.EV = digimon.MaxEV;
                    break;
                case 35:
                    foreach (Digimon d in digimons)
                    {
                        if (d != null && d.Health > 0)
                        {
                            d.EV += d.MaxEV * (value / 100);
                            if (d.EV > d.MaxEV) d.EV = d.MaxEV;
                        }
                    }
                    break;
                case 36:
                    if (digimon.Health > 1)
                    {
                        int calc = digimon.Health - (digimon.MaxHealth * value / 100);
                        if (calc < 1)
                        {
                            calc = 1;
                        }
                        //Console.WriteLine("SETANDO HP" + calc + " | MAXHEALTH " + digimon.MaxHealth + " VALUE " + value);
                        digimon.setHP(calc);
                    }
                    break;
                case 105:
                    //EFEITO GOLD DRILL - ATAQUE TODOS, GARANTIDO, + 100% DE EXTRA DAMAGE
                    digimon.drill = true;
                    digimon.ExtraDamage += 100;
                    returnSlash = 2;
                    break;
                case 106:
                    //EFEITO GOLD DRILL - ATAQUE TODOS, GARANTIDO, + 100% DE EXTRA DAMAGE
                    digimon.whitewing = true;
                    returnSlash = 2;
                    break;
                case 303:
                    //EFEITO RK - ATAQUE TODOS, GARANTIDO, EXTRA DAMAGE VARIADO
                    if (!digimon.cardrk)
                    {
                        digimon.drill = true;
                        digimon.cardrk = true;
                        Random r = new Random();
                        int chance = r.Next(1, 101);
                        if (chance >= 99)   //2% de chance
                        {
                            Utils.Comandos.Send(digimon.Tamer.Client, "[RK] Omegamon: All Delete! (One Hit KO)");
                            digimon.ExtraDamage += 1000;
                            digimon.onehitko = true;
                            returnSlash = 11;
                        }
                        else if (chance >= 84)  //15% de chance
                        {
                            Utils.Comandos.Send(digimon.Tamer.Client, "[RK] Alphamon: Ultimate War Blade - King Dragon Sword! (500% DMG Increase)");
                            digimon.ExtraDamage += 500;
                            returnSlash = 12;
                        }
                        else
                        {
                            Utils.Comandos.Send(digimon.Tamer.Client, "[RK] Gallantmon: Final Elysian! (200% DMG Increase)");
                            digimon.ExtraDamage += 200;
                            returnSlash = 13;
                        }
                    }
                    break;
                case 304:
                    if (digimon.Health > 0)
                    {
                        digimon.HPAlvoLimite = value;
                    }
                    break;
                case 311:
                    //EFEITO EXAMON - ATAQUE TODOS, GARANTIDO, + 500% DE EXTRA DAMAGE
                    if (!digimon.cardrk)
                    {
                        digimon.drill = true;
                        digimon.cardrk = true;
                        digimon.ExtraDamage += 500;
                        returnSlash = 14;
                        Utils.Comandos.Send(digimon.Tamer.Client, "[RK] Examon: Avalon's Gate! (500% DMG Increase)");
                    }
                    break;
                case 312:
                    //EFEITO FANGLONG - ATAQUE TODOS, GARANTIDO, + 500% DE EXTRA DAMAGE
                    digimon.drill = true;
                    digimon.ExtraDamage += 500;
                    returnSlash = 15;
                    Utils.Comandos.Send(digimon.Tamer.Client, "[GOD-BEAST] Fanglongmon: Fang of the Emperor! (500% DMG Increase)");
                    break;
            }
            return returnSlash;
        }
    }
}
