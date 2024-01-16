using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Network;
using Digimon_Project.Network.Packets;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Digimon_Project.Utils
{
    public static class Comandos
    {
        public static bool Comando(Client sender, string text)
        {
            if (text.Split('/').Length - 1 > 0)
            {
                string[] split = text.Split(' ');

                if (split[0] != null)
                {
                    /////////////////////////////////////////////
                    ///COMANDOS GLOBAIS PARA TODOS OS JOGADORES//
                    /////////////////////////////////////////////

                    //UPGRADE ITEM
                    if (split[0] == "/upgradecrest")
                    {
                        if (sender.Tamer.Items[0] != null && sender.Tamer.Items[1] != null)
                        {
                            if (sender.Tamer.Items[0].ItemId == sender.Tamer.Items[1].ItemId)
                            {
                                if (Emulator.Enviroment.CrestUpgrade.ContainsKey(sender.Tamer.Items[0].ItemId))
                                {
                                    int ingrediente = sender.Tamer.Items[0].ItemId;
                                    string resultado = Emulator.Enviroment.CrestUpgrade[ingrediente];
                                    string tipo = Emulator.Enviroment.CrestType[ingrediente];
                                    bool itembooster = false;
                                    bool blessing = false;
                                    bool requirements = true;

                                    if (tipo == "simples")
                                    {

                                    }
                                    else if (tipo == "booster")
                                    {
                                        if (sender.Tamer.ItemCount("Item Booster") >= 1)
                                        {
                                            itembooster = true;
                                        }
                                        else
                                        {
                                            requirements = false;
                                        }
                                    }
                                    else if (tipo == "blessing")
                                    {
                                        if ((sender.Tamer.ItemCount("Item Booster") >= 1) && (sender.Tamer.ItemCount("Digi Bless") >= 1))
                                        {
                                            itembooster = true;
                                            blessing = true;
                                        }
                                        else
                                        {
                                            requirements = false;
                                        }

                                    }

                                    if (requirements)
                                    {
                                        if (itembooster)
                                        {
                                            sender.Tamer.RemoveItem("Item Booster", 1);
                                        }

                                        if (blessing)
                                        {
                                            sender.Tamer.RemoveItem("Digi Bless", 1);
                                        }

                                        Utils.Comandos.Send(sender, "Upgrade realizado -> recebeu x1 " + resultado);
                                        sender.Tamer.RemoveItem(sender.Tamer.Items[0]);
                                        sender.Tamer.RemoveItem(sender.Tamer.Items[1]);
                                        sender.Tamer.AtualizarInventario();
                                        sender.Tamer.AddItem(resultado, 1, false);
                                        sender.Tamer.AtualizarInventario();
                                    }

                                }
                                else
                                {
                                    Utils.Comandos.Send(sender, "Upgrade inexistente na matrix!");
                                }
                            }
                            else
                            {
                                Utils.Comandos.Send(sender, "Coloque itens iguais para realizar o upgrade!");
                            }
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Coloque os itens para upgrade no Slot 1 e 2 da Mochila de Itens!");
                        }


                        return true;
                    }

                    //LOJAS - Talvez funcione se passar o NPC ID pra uma variavel do Tamer.
                    // Lojinha - em construção
                    if (split[0] == "/loja")
                    {
                        PACKET_NPC_ITEM_SHOP_WRITER write = new PACKET_NPC_ITEM_SHOP_WRITER();
                        write.WriteCards("NPC_BABALAND_CARD_SHOP", sender);
                        return true;
                    }

                    // Lojinha - em construção
                    if (split[0] == "/loja2")
                    {
                        int npcId = 70201;
                        NPC npc = null;
                        npc = NPCATable.Instance.Get((NPCMap)npcId);
                        if (npc != null)
                        {
                            npc.INPC(sender);
                        }
                        return true;
                    }

                    // ItemInfo
                    if (split[0] == "/slayer")
                    {
                        Send(sender, "bugão bug bu g -~10-23123796!@#!@#$)()=");

                        return true;
                    }

                    // ItemInfo
                    if (split[0] == "/iteminfo")
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            Send(sender, "ITEM_IDX [" + sender.Tamer.Items[0].ItemId.ToString() + "] | Name [" + sender.Tamer.Items[0].ItemName.ToString() + "]" +
                                "| Qntd [" + sender.Tamer.Items[0].ItemQuant.ToString() + "]");
                        }
                        else
                        {
                            Send(sender, "Coloque um item no Slot 1 para debugar");
                        }

                        return true;
                    }

                    // DigimonInfo
                    else if (split[0] == "/digimoninfo")
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            Send(sender,
                                "DB_ID [" + sender.Tamer.Digimon[0].Id.ToString() + "] " +
                                "| DIGI_ID [" + sender.Tamer.Digimon[0].DigimonId.ToString() + "] " +
                                "| Model [" + sender.Tamer.Digimon[0].Model.ToString() + "] " +
                                "| Slot [" + sender.Tamer.Digimon[0].Slot.ToString() + "] " +
                                "| MegaForm [" + sender.Tamer.Digimon[0].MegaForm.ToString() + "]");
                        }
                        else
                        {
                            Send(sender, "Coloque um item no Slot 1 para debugar");
                        }

                        return true;
                    }

                    // Card Info
                    else if (split[0] == "/cardinfo")
                    {
                        if (sender.Tamer.Cards[0] != null)
                        {
                            Send(sender, "ITEM_IDX [" + sender.Tamer.Cards[0].ItemId.ToString() + "] | Name [" + sender.Tamer.Cards[0].ItemName.ToString() + "] | Slot " + sender.Tamer.Cards[0].Slot.ToString());
                        }
                        else
                        {
                            Send(sender, "Coloque um item no Slot 1 para debugar");
                        }

                        return true;
                    }

                    // Posicao Info
                    else if (split[0] == "/pos")
                    {
                        if (sender.Tamer != null)
                        {
                            Send(sender, "MAP_ID [" + sender.Tamer.MapId.ToString() + "]" +
                                "| X [" + sender.Tamer.Location.X.ToString() + "]" +
                                "| Y [" + sender.Tamer.Location.Y.ToString() + "]");
                        }
                        else
                        {
                            Send(sender, "Algo de errado aconteceu");
                        }

                        return true;
                    }

                    // Robust Tamer
                    else if (split[0] == "/robust")
                    {
                        if (sender.Tamer.Level >= 201)
                        {
                            string quest = "Robust Tamer";
                            Quest q = sender.Tamer.GetQuest(quest);
                            Tamer t = sender.Tamer;

                            // Primeira fase
                            if (q == null)
                            {
                                /**/
                                if (t.ItemCount("IcemonDrop") >= 30 && t.ItemCount("IcedevimonDrop") >= 30
                                    && t.ItemCount("WeregarurumonDrop") >= 30)
                                {
                                    t.NewQuest(quest, "especial", "Obter os itens da segunda fase.");
                                    t.RemoveItem("IcemonDrop", 30);
                                    t.RemoveItem("IcedevimonDrop", 30);
                                    t.RemoveItem("WeregarurumonDrop", 30);
                                    t.AtualizarInventario();
                                    Send(sender, "Robust Tamer: first phase completed.");
                                }
                                else
                                    Send(sender, "[QUEST_ROBUST_1] Requer: 30xIcemonDrop, 30xIcedevimonDrop"
                                        + " e 30xWeregarurumonDrop.");

                                /**

                                Debug.Print("IcemonDrop: {0}, IcedevimonDrop: {1}, WeregarurumonDrop: {2}"
                                    , t.ItemCount("IcemonDrop"), t.ItemCount("IcedevimonDrop")
                                    , t.ItemCount("WeregarurumonDrop"));
                                /**/
                            }
                            // Segunda fase
                            else if (t.GetQuestAndamento(quest) == 1)
                            {
                                if (t.ItemCount("CyberdramonDrop") >= 30 && t.ItemCount("PaildramonDrop") >= 30
                                    && t.ItemCount("GroundramonDrop") >= 30)
                                {
                                    t.AvancaQuest(quest);
                                    t.RemoveItem("CyberdramonDrop", 30);
                                    t.RemoveItem("PaildramonDrop", 30);
                                    t.RemoveItem("GroundramonDrop", 30);
                                    t.AtualizarInventario();
                                    Send(sender, "Robust Tamer: second phase completed.");
                                }
                                else
                                    Send(sender, "You need 30xCyberdramonDrop, 30xPaildramonDrop"
                                        + " e 30xGroundramonDrop.");
                            }
                            // Terceira fase
                            else if (t.GetQuestAndamento(quest) == 2)
                            {
                                if (t.ItemCount("EtemonDrop") >= 20 && t.ItemCount("WarumonzaemonDrop") >= 20
                                    && t.ItemCount("MonzaemonDrop") >= 20)
                                {
                                    t.AvancaQuest(quest);
                                    t.RemoveItem("EtemonDrop", 20);
                                    t.RemoveItem("WarumonzaemonDrop", 20);
                                    t.RemoveItem("MonzaemonDrop", 20);
                                    t.AtualizarInventario();
                                    Send(sender, "Robust Tamer: third phase completed.");
                                }
                                else
                                    Send(sender, "You need 20xEtemonDrop, 20xWarumonzaemonDrop"
                                        + " e 20xMonzaemonDrop.");
                            }
                            // Quarta fase
                            else if (t.GetQuestAndamento(quest) == 3)
                            {
                                if (t.ItemCount("PhantomonDrop") >= 20 && t.ItemCount("AsuramonDrop") >= 20
                                    && t.ItemCount("CerberumonDrop") >= 20)
                                {
                                    t.AvancaQuest(quest);
                                    t.RemoveItem("PhantomonDrop", 20);
                                    t.RemoveItem("AsuramonDrop", 20);
                                    t.RemoveItem("CerberumonDrop", 20);
                                    t.AtualizarInventario();
                                    Send(sender, "Robust Tamer: fourth phase completed.");
                                }
                                else
                                    Send(sender, "You need 20xPhantomonDrop, 20xAsuramonDrop"
                                        + " e 20xCerberumonDrop.");
                            }
                            // Quinta fase
                            else if (t.GetQuestAndamento(quest) == 4)
                            {
                                if (t.ItemCount("DiaboromonDrop") >= 5 && t.ItemCount("AnubismonDrop") >= 5
                                    && t.ItemCount("SaberleomonDrop") >= 5
                                    & t.ItemCount("BreakdramonDrop") >= 5)
                                {
                                    t.AvancaQuest(quest);
                                    t.RemoveItem("DiaboromonDrop", 5);
                                    t.RemoveItem("AnubismonDrop", 5);
                                    t.RemoveItem("SaberleomonDrop", 5);
                                    t.RemoveItem("BreakdramonDrop", 5);
                                    t.AtualizarInventario();
                                    t.Rank = 6;
                                    t.SaveXP();
                                    Send(sender, "Congratulations, you are now a Robust Tamer!");
                                    //PREMIOS POR ROBUST
                                    t.AddCardWarehouse("RoyalCard", 22);
                                    t.AddCardWarehouse("WhiteWing", 22);
                                    t.AddCardWarehouse("GDrillD", 100);
                                    t.AddItemWarehouse("Item Booster", 5);
                                    t.AddItemWarehouse("Digi Bless", 5);
                                    Utils.Comandos.SendGM("Yggdrasil", t.Name + " became a Robust Tamer! Congratulations!");
                                    sender.Connection.Send(new Network.Packets.PACKET_TAMER_XP(t));
                                }
                                else
                                    Send(sender, "You need 5xDiaboromonDrop, 5xAnubismonDrop,"
                                        + " 5xSaberleomonDrop e 5xBreakdramonDrop.");
                            }
                        }
                        else
                            Send(sender, "You need Tamer level 201.");

                        return true;
                    }

                    // [CRAFT] Sistema de Craft de Itens via Comando
                    else if (split[0] == "/craft")
                    {
                        if (split.Length < 2)
                        {
                            Send(sender, "Digite /craft [item a ser produzido]");
                            Send(sender, "Itens para produzir: omegamonxcard");
                        }
                        // OMEGAMON X CARD
                        else if (split[1] == "omegamonxcard")
                        {
                            string item1 = "AntiBody X";
                            string item2 = "Evo Card Omegamon 1";
                            string prod = "Evo Card Omegamon X";

                            Digimon d = sender.Tamer.Digimon[0];
                            if (d != null)
                            {
                                int bagSpace = sender.Tamer.ItemSpace();

                                if (bagSpace < 2)
                                {
                                    Send(sender, "[MOCHILA CHEIA] 2 Slots na Mochila de Itens para executar comando!");
                                }
                                else if ((sender.Tamer.ItemCount(item1) >= 1) && (sender.Tamer.ItemCount(item2) >= 1))
                                {
                                    // Descontando os itens
                                    sender.Tamer.RemoveItem(item1, 1);
                                    sender.Tamer.RemoveItem(item2, 1);

                                    // Adicionando os itens
                                    sender.Tamer.AddItem("Evo Card Omegamon X", 1, false);
                                    sender.Tamer.AtualizarInventario();

                                    Send(sender, "Produziu x1 [" + prod + "]!");
                                }
                                else
                                    Send(sender, "Requer x1 [" + item1 + "] e x1 [" + item2 + "].");

                            }
                            else
                                Send(sender, "An error has occurred! Try again later.");
                        }
                        else
                        {
                            Send(sender, "Digite /craft [item a ser produzido]");
                            Send(sender, "Itens para produzir: omegamonxcard");
                        }

                        return true;
                    }

                    // [EVOLUTION] Quatro Bestas Sagradas: Baihu, Zhuqiao, Azulong e Ebonwu
                    
                    else if (split[0] == "/baihumon" || split[0] == "/zhuqiaomon"
                        || split[0] == "/azulongmon" || split[0] == "/ebonwumon")
                    {
                        string item = "Baihumon Core";
                        string fera = "Baihumon";

                        switch (split[0])
                        {
                            case "/zhuqiaomon":
                                item = "Zhuqiaomon Core Fire";
                                fera = "Zhuqiaomon";
                                break;
                            case "/azulongmon":
                                item = "Azulongmon Core Ray";
                                fera = "Azulongmon";
                                break;
                            case "/ebonwumon":
                                item = "Ebonwumon Core Green";
                                fera = "Ebonwumon";
                                break;
                        }

                        Digimon d = sender.Tamer.Digimon[0];
                        if (d != null)
                        {
                            if (d.Level >= 105)
                            {
                                if (sender.Tamer.ItemCount(item) >= 12)
                                {
                                    // Adicionando a nova forma
                                    d.NewDigievolution(5, fera);

                                    // Descontando os itens
                                    sender.Tamer.RemoveItem(item, 12);
                                    sender.Tamer.AtualizarInventario();

                                    Send(sender, d.Name + " received the spirit of" + fera + "!");
                                }
                                else
                                    Send(sender, "You need 12 " + item + ".");
                            }
                            else
                                Send(sender, d.Name + " needs to be at least lvl 105 to receive the spirit "
                                    + "of " + fera + ".");
                        }
                        else
                            Send(sender, "An error has occurred! Try again later.");

                        return true;
                    }
                    

                    // [EVENTO] Omegamon X Card
                    else if (split[0] == "/omegaxcard")
                    {
                        string item = "AntiBody X";
                        string fera = "Evo Card Omegamon X";

                        Digimon d = sender.Tamer.Digimon[0];
                        if (d != null)
                        {
                            if (d.Level >= 1)
                            {
                                int bagSpace = sender.Tamer.ItemSpace();
                                int cost = 100;

                                if (bagSpace < 2)
                                {
                                    Send(sender, "[MOCHILA CHEIA] 2 Slots na Mochila de Itens para executar comando!");
                                }
                                else if (sender.Tamer.ItemCount(item) >= cost)
                                {
                                    // Descontando os requerimentos
                                    sender.Tamer.RemoveItem(item, cost);

                                    // Adicionando os itens
                                    sender.Tamer.AddItem("Evo Card Omegamon X", 1, false);
                                    sender.Tamer.AtualizarInventario();

                                    Send(sender, "Recebeu x1 [" + fera + "]!");
                                }
                                else
                                    Send(sender, "Requer x50 [" + item + "].");
                            }
                            else
                                Send(sender, "Algum erro ocorreu!");
                        }
                        else
                            Send(sender, "An error has occurred! Try again later.");

                        return true;
                    }

                    ///////////////////////////////////
                    ///COMANDOS GLOBAIS PARA GM e DEV//
                    ///////////////////////////////////

                    //Saving Item_codex
                    if (split[0] == "/saveitem" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 2)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                string nome = text.Replace("/saveitem ", "").Replace("-", "");
                                // Verificando se o item já está salvo
                                VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>(
                                "id", "item_codex", "WHERE item_idx=@item_id AND item_tag=@item_tag AND item_tab = 0"
                                + " AND name=@nome"
                                , new Database.QueryParameters() { {"item_id", sender.Tamer.Items[0].ItemId }
                                , {"item_tag", sender.Tamer.Items[0].ItemTag }, { "nome", nome } });

                                if (!result.HasRows)
                                {
                                    uint itemId = Emulator.Enviroment.Database.Insert<uint>("item_codex"
                                        , new Database.QueryParameters() {
                                            { "item_idx", sender.Tamer.Items[0].ItemId }
                                            ,  { "name", nome }
                                            ,  { "item_type1", sender.Tamer.Items[0].ItemType }
                                            ,  { "default_max_quantity", sender.Tamer.Items[0].ItemQuantMax }
                                            ,  { "required_level", sender.Tamer.Items[0].ItemtamerLvl }
                                            ,  { "effect_type_1", sender.Tamer.Items[0].ItemEffect1 }
                                            ,  { "effect_value_1", sender.Tamer.Items[0].ItemEffect1Value }
                                            ,  { "effect_type_2", sender.Tamer.Items[0].ItemEffect2 }
                                            ,  { "effect_value_2", sender.Tamer.Items[0].ItemEffect2Value }
                                            ,  { "effect_type_3", sender.Tamer.Items[0].ItemEffect3 }
                                            ,  { "effect_value_3", sender.Tamer.Items[0].ItemEffect3Value }
                                            ,  { "effect_type_4", sender.Tamer.Items[0].ItemEffect4 }
                                            ,  { "effect_value_4", sender.Tamer.Items[0].ItemEffect4Value }
                                            ,  { "item_tag", sender.Tamer.Items[0].ItemTag }
                                            ,  { "custo", sender.Tamer.Items[0].Custo }
                                        });

                                    sender.Tamer.SaveItem(0);

                                    if (itemId > 0)
                                    {
                                        Send(sender, "Item: " + nome + " successfully added! Do not forget "
                                            + "to check the TAG Item.");
                                    }
                                }
                                else
                                {
                                    Emulator.Enviroment.Database.Update("item_codex"
                                        , new Database.QueryParameters() {
                                            { "item_idx", sender.Tamer.Items[0].ItemId }
                                            //,  { "name", nome }
                                            ,  { "item_type1", sender.Tamer.Items[0].ItemType }
                                            ,  { "default_max_quantity", sender.Tamer.Items[0].ItemQuantMax }
                                            ,  { "required_level", sender.Tamer.Items[0].ItemtamerLvl }
                                            ,  { "effect_type_1", sender.Tamer.Items[0].ItemEffect1 }
                                            ,  { "effect_value_1", sender.Tamer.Items[0].ItemEffect1Value }
                                            ,  { "effect_type_2", sender.Tamer.Items[0].ItemEffect2 }
                                            ,  { "effect_value_2", sender.Tamer.Items[0].ItemEffect2Value }
                                            ,  { "effect_type_3", sender.Tamer.Items[0].ItemEffect3 }
                                            ,  { "effect_value_3", sender.Tamer.Items[0].ItemEffect3Value }
                                            ,  { "effect_type_4", sender.Tamer.Items[0].ItemEffect4 }
                                            ,  { "effect_value_4", sender.Tamer.Items[0].ItemEffect4Value }
                                            ,  { "item_tag", sender.Tamer.Items[0].ItemTag }
                                            ,  { "custo", sender.Tamer.Items[0].Custo }
                                        }, "WHERE item_idx=@item_id AND item_tag=@tag AND item_tab = 0"
                                    , new Database.QueryParameters() { {"item_id", sender.Tamer.Items[0].ItemId }
                                    , {"tag", sender.Tamer.Items[0].ItemTag } });

                                    sender.Tamer.SaveItem(0);

                                    Send(sender, "Updated item in table.");
                                }
                            }
                            else
                            {
                                Send(sender, "There is no item in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /saveitem <item name>");
                        }

                        return true;
                    }

                    // Salvando o item_codex de um Card
                    if (split[0] == "/savecard" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                string nome = text.Replace("/savecard ", "").Replace("-", "");
                                // Verificando se o item já está salvo
                                VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>(
                                "id", "item_codex", "WHERE item_idx=@item_id AND item_tag=@item_tag AND item_tab = 1"
                                + " AND name=@nome"
                                , new Database.QueryParameters() { {"item_id", sender.Tamer.Cards[0].ItemId }
                                , {"item_tag", sender.Tamer.Cards[0].ItemTag }, { "nome", nome } });

                                if (!result.HasRows)
                                {
                                    uint itemId = Emulator.Enviroment.Database.Insert<uint>("item_codex"
                                        , new Database.QueryParameters() {
                                            { "item_idx", sender.Tamer.Cards[0].ItemId }
                                            ,  { "name", nome }
                                            ,  { "item_type1", sender.Tamer.Cards[0].ItemType }
                                            ,  { "default_max_quantity", sender.Tamer.Cards[0].ItemQuantMax }
                                            ,  { "required_level", sender.Tamer.Cards[0].ItemtamerLvl }
                                            ,  { "effect_type_1", sender.Tamer.Cards[0].ItemEffect1 }
                                            ,  { "effect_value_1", sender.Tamer.Cards[0].ItemEffect1Value }
                                            ,  { "effect_type_2", sender.Tamer.Cards[0].ItemEffect2 }
                                            ,  { "effect_value_2", sender.Tamer.Cards[0].ItemEffect2Value }
                                            ,  { "effect_type_3", sender.Tamer.Cards[0].ItemEffect3 }
                                            ,  { "effect_value_3", sender.Tamer.Cards[0].ItemEffect3Value }
                                            ,  { "effect_type_4", sender.Tamer.Cards[0].ItemEffect4 }
                                            ,  { "effect_value_4", sender.Tamer.Cards[0].ItemEffect4Value }
                                            ,  { "item_tag", sender.Tamer.Cards[0].ItemTag }
                                            ,  { "item_use_on", sender.Tamer.Cards[0].ItemUseOn }
                                            ,  { "custo", sender.Tamer.Cards[0].Custo }
                                            ,  { "item_tab", 1 }
                                        });

                                    sender.Tamer.SaveCard(0);

                                    if (itemId > 0)
                                    {
                                        Send(sender, "Card: " + nome + " successfully added! Do not forget "
                                            + "to check the TAG Item.");
                                    }
                                }
                                else
                                {
                                    Emulator.Enviroment.Database.Update("item_codex"
                                        , new Database.QueryParameters() {
                                            { "item_idx", sender.Tamer.Cards[0].ItemId }
                                            //,  { "name", nome }
                                            ,  { "item_type1", sender.Tamer.Cards[0].ItemType }
                                            ,  { "default_max_quantity", sender.Tamer.Cards[0].ItemQuantMax }
                                            ,  { "required_level", sender.Tamer.Cards[0].ItemtamerLvl }
                                            ,  { "effect_type_1", sender.Tamer.Cards[0].ItemEffect1 }
                                            ,  { "effect_value_1", sender.Tamer.Cards[0].ItemEffect1Value }
                                            ,  { "effect_type_2", sender.Tamer.Cards[0].ItemEffect2 }
                                            ,  { "effect_value_2", sender.Tamer.Cards[0].ItemEffect2Value }
                                            ,  { "effect_type_3", sender.Tamer.Cards[0].ItemEffect3 }
                                            ,  { "effect_value_3", sender.Tamer.Cards[0].ItemEffect3Value }
                                            ,  { "effect_type_4", sender.Tamer.Cards[0].ItemEffect4 }
                                            ,  { "effect_value_4", sender.Tamer.Cards[0].ItemEffect4Value }
                                            ,  { "item_tag", sender.Tamer.Cards[0].ItemTag }
                                            ,  { "custo", sender.Tamer.Cards[0].Custo }
                                            ,  { "item_use_on", sender.Tamer.Cards[0].ItemUseOn }
                                            ,  { "item_tab", 1 }
                                        }, "WHERE item_idx=@item_id AND item_tag=@tag AND item_tab = 1"
                                    , new Database.QueryParameters() { {"item_id", sender.Tamer.Cards[0].ItemId }
                                    , {"tag", sender.Tamer.Cards[0].ItemTag } });

                                    sender.Tamer.SaveCard(0);

                                    Send(sender, "Card updated in the table.");
                                }
                            }
                            else
                            {
                                Send(sender, "There is no Card in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /savecard <card name>");
                        }

                        return true;
                    }

                    // Reading Item from file
                    else if (split[0] == "/loaditem" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            byte[] data = StringHex.Hex2Binary(PacketString.Read("item"));
                            InPacket packet = new InPacket(data);
                            int id = packet.ReadInt();
                            sender.Tamer.Items[0].ItemId = packet.ReadInt();
                            sender.Tamer.Items[0].ItemTag = packet.ReadByte();
                            sender.Tamer.Items[0].ItemType = packet.ReadByte();
                            short unk = packet.ReadShort();
                            sender.Tamer.Items[0].ItemQuant = packet.ReadInt();
                            sender.Tamer.Items[0].ItemQuantMax = packet.ReadInt();
                            sender.Tamer.Items[0].Custo = packet.ReadInt();
                            id = packet.ReadInt();
                            id = packet.ReadInt();
                            sender.Tamer.Items[0].ItemtamerLvl = packet.ReadInt();
                            id = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect1 = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect1Value = packet.ReadInt();
                            id = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect2 = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect2Value = packet.ReadInt();
                            id = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect3 = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect3Value = packet.ReadInt();
                            id = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect4 = packet.ReadInt();
                            sender.Tamer.Items[0].ItemEffect4Value = packet.ReadInt();
                            byte b; // Lendo o restante do pacote
                            while (packet.Remaining > 0) b = packet.ReadByte();
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Loading Item from file ... ID: "
                                + sender.Tamer.Items[0].ItemId);
                        }
                        else
                        {
                            Send(sender, "There is no Item in slot 1.");
                        }

                        return true;
                    }

                    // Reading Card from file
                    else if (split[0] == "/loadcard" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Cards[0] != null)
                        {
                            byte[] data = StringHex.Hex2Binary(PacketString.Read("card"));
                            InPacket packet = new InPacket(data);
                            int id = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemId = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemTag = packet.ReadByte();
                            sender.Tamer.Cards[0].ItemType = packet.ReadByte();
                            byte unk = packet.ReadByte();
                            sender.Tamer.Cards[0].ItemUseOn = packet.ReadByte();
                            sender.Tamer.Cards[0].ItemQuant = packet.ReadByte();
                            sender.Tamer.Cards[0].ItemQuantMax = packet.ReadByte();
                            short unk2 = packet.ReadShort();
                            sender.Tamer.Cards[0].Custo = packet.ReadInt();
                            id = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect1 = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect1Value = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect2 = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect2Value = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect3 = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect3Value = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect4 = packet.ReadInt();
                            sender.Tamer.Cards[0].ItemEffect4Value = packet.ReadInt();
                            byte b; // Lendo o restante do pacote
                            while (packet.Remaining > 0) b = packet.ReadByte();
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Loading card from file... ID: "
                                + sender.Tamer.Cards[0].ItemId);
                        }
                        else
                        {
                            Send(sender, "There is no Card in slot 1.");
                        }

                        return true;
                    }

                    // Next Item
                    else if (split[0] == "/nextitemOLD" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            sender.Tamer.Items[0].ItemId++;
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Next item... ID: " + sender.Tamer.Items[0].ItemId.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no item in slot 1.");
                        }

                        return true;
                    }

                    // Next Item
                    else if (split[0] == "/nextitem" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            sender.Tamer.Items[0].ItemId++;
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Next item... ID: " + sender.Tamer.Items[0].ItemId.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no item in slot 1.");
                        }

                        return true;
                    }

                    // EFFECT TYPE
                    else if (split[0] == "/et" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            sender.Tamer.Items[0].ItemEffect1++;
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Next EffectType ID: " + sender.Tamer.Items[0].ItemEffect1.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no item in slot 1.");
                        }

                        return true;
                    }

                    // Next Item
                    else if (split[0] == "/ni" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            for (var i = 0; i < 24; i++)
                            {
                                sender.Tamer.Items[i].ItemId += i + 1;
                                sender.Tamer.Items[i].ItemTag = (byte)(i + 1);
                                Send(sender, "i " + i);
                            }
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Partindo Do ID {0} " + sender.Tamer.Items[0].ItemId.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no item in slot 1.");
                        }

                        return true;
                    }

                    // Next Item
                    else if (split[0] == "/niall" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            for (int i = 0; i < 24; i++)
                            {
                                sender.Tamer.Items[i].ItemId++;
                                sender.Tamer.Items[i].ItemTag = (byte)(i + 1);
                                Send(sender, "i " + i);
                            }
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Partindo Do ID " + sender.Tamer.Items[0].ItemId.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no item in slot 1.");
                        }

                        return true;
                    }

                    // Próximo Card
                    else if (split[0] == "/nextcard" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Cards[0] != null)
                        {
                            sender.Tamer.Cards[0].ItemId++;
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Próximo card... ID: " + sender.Tamer.Cards[0].ItemId.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no card in slot 1.");
                        }

                        return true;
                    }

                    // Próximo Item Tag
                    else if (split[0] == "/nextitemtag" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Items[0] != null)
                        {
                            sender.Tamer.Items[0].ItemTag++;
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Next item Tag... TAG: " + sender.Tamer.Items[0].ItemTag.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no item in slot 1.");
                        }

                        return true;
                    }
                    // Next Card Tag
                    else if (split[0] == "/nextcardtag" && sender.User.Autoridade >= 100)
                    {
                        if (sender.Tamer.Cards[0] != null)
                        {
                            sender.Tamer.Cards[0].ItemTag++;
                            sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            Send(sender, "Next Card Tag... TAG: " + sender.Tamer.Cards[0].ItemTag.ToString());
                        }
                        else
                        {
                            Send(sender, "There is no card in slot 1.");
                        }

                        return true;
                    }

                    // Setting the item ID
                    else if (split[0] == "/itemid" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                sender.Tamer.Items[0].ItemId = int.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Item ID changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no item in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /itemid <ID>");
                        }

                        return true;
                    }
                    // Setting the Card ID
                    else if (split[0] == "/cardid" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                sender.Tamer.Cards[0].ItemId = int.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Card ID changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no card in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /cardid <ID>");
                        }

                        return true;
                    }

                    // Setando Use on do Card
                    else if (split[0] == "/carduse" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                sender.Tamer.Cards[0].ItemUseOn = byte.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Card OnUse changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no card in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /carduse <Use>");
                        }

                        return true;
                    }

                    // Setting the item tag
                    else if (split[0] == "/itemtag" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                sender.Tamer.Items[0].ItemTag = byte.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Item TAG changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no item in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /itemtag <TAG>");
                        }

                        return true;
                    }
                    // Setting the Card TAG
                    else if (split[0] == "/cardtag" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                sender.Tamer.Cards[0].ItemTag = byte.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Card TAG changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no card in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /cardtag <TAG>");
                        }

                        return true;
                    }

                    // Setting the item cost
                    else if (split[0] == "/itemcost" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                sender.Tamer.Items[0].Custo = int.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Item cost changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no card in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /itemcost <Custo>");
                        }

                        return true;
                    }
                    else if (split[0] == "/cardcost" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                sender.Tamer.Cards[0].Custo = int.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Card cost changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no card in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /cardcost <Custo>");
                        }

                        return true;
                    }

                    // Setting the item's Type
                    else if (split[0] == "/itemtype" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                sender.Tamer.Items[0].ItemType = byte.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Type of item changed to: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "There is no card in slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Invalid Command. Type: /itemtype <Type>");
                        }

                        return true;
                    }
                    //Setting the Card Type
                    else if (split[0] == "/cardtype" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                sender.Tamer.Cards[0].ItemType = byte.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Type do card alterada para: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "Não há nenhum card no slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Comando invalido. Digite: /cardtype <Type>");
                        }

                        return true;
                    }

                    // Setando quantidade do item
                    else if (split[0] == "/itemquant" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                sender.Tamer.Items[0].ItemQuantMax = int.Parse(split[1]);
                                //sender.Tamer.Items[0].ItemQuant = int.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Quantidade Maxima do item alterada para: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "Não há nenhum item no slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Comando invalido. Digite: /itemquant <Quantidade Maxima>");
                        }

                        return true;
                    }
                    // Setando quantidade do Card
                    else if (split[0] == "/cardquant" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                sender.Tamer.Cards[0].ItemQuantMax = byte.Parse(split[1]);
                                sender.Tamer.Cards[0].ItemQuant = byte.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Quantidade Maxima do card alterada para: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "Não há nenhum card no slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Comando invalido. Digite: /cardquant <Quantidade Maxima>");
                        }

                        return true;
                    }

                    // Setando Tamer Level
                    else if (split[0] == "/itemlvl" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                sender.Tamer.Items[0].ItemtamerLvl = int.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Tamer Level required do item alterado para: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "Não há nenhum item no slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Comando invalido. Digite: /itemlvl <tamer lavel required>");
                        }

                        return true;
                    }

                    else if (split[0] == "/cardlvl" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                sender.Tamer.Cards[0].ItemtamerLvl = int.Parse(split[1]);
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                Send(sender, "Tamer Level required do card alterado para: " + split[1]);
                            }
                            else
                            {
                                Send(sender, "Não há nenhum card no slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Comando invalido. Digite: /cardlvl <tamer lavel required>");
                        }

                        return true;
                    }

                    // Setando effects e value effects do item
                    else if (split[0] == "/itemeff" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 4)
                        {
                            if (sender.Tamer.Items[0] != null)
                            {
                                int posicao = int.Parse(split[1]);
                                if (posicao >= 1 && posicao <= 4)
                                {
                                    switch (posicao)
                                    {
                                        case 1:
                                            sender.Tamer.Items[0].ItemEffect1 = int.Parse(split[2]);
                                            sender.Tamer.Items[0].ItemEffect1Value = int.Parse(split[3]);
                                            break;
                                        case 2:
                                            sender.Tamer.Items[0].ItemEffect2 = int.Parse(split[2]);
                                            sender.Tamer.Items[0].ItemEffect2Value = int.Parse(split[3]);
                                            break;
                                        case 3:
                                            sender.Tamer.Items[0].ItemEffect3 = int.Parse(split[2]);
                                            sender.Tamer.Items[0].ItemEffect3Value = int.Parse(split[3]);
                                            break;
                                        case 4:
                                            sender.Tamer.Items[0].ItemEffect4 = int.Parse(split[2]);
                                            sender.Tamer.Items[0].ItemEffect4Value = int.Parse(split[3]);
                                            break;
                                    }
                                    sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                    Send(sender, "Effect" + split[1] + " do item alterado para: "
                                        + split[2] + " e Effect" + split[1] + "Value alterado para: "
                                        + split[3]);
                                }
                                else
                                {
                                    Send(sender, "A posicao do efeito precisa ser entre 1 e 4.");
                                }
                            }
                            else
                            {
                                Send(sender, "Não há nenhum item no slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Comando invalido. Digite: /itemeff <posicao> <Effect ID> <Effect Value>");
                        }

                        return true;
                    }
                    // Setanto Effects e Value Effects do Card
                    else if (split[0] == "/cardeff" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 4)
                        {
                            if (sender.Tamer.Cards[0] != null)
                            {
                                int posicao = int.Parse(split[1]);
                                if (posicao >= 1 && posicao <= 4)
                                {
                                    switch (posicao)
                                    {
                                        case 1:
                                            sender.Tamer.Cards[0].ItemEffect1 = int.Parse(split[2]);
                                            sender.Tamer.Cards[0].ItemEffect1Value = int.Parse(split[3]);
                                            break;
                                        case 2:
                                            sender.Tamer.Cards[0].ItemEffect2 = int.Parse(split[2]);
                                            sender.Tamer.Cards[0].ItemEffect2Value = int.Parse(split[3]);
                                            break;
                                        case 3:
                                            sender.Tamer.Cards[0].ItemEffect3 = int.Parse(split[2]);
                                            sender.Tamer.Cards[0].ItemEffect3Value = int.Parse(split[3]);
                                            break;
                                        case 4:
                                            sender.Tamer.Cards[0].ItemEffect4 = int.Parse(split[2]);
                                            sender.Tamer.Cards[0].ItemEffect4Value = int.Parse(split[3]);
                                            break;
                                    }
                                    sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                                    Send(sender, "Effect" + split[1] + " do card alterado para: "
                                        + split[2] + " e Effect" + split[1] + "Value alterado para: "
                                        + split[3]);
                                }
                                else
                                {
                                    Send(sender, "A posicao do efeito precisa ser entre 1 e 4.");
                                }
                            }
                            else
                            {
                                Send(sender, "Não há nenhum card no slot 1.");
                            }
                        }
                        else
                        {
                            Send(sender, "Comando invalido. Digite: /cardeff <posicao> <Effect ID> <Effect Value>");
                        }

                        return true;
                    }

                    // Criando item para si mesmo
                    else if (split[0] == "/createitem" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 3)
                        {
                            int quant = int.Parse(split[1]);
                            string name = text.Replace("/createitem ", "").Replace(split[1] + " ", "");
                            if (Emulator.Enviroment.Codex.ContainsKey(name))
                            {
                                ItemCodex item = Emulator.Enviroment.Codex[name];
                                sender.Tamer.AddItem(name, quant, false);
                                Send(sender, quant.ToString() + " " + name + " adicionados.");
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            }
                            else
                                Send(sender, "Este item nao existe. Verifique o nome digitado.");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /createitem <quant> <item>");

                        return true;
                    }
                    // Criando item para si mesmo
                    else if (split[0] == "/createitemware" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 3)
                        {
                            int quant = int.Parse(split[1]);
                            string name = text.Replace("/createitemware ", "").Replace(split[1] + " ", "");
                            if (Emulator.Enviroment.Codex.ContainsKey(name))
                            {
                                Send(sender, quant.ToString() + " " + name + " adicionados.");
                                sender.Tamer.AddItemWarehouse(name, quant);
                            }
                            else
                                Send(sender, "Este item nao existe. Verifique o nome digitado.");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /createitemware <quant> <item>");

                        return true;
                    }
                    // Criando card para si mesmo
                    else if (split[0] == "/createcard" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 3)
                        {
                            int quant = int.Parse(split[1]);
                            string name = text.Replace("/createcard ", "").Replace(split[1] + " ", "");
                            ItemCodex item = Emulator.Enviroment.Codex[name];
                            if (item != null)
                            {
                                sender.Tamer.AddCard(name, quant, false);
                                Send(sender, quant.ToString() + " " + name + " adicionados.");
                                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
                            }
                            else
                                Send(sender, "Este card nao existe. Verifique o nome digitado.");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /createcard <quant> <card>");

                        return true;
                    }
                    else if (split[0] == "/createcardware" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 3)
                        {
                            int quant = int.Parse(split[1]);
                            string name = text.Replace("/createcardware ", "").Replace(split[1] + " ", "");
                            ItemCodex item = Emulator.Enviroment.Codex[name];
                            if (item != null)
                            {
                                sender.Tamer.AddCardWarehouse(name, quant);
                                Send(sender, quant.ToString() + " " + name + " adicionados.");
                            }
                            else
                                Send(sender, "Este card nao existe. Verifique o nome digitado.");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /createcardware <quant> <card>");

                        return true;
                    }

                    // Teleport
                    if ((split[0] == "/tp" || split[0] == "/map")
                        && sender.User.Autoridade >= 10)
                    {
                        if (split.Length == 4)
                        {
                            int mapid = int.Parse(split[1]);
                            short x = short.Parse(split[2]);
                            short y = short.Parse(split[3]);
                            sender.Tamer.Teleport(mapid, x, y);
                        }
                        else if (split.Length == 2)
                        {
                            int mapid = int.Parse(split[1]);
                            sender.Tamer.Teleport(mapid);
                        }
                        else if (split.Length == 3)
                        {
                            short x = short.Parse(split[1]);
                            short y = short.Parse(split[2]);
                            sender.Tamer.Teleport(x, y);
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /tp(ou /map) <mapID> e/ou <x> <y>");

                        return true;
                    }

                    // Teleport para a posição de um Tamer
                    if ((split[0] == "/gototamer")
                        && sender.User.Autoridade >= 10)
                    {
                        if (split.Length == 2)
                        {
                            string nick = split[1];
                            foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                            {
                                if (c.Tamer != null && c.Tamer.Name == nick)
                                {
                                    sender.Tamer.Teleport(c.Tamer.MapId, (short)c.Tamer.Location.X
                                        , (short)c.Tamer.Location.Y);

                                    return true;
                                }
                            }
                            Send(sender, string.Format("Tamer nao encontrado: {0}", nick));
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /gototamer <nick>");

                        return true;
                    }

                    // TEMPORÁRIO: TP para Mud Village e Real World
                    if ((split[0] == "/mud"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 20)
                            sender.Tamer.Teleport(20, 50, 69);
                        else
                            Send(sender, "Voce ja esta em Mud Village.");

                        return true;
                    }
                    if ((split[0] == "/geko"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 40)
                            sender.Tamer.Teleport(40, 88, 161);
                        else
                            Send(sender, "Voce ja esta em Geko Swamp.");

                        return true;
                    }
                    if ((split[0] == "/unstuck"))
                    {
                        if (sender.Tamer.MapId == 60)
                        {
                            sender.Tamer.Teleport(60, 91, 102);
                        }
                        Send(sender, "Relogue.");
                        return true;
                    }
                    if ((split[0] == "/timev"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 60)
                            sender.Tamer.Teleport(60, 86, 119);
                        else
                            Send(sender, "Voce ja esta em Time Village.");

                        return true;
                    }
                    if ((split[0] == "/baba"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 70)
                        {
                            if (sender.Tamer.Level >= 41)
                            {
                                sender.Tamer.Teleport(70, 75, 128);
                            }
                            else
                            {
                                Send(sender, "Requer Level 41.");
                            }
                        }

                        else
                            Send(sender, "Voce ja esta em Baba Land.");

                        return true;
                    }
                    if ((split[0] == "/sky3"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 77)
                            sender.Tamer.Teleport(77, 67, 174);
                        else
                            Send(sender, "Voce ja esta em Sky3F.");

                        return true;
                    }
                    if ((split[0] == "/rb"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 80)
                            sender.Tamer.Teleport(80, 100, 166);
                        else
                            Send(sender, "Voce ja esta em Sky3F.");

                        return true;
                    }
                    if ((split[0] == "/leaftown"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 88)
                            sender.Tamer.Teleport(88, 48, 71);
                        else
                            Send(sender, "Voce ja esta em Sky3F.");

                        return true;
                    }
                    if ((split[0] == "/undergroundevent"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 90)
                            sender.Tamer.Teleport(90, 14, 22);
                        else
                            Send(sender, "Voce ja esta em Sky3F.");

                        return true;
                    }
                    if ((split[0] == "/amusement"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 5)
                            sender.Tamer.Teleport(5, 67, 98);
                        else
                            Send(sender, "Voce ja esta em Amusement Park.");

                        return true;
                    }
                    if ((split[0] == "/real"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 1)
                            sender.Tamer.Teleport(1, 58, 82);
                        else
                            Send(sender, "Voce ja esta no Real World.");

                        return true;
                    }
                    if ((split[0] == "/toy"))
                    {
                        if (sender.Tamer.MapId == 79 && sender.User.Autoridade == 100)
                        {
                            Send(sender, "Voce precisa completar o tutorial antes.");
                            return true;
                        }

                        if (sender.Tamer.MapId != 79)
                            sender.Tamer.Teleport(79, 59, 55);
                        else
                            Send(sender, "Voce ja esta em Toy City.");

                        return true;
                    }

                    // Criando um Teleportador
                    if ((split[0] == "/createtp")
                        && sender.User.Autoridade >= 100)
                    {
                        // Primeiro, verificamos se já existe um TP aqui
                        VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                        , "teleports", "WHERE mapid=@mapid AND posx=@posx AND posy=@posy LIMIT 1"
                        , new QueryParameters() { { "mapid", sender.Tamer.MapId }
                        , { "posx", sender.Tamer.Location.X }, { "posy", sender.Tamer.Location.Y } });
                        if (!result.HasRows)
                        {
                            int Id = Emulator.Enviroment.Database.Insert<int>("teleports"
                            , new QueryParameters() { { "mapid", sender.Tamer.MapId }
                            , { "posx", sender.Tamer.Location.X }, { "posy", sender.Tamer.Location.Y }});

                            Send(sender, "Teleport criado com sucesso! ID: " + Id.ToString()
                                + ". Lembre-se: guarde o ID, e cadastre o destino deste teleport.");

                            MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                            if (map != null)
                                map.CarregarTeleports();
                        }
                        else
                            Send(sender, "Ja existe um Teleport aqui! ID: " + result.Id.ToString());

                        return true;
                    }
                    // Setando o alvo do Teleport
                    if ((split[0] == "/tpalvo")
                        && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            int mapid = int.Parse(split[1]);

                            // Primeiro, verificamos se este Teleport existe
                            VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                            , "teleports", "WHERE id=@id LIMIT 1"
                            , new QueryParameters() { { "id", mapid } });
                            if (result.HasRows)
                            {
                                Emulator.Enviroment.Database.Update("teleports", new QueryParameters() {
                                    { "alvo", sender.Tamer.MapId }
                                    , { "alvox", sender.Tamer.Location.X }, { "alvoy", sender.Tamer.Location.Y } }
                                    , "WHERE id=@id", new QueryParameters() { { "id", mapid } });

                                Send(sender, "Alvo do teleport definido com sucesso!");

                                MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                                if (map != null)
                                    map.CarregarTeleports();
                            }
                            else
                                Send(sender, "Nao existe um Teleport com este ID!");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /tpalvo <Teleport ID>");

                        return true;
                    }
                    // Setando o Level do Teleport
                    if ((split[0] == "/tplvl")
                        && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 3)
                        {
                            int mapid = int.Parse(split[1]);
                            int lvl = int.Parse(split[2]);

                            // Primeiro, verificamos se este Teleport existe
                            VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                            , "teleports", "WHERE id=@id LIMIT 1"
                            , new QueryParameters() { { "id", mapid } });
                            if (result.HasRows)
                            {
                                Emulator.Enviroment.Database.Update("teleports", new QueryParameters() {
                                    { "lvl", lvl }}
                                    , "WHERE id=@id", new QueryParameters() { { "id", mapid } });

                                Send(sender, "Level necessario do teleport: " + lvl);

                                MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                                if (map != null)
                                    map.CarregarTeleports();
                            }
                            else
                                Send(sender, "Nao existe um Teleport com este ID!");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /tplvl <Teleport ID> <Level>");

                        return true;
                    }
                    // Setando o Rank do Teleport
                    if ((split[0] == "/tprank")
                        && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 3)
                        {
                            int mapid = int.Parse(split[1]);
                            int lvl = int.Parse(split[2]);

                            // Primeiro, verificamos se este Teleport existe
                            VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                            , "teleports", "WHERE id=@id LIMIT 1"
                            , new QueryParameters() { { "id", mapid } });
                            if (result.HasRows)
                            {
                                Emulator.Enviroment.Database.Update("teleports", new QueryParameters() {
                                    { "rank", lvl }}
                                    , "WHERE id=@id", new QueryParameters() { { "id", mapid } });

                                Send(sender, "Rank necessario do teleport: " + lvl);

                                MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                                if (map != null)
                                    map.CarregarTeleports();
                            }
                            else
                                Send(sender, "Nao existe um Teleport com este ID!");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /tprank <Teleport ID> <Rank>");

                        return true;
                    }

                    // Criando Spawns
                    if ((split[0] == "/spawn")
                        && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 2)
                        {
                            string[] arg = text.Replace("/spawn ", "").Split('.');
                            if (arg.Length == 7)
                            {
                                string digimon = arg[0];
                                int lvl = int.Parse(arg[1]);
                                int rank = int.Parse(arg[2]);
                                int move = int.Parse(arg[3]);
                                int quant = int.Parse(arg[4]);
                                int quantMin = int.Parse(arg[5]);
                                string drop = arg[6].Replace("(", "").Replace(")", "");

                                // Primeiro, verificamos se este Digimon existe
                                VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                                , "digimon", "WHERE nome=@nome LIMIT 1"
                                , new QueryParameters() { { "nome", digimon } });
                                if (result.HasRows)
                                {
                                    int Id = Emulator.Enviroment.Database.Insert<int>("spawn_digimons"
                                    , new QueryParameters() { { "digimon_id", result.Id }
                                    , { "name", digimon }, { "map_id", sender.Tamer.MapId }
                                    , { "x", sender.Tamer.Location.X }, { "y", sender.Tamer.Location.Y }
                                    , { "lvl", lvl }, { "rank", rank }, { "move", move }
                                    , { "quant", quant }, { "mquant", quantMin }, { "item_drop", drop } });

                                    Send(sender, "Spawn criado com sucesso! ID: " + Id.ToString());

                                    MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                                    if (map != null)
                                    {
                                        map.Spawn();
                                        map.sendSpawn(sender);
                                    }
                                }
                                else
                                    Send(sender, "Este digimon nao existe na base de dados: " + digimon + ".");
                            }
                            else
                                Send(sender, "Comando invalido. Digite: /spawn <Digimon>.<lvl>.<rank>.<move?>."
                                    + "<quantMAX>.<quantMIN>.(<item1>-<quant1>-<chance1>/<item2>-<quant2>-<chance2>/.../"
                                    + "<itemN>-<quantN>-<chanceN>)");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /spawn <Digimon>.<lvl>.<rank>.<move?>."
                                + "<quantMAX>.<quantMIN>.(<item1>-<quant1>-<chance1>/<item2>-<quant2>-<chance2>/.../"
                                + "<itemN>-<quantN>-<chanceN>)");

                        return true;
                    }

                    // Criando ajudantes para um Spawn
                    if ((split[0] == "/ajudante")
                        && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 2)
                        {
                            string[] arg = text.Replace("/ajudante ", "").Split('.');
                            if (arg.Length == 4)
                            {
                                int LiderId = int.Parse(arg[0]);
                                string digimon = arg[1];
                                int lvl = int.Parse(arg[2]);
                                int rank = int.Parse(arg[3]);

                                // Primeiro, verificamos se este Digimon existe
                                VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                                , "spawn_digimons", "WHERE id=@id LIMIT 1"
                                , new QueryParameters() { { "id", LiderId } });
                                if (result.HasRows)
                                {
                                    // Depois, verificamos se este Digimon existe
                                    result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                                    , "digimon", "WHERE nome=@nome LIMIT 1"
                                    , new QueryParameters() { { "nome", digimon } });
                                    if (result.HasRows)
                                    {
                                        int Id = Emulator.Enviroment.Database.Insert<int>("spawn_digimons"
                                        , new QueryParameters() { { "digimon_id", result.Id }
                                    , { "name", digimon }, { "map_id", 0 }, { "lider", LiderId }
                                    , { "x", 0 }, { "y", 0 }
                                    , { "lvl", lvl }, { "rank", rank }, { "move", 0 }
                                    , { "quant", 1 }, { "mquant", 1 }, { "item_drop", "vazio" } });

                                        Send(sender, "Ajudante criado com sucesso! ID: " + Id.ToString());

                                        MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                                        if (map != null)
                                        {
                                            map.Spawn();
                                            map.sendSpawn(sender);
                                        }
                                    }
                                    else
                                        Send(sender, "Este digimon nao existe na base de dados: " + digimon + ".");
                                }
                                else
                                    Send(sender, "Este spawn nao existe na base de dados: " + LiderId + ".");
                            }
                            else
                                Send(sender, "Comando invalido. Digite: /ajudante <LiderID>.<Digimon>.<lvl>.<rank>");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /ajudante <LiderID>.<Digimon>.<lvl>.<rank>");

                        return true;
                    }

                    // Deletando um Spawn
                    if (split[0] == "/despawn" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            int id = int.Parse(split[1]);
                            MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                            foreach (Spawn s in map.spawn)
                            {
                                if (s.Id == id)
                                {
                                    map.spawn.Remove(s);
                                    break;
                                }
                            }

                            Emulator.Enviroment.Database.Update("spawn_digimons", new QueryParameters()
                            { { "is_deleted", true } }, "WHERE id=@id", new QueryParameters() {
                            { "id", id } }, "dtexclusao=CURRENT_TIME()");

                            Send(sender, "Spawn deletado. E necessario reiniciar o server, ou esvaziar o mapa.");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /despawn <ID>");

                        return true;
                    }

                    // Setando o tempo de respawn
                    if (split[0] == "/spawntime" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 3)
                        {
                            int id = int.Parse(split[1]);
                            int tempo = int.Parse(split[2]);
                            MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                            foreach (Spawn s in map.spawn)
                            {
                                if (s.Id == id)
                                {
                                    s.Tempo = tempo;
                                    break;
                                }
                            }

                            Emulator.Enviroment.Database.Update("spawn_digimons", new QueryParameters()
                            { { "tempo", tempo } }, "WHERE id=@id", new QueryParameters() {
                            { "id", id } });

                            Send(sender, string.Format("Agora o spawn {0} leva {1} minutos para respawnar"
                                , id, tempo));
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /spawntime <ID> <tempo(minutos)>");

                        return true;
                    }

                    // Setando drop específico de um spawn
                    else if (split[0] == "/espdrop" && sender.User.Autoridade >= 100)
                    {
                        bool ok = true;
                        if (split.Length >= 2)
                        {
                            string[] arg = text.Replace("/espdrop ", "").Split('.');

                            if (arg.Length == 4)
                            {
                                // Verificando se o item existe no codex
                                if (!Emulator.Enviroment.Codex.ContainsKey(arg[1]))
                                {
                                    ok = false;
                                    Send(sender, "Item nao existe no codex: " + arg[1] + ".");
                                }

                                // O item existe na tabela, podemos prosseguir
                                if (ok)
                                {
                                    // Obtendo o valor já existente
                                    StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>
                                        ("item_drop as value", "spawn_digimons", "WHERE name=@name LIMIT 1"
                                        , new QueryParameters() { { "name", arg[0] } });

                                    if (result.HasRows)
                                    {
                                        string dropOriginal = result.value;
                                        // O drop específico fica na primeira prosição da lista
                                        string newDrop = arg[1] + "-" + arg[2] + "-" + arg[3] + "/" + dropOriginal;

                                        // Atualizando a tabela
                                        Emulator.Enviroment.Database.Update("spawn_digimons",
                                            new QueryParameters() { { "item_drop", newDrop } }
                                            , "WHERE name=@name", new QueryParameters() { { "name", arg[0] } });

                                        Send(sender, "Adicionado drop especifico " + arg[1] + " para "
                                            + arg[0] + ".");
                                    }
                                    else
                                        Send(sender, "Digimon " + arg[0] + " nao encontrado na tabela de spawns.");
                                }
                            }
                            else
                                Send(sender, "Comando invalido. Digite: /espdrop <Digimon>.<item>.<quantidade>"
                                    + ".<chance>");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /espdrop <Digimon>.<item>.<quantidade>"
                                + ".<chance>");


                        return true;
                    }

                    // Adicionando drop a um spawn
                    else if (split[0] == "/adddrop" && sender.User.Autoridade >= 100)
                    {
                        bool ok = true;
                        if (split.Length >= 2)
                        {
                            string[] arg = text.Replace("/adddrop ", "").Split('.');

                            if (arg.Length == 4)
                            {
                                // Verificando se o item existe no codex
                                if (!Emulator.Enviroment.Codex.ContainsKey(arg[1]))
                                {
                                    ok = false;
                                    Send(sender, "Item nao existe no codex: " + arg[1] + ".");
                                }

                                // O item existe na tabela, podemos prosseguir
                                if (ok)
                                {
                                    // Obtendo o valor já existente
                                    StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>
                                        ("item_drop as value", "spawn_digimons", "WHERE name=@name LIMIT 1"
                                        , new QueryParameters() { { "name", arg[0] } });

                                    if (result.HasRows)
                                    {
                                        string dropOriginal = result.value;
                                        // Estamos só adicionando um drop novo. Este vai para o final da lista
                                        string newDrop = dropOriginal + arg[1] + "-" + arg[2] + "-" + arg[3] + "/";

                                        // Atualizando a tabela
                                        Emulator.Enviroment.Database.Update("spawn_digimons",
                                            new QueryParameters() { { "item_drop", newDrop } }
                                            , "WHERE name=@name", new QueryParameters() { { "name", arg[0] } });

                                        Send(sender, "Adicionado " + arg[1] + " para " + arg[0] + ".");
                                    }
                                    else
                                        Send(sender, "Digimon " + arg[0] + " nao encontrado na tabela de spawns.");
                                }
                            }
                            else
                                Send(sender, "Comando invalido. Digite: /adddrop <Digimon>.<item>.<quantidade>"
                                    + ".<chance>");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /adddrop <Digimon>.<item>.<quantidade>"
                                + ".<chance>");


                        return true;
                    }

                    // Setando drops de spawn a partir de arquivo
                    else if (split[0] == "/spawndrop" && sender.User.Autoridade >= 100)
                    {
                        // Abrindo o arquivo
                        string textFile = Directory.GetCurrentDirectory() + "/spawndrop.txt";
                        string[] lines = File.ReadAllLines(textFile);

                        // Percorrendo as linhas
                        foreach (string line in lines)
                            if (!line.Contains("#") && line.Length > 2) // Ignorando comentários e linhas vazias
                            {
                                string[] info = line.Split(' ');
                                string nome = info[0]; // Nome do Digimon
                                Regex regex = new Regex(Regex.Escape(nome + " "));
                                string drops = regex.Replace(line, "", 1); // Itens a serem inseridos

                                // Verificando se o Digimon existe na base
                                VerificationResult r = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                                    , "spawn_digimons", "WHERE name=@nome", new QueryParameters() { { "nome", nome } });
                                if (r.HasRows)
                                {
                                    // Verificando se todos os itens da linha existem no codex
                                    bool ok = true;
                                    info = drops.Split('/');
                                    foreach (string i in info)
                                    {
                                        string[] item = i.Split('-');
                                        if (item.Length == 3)
                                        {
                                            if (!Emulator.Enviroment.Codex.ContainsKey(item[0]))
                                            {
                                                ok = false;
                                                Send(sender, "Item nao existe no codex: " + item[0] + " Digimon: " + nome);
                                            }
                                        }
                                        else
                                        {
                                            Send(sender, "Itens mal formados para o " + nome);
                                            ok = false;
                                        }
                                    }

                                    // Está tudo certo, vamos salvar as informações
                                    if (ok)
                                        Emulator.Enviroment.Database.Update("spawn_digimons", new QueryParameters()
                                            { { "item_drop", drops } }, "WHERE name=@nome AND id > 3"
                                            , new QueryParameters() { { "nome", nome } });
                                }
                                else
                                    Send(sender, nome + " nao existe na base de spawn.");
                            }
                        Send(sender, "Spawn drop finish.");

                        return true;
                    }

                    // Mudando nome do Digimon
                    else if (split[0] == "/rename" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            sender.Tamer.Digimon[0].Name = (split[1]);
                            sender.Connection.Send(new Network.Packets.PACKET_DIGIMON_INDIVIDUAL(sender.Tamer.Digimon[0]));
                            Digimon d = sender.Tamer.Digimon[0];
                            d.ChangeName(split[1]);
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /rename <nome do digimon>");

                        return true;
                    }

                    // Mudano modelo do Digimon
                    else if (split[0] == "/digimonmodel" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            sender.Tamer.Digimon[0].Model = ushort.Parse(split[1]);
                            sender.Connection.Send(new Network.Packets.PACKET_DIGIMON_INDIVIDUAL(sender.Tamer.Digimon[0]));
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /digimonmodel <Modelo>");

                        return true;
                    }

                    // Mudando level do Digimon
                    else if (split[0] == "/digimonlvlOLD" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            sender.Tamer.Digimon[0].Level = ushort.Parse(split[1]);
                            sender.Connection.Send(new Network.Packets.PACKET_DIGIMON_INDIVIDUAL(sender.Tamer.Digimon[0]));
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /digimonlvl <level>");

                        return true;
                    }

                    // Mudando level do Digimon
                    else if (split[0] == "/digimonlvl" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 2)
                        {
                            sender.Tamer.Digimon[0].Level = ushort.Parse(split[1]);
                            sender.Connection.Send(new Network.Packets.PACKET_DIGIMON_INDIVIDUAL(sender.Tamer.Digimon[0]));
                            Digimon d = sender.Tamer.Digimon[0];
                            d.Level = ushort.Parse(split[1]);
                            d.SaveLvl();

                        }
                        else
                            Send(sender, "Comando invalido. Digite: /digimonlvl <level>");

                        return true;
                    }

                    // Salvando Digimon no banco
                    else if ((split[0] == "/savedigimon" || split[0] == "/save") && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 4)
                        {
                            string name = split[1];
                            int f1vp = int.Parse(split[2]);
                            int f2vp = int.Parse(split[3]);

                            // Chegando se o Digimon existe no banco
                            VerificationResult r = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                                , "digimon", "WHERE nome=@nome", new QueryParameters() { { "nome", name } });
                            if (r.HasRows)
                            {
                                // Skills
                                // F2
                                r = Emulator.Enviroment.Database.Select<VerificationResult>("skill_id as id"
                                    , "digimon_skills", "WHERE id=@id", new QueryParameters() { { "id"
                                , sender.Tamer.Digimon[0].skill1.Id } });
                                // Se não existe, insere
                                if (!r.HasRows)
                                    Emulator.Enviroment.Database.Insert<int>("digimon_skills", new QueryParameters() {
                                    { "skill_id", sender.Tamer.Digimon[0].skill1.Id }
                                    , { "name", name + " F2" }
                                    , { "required_level", sender.Tamer.Digimon[0].skill1.Lvl }
                                    , { "units", sender.Tamer.Digimon[0].skill1.Units }
                                    , { "range_type", sender.Tamer.Digimon[0].skill1.Range }
                                    , { "power", sender.Tamer.Digimon[0].skill1.Poder }
                                    , { "vp", f1vp } });
                                else
                                    Emulator.Enviroment.Database.Update("digimon_skills", new QueryParameters() {
                                    { "name", name + " F2" }
                                    , { "required_level", sender.Tamer.Digimon[0].skill1.Lvl }
                                    , { "units", sender.Tamer.Digimon[0].skill1.Units }
                                    , { "range_type", sender.Tamer.Digimon[0].skill1.Range }
                                    , { "power", sender.Tamer.Digimon[0].skill1.Poder }
                                    , { "vp", f1vp } }, "WHERE skill_id=@id"
                                        , new QueryParameters() { { "id"
                                    , sender.Tamer.Digimon[0].skill1.Id } });

                                // F3
                                r = Emulator.Enviroment.Database.Select<VerificationResult>("skill_id as id"
                                    , "digimon_skills", "WHERE id=@id", new QueryParameters() { { "id"
                                , sender.Tamer.Digimon[0].skill2.Id } });
                                // Se não existe, insere
                                if (!r.HasRows)
                                    Emulator.Enviroment.Database.Insert<int>("digimon_skills", new QueryParameters() {
                                    { "skill_id", sender.Tamer.Digimon[0].skill2.Id }
                                    , { "name", name + " F3" }
                                    , { "required_level", sender.Tamer.Digimon[0].skill2.Lvl }
                                    , { "units", sender.Tamer.Digimon[0].skill2.Units }
                                    , { "range_type", sender.Tamer.Digimon[0].skill2.Range }
                                    , { "power", sender.Tamer.Digimon[0].skill2.Poder }
                                    , { "vp", f2vp } });
                                else
                                    Emulator.Enviroment.Database.Update("digimon_skills", new QueryParameters() {
                                     { "name", name + " F3" }
                                    , { "required_level", sender.Tamer.Digimon[0].skill2.Lvl }
                                    , { "units", sender.Tamer.Digimon[0].skill2.Units }
                                    , { "range_type", sender.Tamer.Digimon[0].skill2.Range }
                                    , { "power", sender.Tamer.Digimon[0].skill2.Poder }
                                    , { "vp", f2vp } }, "WHERE skill_id=@id"
                                        , new QueryParameters() { { "id"
                                    , sender.Tamer.Digimon[0].skill2.Id } });

                                // Salvando o Digimon
                                Emulator.Enviroment.Database.Update("digimon", new QueryParameters() {
                                 { "estage", sender.Tamer.Digimon[0].estage }
                                , { "tipo", sender.Tamer.Digimon[0].type }
                                , { "skill1", sender.Tamer.Digimon[0].skill1.Id}
                                , { "skill2", sender.Tamer.Digimon[0].skill2.Id} }
                                , "WHERE nome=@nome AND model=@model", new QueryParameters() { { "nome", name }
                                , { "model", sender.Tamer.Digimon[0].Model } });
                                Send(sender, "'" + name + "' atualizado com sucesso! Verifique as skills.");
                            }
                            else
                                Send(sender, "Este Digimon '" + name + "', nao existe no banco.");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /savedigimon <Nome> <F1 VP> <F2 VP>");

                        return true;
                    }

                    // Carregando Digimon a partir de arquivo
                    else if ((split[0] == "/load" || split[0] == "/loaddigimon") && sender.User.Autoridade >= 100)
                    {
                        byte[] data = StringHex.Hex2Binary(PacketString.Read("digimon"));
                        InPacket packet = new InPacket(data);

                        byte[] trash = packet.ReadBytes(20);
                        sender.Tamer.Digimon[0].RModel = packet.ReadInt();
                        sender.Tamer.Digimon[0].CModel = sender.Tamer.Digimon[0].RModel;
                        sender.Tamer.Digimon[0].UModel = sender.Tamer.Digimon[0].RModel;
                        sender.Tamer.Digimon[0].MModel = sender.Tamer.Digimon[0].RModel;
                        sender.Tamer.Digimon[0].Model = (ushort)sender.Tamer.Digimon[0].RModel;
                        sender.Tamer.Digimon[0].EvolutionQuant = packet.ReadByte();
                        string nome = packet.ReadString(21);
                        trash = packet.ReadBytes(114);

                        // Fase
                        byte fase = packet.ReadByte();
                        while (fase == 0)
                            fase = packet.ReadByte();
                        sender.Tamer.Digimon[0].estage = fase;
                        sender.Tamer.Digimon[0].type = packet.ReadByte();
                        sender.Tamer.Digimon[0].Rtype = sender.Tamer.Digimon[0].type;
                        sender.Tamer.Digimon[0].Ctype = sender.Tamer.Digimon[0].type;
                        sender.Tamer.Digimon[0].Utype = sender.Tamer.Digimon[0].type;
                        sender.Tamer.Digimon[0].Mtype = sender.Tamer.Digimon[0].type;
                        trash = packet.ReadBytes(94);
                        sender.Tamer.Digimon[0].skill1.Id = packet.ReadInt();
                        sender.Tamer.Digimon[0].skill1.Lvl = packet.ReadShort();
                        sender.Tamer.Digimon[0].skill1.Poder = packet.ReadShort();
                        sender.Tamer.Digimon[0].skill1.Range = packet.ReadByte();
                        sender.Tamer.Digimon[0].skill1.Units = packet.ReadByte();

                        sender.Tamer.Digimon[0].Rskill1.Id = sender.Tamer.Digimon[0].skill1.Id;
                        sender.Tamer.Digimon[0].Rskill1.Lvl = sender.Tamer.Digimon[0].skill1.Lvl;
                        sender.Tamer.Digimon[0].Rskill1.Poder = sender.Tamer.Digimon[0].skill1.Poder;
                        sender.Tamer.Digimon[0].Rskill1.Range = sender.Tamer.Digimon[0].skill1.Range;
                        sender.Tamer.Digimon[0].Rskill1.Units = sender.Tamer.Digimon[0].skill1.Units;

                        sender.Tamer.Digimon[0].Uskill1.Id = sender.Tamer.Digimon[0].skill1.Id;
                        sender.Tamer.Digimon[0].Uskill1.Lvl = sender.Tamer.Digimon[0].skill1.Lvl;
                        sender.Tamer.Digimon[0].Uskill1.Poder = sender.Tamer.Digimon[0].skill1.Poder;
                        sender.Tamer.Digimon[0].Uskill1.Range = sender.Tamer.Digimon[0].skill1.Range;
                        sender.Tamer.Digimon[0].Uskill1.Units = sender.Tamer.Digimon[0].skill1.Units;

                        sender.Tamer.Digimon[0].Cskill1.Id = sender.Tamer.Digimon[0].skill1.Id;
                        sender.Tamer.Digimon[0].Cskill1.Lvl = sender.Tamer.Digimon[0].skill1.Lvl;
                        sender.Tamer.Digimon[0].Cskill1.Poder = sender.Tamer.Digimon[0].skill1.Poder;
                        sender.Tamer.Digimon[0].Cskill1.Range = sender.Tamer.Digimon[0].skill1.Range;
                        sender.Tamer.Digimon[0].Cskill1.Units = sender.Tamer.Digimon[0].skill1.Units;

                        sender.Tamer.Digimon[0].Mskill1.Id = sender.Tamer.Digimon[0].skill1.Id;
                        sender.Tamer.Digimon[0].Mskill1.Lvl = sender.Tamer.Digimon[0].skill1.Lvl;
                        sender.Tamer.Digimon[0].Mskill1.Poder = sender.Tamer.Digimon[0].skill1.Poder;
                        sender.Tamer.Digimon[0].Mskill1.Range = sender.Tamer.Digimon[0].skill1.Range;
                        sender.Tamer.Digimon[0].Mskill1.Units = sender.Tamer.Digimon[0].skill1.Units;

                        trash = packet.ReadBytes(10);
                        sender.Tamer.Digimon[0].skill2.Id = packet.ReadInt();
                        sender.Tamer.Digimon[0].skill2.Lvl = packet.ReadShort();
                        sender.Tamer.Digimon[0].skill2.Poder = packet.ReadShort();
                        sender.Tamer.Digimon[0].skill2.Range = packet.ReadByte();
                        sender.Tamer.Digimon[0].skill2.Units = packet.ReadByte();

                        sender.Tamer.Digimon[0].Rskill2.Id = sender.Tamer.Digimon[0].skill2.Id;
                        sender.Tamer.Digimon[0].Rskill2.Lvl = sender.Tamer.Digimon[0].skill2.Lvl;
                        sender.Tamer.Digimon[0].Rskill2.Poder = sender.Tamer.Digimon[0].skill2.Poder;
                        sender.Tamer.Digimon[0].Rskill2.Range = sender.Tamer.Digimon[0].skill2.Range;
                        sender.Tamer.Digimon[0].Rskill2.Units = sender.Tamer.Digimon[0].skill2.Units;

                        sender.Tamer.Digimon[0].Uskill2.Id = sender.Tamer.Digimon[0].skill2.Id;
                        sender.Tamer.Digimon[0].Uskill2.Lvl = sender.Tamer.Digimon[0].skill2.Lvl;
                        sender.Tamer.Digimon[0].Uskill2.Poder = sender.Tamer.Digimon[0].skill2.Poder;
                        sender.Tamer.Digimon[0].Uskill2.Range = sender.Tamer.Digimon[0].skill2.Range;
                        sender.Tamer.Digimon[0].Uskill2.Units = sender.Tamer.Digimon[0].skill2.Units;

                        sender.Tamer.Digimon[0].Cskill2.Id = sender.Tamer.Digimon[0].skill2.Id;
                        sender.Tamer.Digimon[0].Cskill2.Lvl = sender.Tamer.Digimon[0].skill2.Lvl;
                        sender.Tamer.Digimon[0].Cskill2.Poder = sender.Tamer.Digimon[0].skill2.Poder;
                        sender.Tamer.Digimon[0].Cskill2.Range = sender.Tamer.Digimon[0].skill2.Range;
                        sender.Tamer.Digimon[0].Cskill2.Units = sender.Tamer.Digimon[0].skill2.Units;

                        sender.Tamer.Digimon[0].Mskill2.Id = sender.Tamer.Digimon[0].skill2.Id;
                        sender.Tamer.Digimon[0].Mskill2.Lvl = sender.Tamer.Digimon[0].skill2.Lvl;
                        sender.Tamer.Digimon[0].Mskill2.Poder = sender.Tamer.Digimon[0].skill2.Poder;
                        sender.Tamer.Digimon[0].Mskill2.Range = sender.Tamer.Digimon[0].skill2.Range;
                        sender.Tamer.Digimon[0].Mskill2.Units = sender.Tamer.Digimon[0].skill2.Units;

                        byte b; // Lendo o restante do pacote
                        while (packet.Remaining > 0) b = packet.ReadByte();

                        sender.Connection.Send(new Network.Packets.PACKET_DIGIMON_INDIVIDUAL(sender.Tamer.Digimon[0]));
                        Send(sender, "Carregando Digimon a partir do arquivo... Model: " + sender.Tamer.Digimon[0].Model);

                        return true;
                    }

                    // Heal Digimon
                    if (split[0] == "/heal" && sender.User.Autoridade >= 100)
                    {
                        sender.Tamer.Digimon[0].Health = sender.Tamer.Digimon[0].MaxHealth;
                        sender.Tamer.Digimon[0].VP = sender.Tamer.Digimon[0].MaxVP;
                        sender.Tamer.Digimon[0].EV = sender.Tamer.Digimon[0].MaxEV;
                        sender.Tamer.Digimon[0].AddHP(sender.Tamer.Digimon[0].MaxHealth);
                        sender.Tamer.Digimon[0].AddVP(sender.Tamer.Digimon[0].MaxVP);
                        sender.Tamer.Digimon[0].AddEVP(sender.Tamer.Digimon[0].MaxEV);
                        sender.Connection.Send(new Network.Packets.PACKET_DIGIMON_INDIVIDUAL(sender.Tamer.Digimon[0]));
                        Send(sender, "Digimon curado!");

                        return true;
                    }

                    // Fator de Experiência
                    if (split[0] == "/exp" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 1)
                        {
                            Emulator.Enviroment.ExpFator = 1;
                            Emulator.Enviroment.Config.ExpFator = 1;
                            Emulator.Enviroment.Database.Update("config", new QueryParameters() { { "valor", 1 } }
                                , "WHERE config=@config", new QueryParameters() { { "config", "ExpFator" } });
                            Send(sender, "Normalized experience factor.");
                            Utils.Comandos.SendGM("Yggdrasil", "EXP Adicional: 0%");
                            Utils.Comandos.SendGMLayout2(sender, "00");
                        }
                        else if (split.Length == 2)
                        {
                            
                            double fator = double.Parse(split[1]);
                            if ((fator >= 1) && (fator <= 21))
                            {
                                //na realidade o segundo é o verdadeiro, mas nao sei se o primeiro pode ser ignorado.
                                Emulator.Enviroment.ExpFator = fator;
                                Emulator.Enviroment.Config.ExpFator = fator;
                                Emulator.Enviroment.Database.Update("config", new QueryParameters() { { "valor", fator } }
                                    , "WHERE config=@config", new QueryParameters() { { "config", "ExpFator" } });
                                Send(sender, string.Format("Experience factor changed to: {0}", fator));
                                //Emulator.Enviroment.Config.Atualizar();

                                fator = (fator - 1) * 100;
                                Utils.Comandos.SendGM("YGGDRASIL", "EXP Adicional: " + fator + "%");

                                //MANDA A IMAGEM DA EXP ATUAL
                                int pacoteCode;
                                pacoteCode = ((int)fator) / 25;
                                string myHex = pacoteCode.ToString("X2");
                                Utils.Comandos.SendGMLayout2(sender, myHex);
                            }
                            else
                            {
                                Send(sender, string.Format("Valor deve ser entre 1 e 21"));
                            }
                            
                        }

                        return true;
                    }

                    // Maintenance
                    if (split[0] == "/manutencao" && sender.User.Autoridade >= 100)
                    {
                        // Inverting the flag
                        Emulator.Enviroment.Manutencao = !Emulator.Enviroment.Manutencao;

                        // Server went into maintenance
                        if (Emulator.Enviroment.Manutencao)
                        {
                            Send(sender, "The server is now on maintaince.");

                            foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                            {
                                if (c.User.Autoridade <= 0)
                                    c.Disconect();
                            }
                        }
                        else
                            Send(sender, "Maintenance mode disabled.");

                        return true;
                    }

                    // Creating drop on the ground
                    else if (split[0] == "/drop" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length >= 3)
                        {
                            int quant = int.Parse(split[1]);
                            string name = text.Replace("/drop ", "").Replace(split[1] + " ", "");
                            if (Emulator.Enviroment.Codex.ContainsKey(name))
                            {
                                ItemCodex item = Emulator.Enviroment.Codex[name];
                                Random r = new Random(DateTime.Now.Millisecond + (1000));
                                int ID = sender.Tamer.Id;
                                float posx = sender.Tamer.Location.X;
                                float posy = sender.Tamer.Location.Y;
                                MapZone zone = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                                ItemMap newItem = new ItemMap(item.GetItem(quant, r.Next(100) + ID)
                                    , new Game.Data.Vector2(posx, posy), zone, ID);
                                newItem.Zone = zone;
                                zone.Items.Add(newItem);
                                zone.sendItem(newItem);

                                Send(sender, quant.ToString() + " " + name + " dropped.");
                            }
                            else
                                Send(sender, "This item does not exist. Check the name entered.");
                        }
                        else
                            Send(sender, "Invalid Command. Type: /drop <quant> <item>");

                        return true;
                    }

                    // Rank configuration
                    if (split[0] == "/rankconfig" && sender.User.Autoridade >= 100)
                    {
                        // Configuração Geral
                        if (split.Length == 4)
                        {
                            int rank = int.Parse(split[1]);
                            string parametro = split[2];
                            int valor = int.Parse(split[3]);

                            // O Parâmetro existe?
                            StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>(
                                "comentario as value", "rank_config", "WHERE parametro=@parametro", new QueryParameters() {
                                    { "parametro", parametro } });
                            if (result.HasRows)
                            {
                                // Se o parâmetro já existe para o rank, vamos alterar
                                VerificationResult res = Emulator.Enviroment.Database.Select<VerificationResult>(
                                "id", "rank_config", "WHERE parametro=@parametro AND rank=@rank",
                                new QueryParameters() { { "parametro", parametro }, { "rank", rank } });
                                if (res.HasRows)
                                {
                                    Emulator.Enviroment.Database.Update("rank_config", new QueryParameters()
                                    { { "valor", valor } }, "WHERE parametro=@parametro AND rank=@rank",
                                    new QueryParameters() { { "parametro", parametro }, { "rank", rank } });
                                }
                                // Se não existe, vamos inserir
                                else
                                {
                                    Emulator.Enviroment.Database.Insert<uint>("rank_config", new QueryParameters()
                                    { { "rank", rank }, { "parametro", parametro }, { "valor", valor }
                                    , { "comentario", result.value } });
                                }

                                Emulator.Enviroment.CarregarRankConfig();
                                Send(sender, "Parametro " + parametro + " alterado para " + valor + " no rank "
                                    + rank + " com sucesso!");
                            }
                            else
                                Send(sender, "O parametro " + parametro + " nao existe.");
                        }
                        // Configuração por Nome do Digimon
                        else if (split.Length >= 2)
                        {
                            string[] arg = text.Replace("/rankconfig ", "").Split('.');

                            if (arg.Length == 4)
                            {
                                string name = arg[0];
                                int rank = int.Parse(arg[1]);
                                string parametro = arg[2];
                                int valor = int.Parse(arg[3]);

                                // O Parâmetro existe?
                                StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>(
                                    "comentario as value", "rank_config", "WHERE parametro=@parametro", new QueryParameters() {
                                    { "parametro", parametro } });
                                if (result.HasRows)
                                {
                                    // Se o parâmetro já existe para o nome e rank, vamos alterar
                                    VerificationResult res = Emulator.Enviroment.Database.Select<VerificationResult>(
                                    "id", "rank_config", "WHERE parametro=@parametro AND rank=@rank AND digimon=@name",
                                    new QueryParameters() { { "parametro", parametro }, { "rank", rank }
                                , { "name", name } });
                                    if (res.HasRows)
                                    {
                                        Emulator.Enviroment.Database.Update("rank_config", new QueryParameters()
                                    { { "valor", valor } }, "WHERE parametro=@parametro AND rank=@rank "
                                        + "AND digimon=@name",
                                        new QueryParameters() { { "parametro", parametro }, { "rank", rank }
                                    , { "name", name } });
                                    }
                                    // Se não existe, vamos inserir
                                    else
                                    {
                                        Emulator.Enviroment.Database.Insert<uint>("rank_config", new QueryParameters()
                                    { { "rank", rank }, { "parametro", parametro }, { "valor", valor }
                                    , { "digimon", name }, { "comentario", result.value } });
                                    }

                                    Emulator.Enviroment.CarregarRankConfig();
                                    Send(sender, "Parametro " + parametro + " alterado para " + valor + ", no rank "
                                        + rank + ", do digimon " + name + " com sucesso!");
                                }
                                else
                                    Send(sender, "O parametro " + parametro + " nao existe.");
                            }
                            else
                                Send(sender, "Comando invalido. Digite: /rankconfig <rank> <parametro> <valor> OU "
                                    + "/rankconfig <nome>.<rank>.<parametro>.<valor>");
                        }
                        else
                            Send(sender, "Comando invalido. Digite: /rankconfig <rank> <parametro> <valor> OU "
                                + "/rankconfig <nome>.<rank>.<parametro>.<valor>");

                        return true;
                    }
                    // Configuração por Spawn_Id
                    if (split[0] == "/configspawn" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 4)
                        {
                            int spawn_id = int.Parse(split[1]);
                            string parametro = split[2];
                            int valor = int.Parse(split[3]);

                            // O Parâmetro existe?
                            StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>(
                                "comentario as value", "rank_config", "WHERE parametro=@parametro", new QueryParameters() {
                                    { "parametro", parametro } });
                            if (result.HasRows)
                            {
                                // Se o parâmetro já existe para o spawn_id, vamos alterar
                                VerificationResult res = Emulator.Enviroment.Database.Select<VerificationResult>(
                                "id", "rank_config", "WHERE parametro=@parametro AND spawn_id=@spawn_id",
                                new QueryParameters() { { "parametro", parametro }, { "spawn_id", spawn_id } });
                                if (res.HasRows)
                                {
                                    Emulator.Enviroment.Database.Update("rank_config", new QueryParameters()
                                    { { "valor", valor } }, "WHERE parametro=@parametro AND spawn_id=@spawn_id",
                                    new QueryParameters() { { "parametro", parametro }, { "spawn_id", spawn_id } });
                                }
                                // Se não existe, vamos inserir
                                else
                                {
                                    Emulator.Enviroment.Database.Insert<uint>("rank_config", new QueryParameters()
                                    { { "spawn_id", spawn_id }, { "parametro", parametro }, { "valor", valor }
                                    , { "comentario", result.value } });
                                }

                                Emulator.Enviroment.CarregarRankConfig();
                                Send(sender, "Parametro " + parametro + " alterado para " + valor + " no spawn_id "
                                    + spawn_id + " com sucesso!");
                            }
                            else
                                Send(sender, "O parametro " + parametro + " nao existe.");
                        }
                        else
                            Send(sender, "Invalid Command. Type: /configspawn <spawn_id> <parametro> <valor>");

                        return true;
                    }

                    // Mensagem de GM
                    if (split[0] == "/m" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length > 1)
                        {
                            string msg = text.Replace("/m ", "");

                            //DISCORD WEBHOOK
                            Utils.Http.Post("https://discord.com/api/webhooks/878785915869364245/oazC47ul84uAm1nRaFm6nKxKiY2-kvB5f6yVMmSSfgoYCCbPuicrnfjU4ld9q7xupfCT",
                                new System.Collections.Specialized.NameValueCollection()
                            {
                                {
                                    "username", "GM " + sender.Tamer.Name
                                },
                                {
                                    "content", msg
                                }
                            });
                            foreach (Client c in Emulator.Enviroment.LoginListener.Clients)
                                if (c != null)
                                {
                                    c.Connection.Send(new Network.Packets.PACKET_CHAT_GM("GM " + sender.Tamer.Name, msg));
                                }
                        }
                        else
                            Send(sender, "Invalid Command. Type: /m <messageGM>");

                        return true;
                    }

                    // Mensagem de GM 2
                    if (split[0] == "/shout" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length > 1)
                        {
                            string msg = text.Replace("/shout ", "");

                            foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                                if (c != null && c.Connection != null)
                                {
                                    //SendWithType(c, " " + c.Tamer.Name, 2);
                                    c.Connection.Send(new Network.Packets.PACKET_CHAT_SPEAKER(2, "[GM " + sender.Tamer.Name + "] " + msg, 128));
                                }

                        }
                        else
                            Send(sender, "Invalid Command. Type: /shout <messageGM>");

                        return true;
                    }

                    // Mensagem de GM 2
                    if (split[0] == "/online" && sender.User.Autoridade >= 100)
                    {
                        if (split.Length == 1)
                        {
                            foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                                if (c != null && c.Connection != null)
                                    if (c.Tamer.Name != null)
                                        Send(sender, " " + c.Tamer.Name);
                        }
                        else
                            Send(sender, "Invalid Command. Type: /shout <messageGM>");

                        return true;
                    }

                    // Atualizar configuração
                    if (split[0] == "/config" && sender.User.Autoridade >= 100)
                    {
                        Emulator.Enviroment.Config.Atualizar();
                        Send(sender, "Updated Configuration!");

                        return true;
                    }

                    // Kickando tamer
                    if ((split[0] == "/kick")
                        && sender.User.Autoridade >= 10)
                    {
                        if (split.Length > 1)
                        {
                            string nick = text.Replace("/kick ", "");
                            Client found = null;
                            foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                            {
                                if (c.Tamer != null && c.Tamer.Name == nick)
                                {
                                    found = c;
                                    break;
                                }
                            }
                            if (found != null)
                            {
                                if (sender.User.Autoridade > found.User.Autoridade)
                                {
                                    SendGM(Emulator.Enviroment.GMNick
                                        , string.Format("{0} was kicked out of the server.", nick));

                                    found.Disconect();
                                }
                                else
                                {
                                    Send(sender, string.Format("You have no authority to kick {0}", nick));
                                }

                                return true;
                            }
                            else
                                Send(sender, string.Format("Tamer not found: {0}", nick));
                        }
                        else
                            Send(sender, "Invalid Command. Type: /kick <nick>");

                        return true;
                    }

                    // Banindo contas
                    // Horas
                    if ((split[0] == "/ban")
                        && sender.User.Autoridade >= 10)
                    {
                        if (split.Length > 2)
                        {
                            int hours = int.Parse(split[1]);
                            string nick = text.Replace("/ban " + hours + " ", "");
                            Client found = null;
                            foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                            {
                                if (c.Tamer != null && c.Tamer.Name == nick)
                                {
                                    found = c;
                                    break;
                                }
                            }
                            if (found != null)
                            {
                                if (sender.User.Autoridade > found.User.Autoridade)
                                {
                                    SendGM(Emulator.Enviroment.GMNick
                                        , string.Format("{0} was banned from the server for {1} hours.", nick
                                        , hours));

                                    Emulator.Enviroment.Database.Update("users", new QueryParameters() {
                                        { "ban", hours } }, "WHERE id=@id", new QueryParameters() {
                                            { "id", found.User.Id } });

                                    found.Disconect();
                                }
                                else
                                {
                                    Send(sender, string.Format("You have no authority to ban {0}", nick));
                                }

                                return true;
                            }
                            else
                                Send(sender, string.Format("Tamer not found: {0}", nick));
                        }
                        else
                            Send(sender, "Invalid Command. Type: /ban <hours> <nick>");

                        return true;
                    }
                    // IP
                    if ((split[0] == "/banip")
                        && sender.User.Autoridade >= 10)
                    {
                        if (split.Length > 1)
                        {
                            string nick = text.Replace("/banip ", "");
                            Client found = null;
                            foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                            {
                                if (c.Tamer != null && c.Tamer.Name == nick)
                                {
                                    found = c;
                                    break;
                                }
                            }
                            if (found != null)
                            {
                                if (sender.User.Autoridade > found.User.Autoridade)
                                {
                                    SendGM(Emulator.Enviroment.GMNick
                                        , string.Format("{0} has been banned from the server.", nick));

                                    string textFile = Directory.GetCurrentDirectory() + "/ip_block.txt";
                                    Emulator.Enviroment.IP_Block.Add(found.Connection.ip);
                                    using (StreamWriter outputFile = new StreamWriter(textFile, append: true))
                                    {
                                        outputFile.WriteLine(found.Connection.ip);
                                    }

                                    found.Disconect();
                                }
                                else
                                {
                                    Send(sender, string.Format("You have no authority to ban {0}", nick));
                                }

                                return true;
                            }
                            else
                                Send(sender, string.Format("Tamer not found: {0}", nick));
                        }
                        else
                            Send(sender, "Invalid Command. Type: /banip <nick>");

                        return true;
                    }

                    // Teste
                    if (Emulator.Enviroment.Teste)
                    {
                        if (split[0] == "/t" && sender.User.Autoridade >= 100)
                        {
                            //sender.Connection.Send(new Network.Packets.PACKET_TESTE());

                            if (split.Length == 3)
                            {
                                int id = int.Parse(split[1]);
                                byte teste = byte.Parse(split[2]);
                                MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                                foreach (Spawn s in map.spawn)
                                {
                                    if (s.Id == id)
                                    {
                                        sender.Connection.Send(new Network.Packets.PACKET_TESTE(s, teste));
                                        break;
                                    }
                                }
                            }
                            else
                                Send(sender, "Invalid Command. Type: /despawn <ID>");

                            return true;
                        }
                        if (split[0] == "/p" && sender.User.Autoridade >= 100)
                        {
                            foreach (Client c in Emulator.Enviroment.LoginListener.Clients)
                                if (c.User != null && c.User.Username == sender.User.Username)
                                    c.Connection.Send(new Network.Packets.PACKET_TESTE());

                            return true;
                        }
                        if (split[0] == "/c" && sender.User.Autoridade >= 100)
                        {
                            string key = split[1];
                            PacketString.CryptoRead("pacote", key);

                            Send(sender, "Decrypted package!");

                            return true;
                        }
                    }
                }
            }


            return false;
        }

        public static void Send(Client sender, string text)
        {
            sender.Connection.Send(new Network.Packets.PACKET_CHAT_SYSTEM(text, 256));
        }

        public static void SendWithType(Client sender, string text, byte tipo)
        {
            sender.Connection.Send(new Network.Packets.PACKET_CHAT_SYSTEM(text, 256, tipo));
        }
        public static void SendGM(string nick, string text)
        {
            foreach (Client c in Emulator.Enviroment.LoginListener.Clients)
                if (c != null && c.Connection != null)
                    c.Connection.Send(new Network.Packets.PACKET_CHAT_GM(nick, text));
        }

        public static void SendGMType(Client sender, byte tipo, string text)
        {
            sender.Connection.Send(new Network.Packets.PACKET_CHAT_SYSTEM(tipo, text, 256));
        }

        public static void SendGMLayout(Client sender, string nick, string text)
        {
            //O QUE EU QUERIA ERA MANDAR A MSG DO GM ESPECIFICAMENTE PARA UM JOGADOR APENAS. ACONTECE
            //QUE QUANDO ESSE PACOTE EH ENVIADO, O CLIENT INTERPRETA COMO EVENTO DE EXP E APARECE O ICONE
            //DO EXP TODO BUGADO.
            //NÃO FUNCIONA :(
            sender.Connection.Send(new Network.Packets.PACKET_CHAT_GM(nick, text));
            //sender.Connection.Send(new Network.Packets.PACKET_CHAT_GM(nick, text));
        }

        public static void SendGMLayout2(Client sender, string nick)
        {
            //O QUE EU QUERIA ERA MANDAR A MSG DO GM ESPECIFICAMENTE PARA UM JOGADOR APENAS. ACONTECE
            //QUE QUANDO ESSE PACOTE EH ENVIADO, O CLIENT INTERPRETA COMO EVENTO DE EXP E APARECE O ICONE
            //DO EXP TODO BUGADO.
            //NÃO FUNCIONA :(
            sender.Connection.Send(new Network.Packets.PACKET_CHAT_GM(nick, true));
            //sender.Connection.Send(new Network.Packets.PACKET_CHAT_GM(nick, text));
        }
    }
}
