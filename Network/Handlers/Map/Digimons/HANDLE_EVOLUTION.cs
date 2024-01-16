using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Processo de evolução (Card/Armor)
    [PacketHandler(Type = PacketType.PACKET_EVOLUTION, Connection = ConnectionType.Map)]
    public class HANDLE_EVOLUTION : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6); // Preenchimento

            byte type = packet.ReadByte(); // 1 - Armor, 2 - Card

            byte[] trash2 = packet.ReadBytes(3); // Preenchimento

            int ID = packet.ReadInt(); // ID do Digimon selecionado
            int item_id = packet.ReadInt(); // ID do item usado

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            byte r = 1;
            int custo = 0;

            // Procurando o item
            foreach(Item item in sender.Tamer.Items)
                if(item != null && item.Id == item_id && item.ItemQuant > 0)
                {
                    Debug.Print("Achou o item");
                    // Procurando o Digimon
                    foreach(Digimon digimon in sender.Tamer.Digimon)
                        if(digimon != null && digimon.Id == ID)
                        {
                            Debug.Print("Achou o digimon");
                            // Obtendo o Codex
                            EvolutionCodexResult result = Emulator.Enviroment.Database.Select<EvolutionCodexResult>
                                ("*", "evolution_codex", "WHERE itens LIKE '%" + item.ItemName + "%'"
                                +" AND prev IN (" + digimon.RookieForm + ", " + digimon.ChampForm + ", " 
                                + digimon.UltimForm + ", " + digimon.MegaForm + ")"
                                + " AND target IN (" + digimon.RookieForm + ", " + digimon.ChampForm + ", "
                                + digimon.UltimForm + ", " + digimon.MegaForm + ")");

                            // Encontramos resultados?
                            if (result.HasRows)
                            {
                                //OBSERVAÇÃO:   No jogo KDRO original, tem as chances diminuidas caso o level do tamer/digimon não seja o necessario pra isso.
                                //              Aqui só faz se for tudo requisito completo
                                Debug.Print("Achou o codex");
                                // Validando requisitos
                                if (sender.Tamer.Reputation >= result.tamer_fame
                                    && digimon.Battles >= result.battles
                                    && digimon.BattleWins >= (digimon.Battles * result.win) / 100
                                    && sender.Tamer.Bits >= result.custo
                                    && (digimon.RookieForm == result.prev || digimon.ChampForm == result.prev
                                    || digimon.UltimForm == result.prev || digimon.MegaForm == result.prev)
                                    && (digimon.RookieForm == result.target || digimon.ChampForm == result.target
                                    || digimon.UltimForm == result.target || digimon.MegaForm == result.target))
                                {
                                    int prejuizo = 0;

                                    //SE O TAMER LV FOR MENOR QUE O REQUISITO PERDE 15%
                                    if (sender.Tamer.Level < result.tamer_lvl)
                                    {
                                        prejuizo += 15;
                                    }

                                    //SE O DIGI LVL FOR MENOR QUE O REQUISITO PERDE 15%
                                    if (digimon.Level < result.digimon_lvl)
                                    {
                                        prejuizo += 15;
                                    }

                                    Debug.Print("Atendeu os requisitos");
                                    // O tamer atendeu todos os requisitos, vamos processar a solicitação
                                    // % de sucesso
                                    int chance = 10;
                                    foreach(string i in result.itens)
                                    {
                                        string[] s = i.Split('-');
                                        if (s.Length == 2 && s[0] == item.ItemName)
                                            chance = int.Parse(s[1]);
                                    }

                                    chance = chance - prejuizo;

                                    // Definindo a fase
                                    byte fase = 2;
                                    int rare = 879; // Raremon da fase específica, em caso de falha
                                    if(digimon.ChampForm == result.target)
                                    {
                                        fase = 3;
                                        rare = 81;
                                    }
                                    if(digimon.UltimForm == result.target)
                                    {
                                        fase = 4;
                                        rare = 880;
                                    }
                                    if(digimon.MegaForm == result.target)
                                    {
                                        fase = 5;
                                        rare = 881;
                                    }

                                    Random random = new Random();
                                    if(random.Next(100) <= chance)
                                    {
                                        // Sucesso!
                                        r = 2;

                                        if (type == 1)
                                        {
                                            digimon.NewDigievolutionRookie(fase, result.digimon_id);
                                            Utils.Comandos.SendGM("Yggdrasil", digimon.Tamer.Name + " got an successfull armor evolution to " + result.digimon_name + "! Congratulations!");
                                        }
                                        else
                                        {
                                            digimon.NewDigievolution(fase, result.digimon_id);
                                            Utils.Comandos.SendGM("Yggdrasil", digimon.Tamer.Name + " got an successfull card evolution to " + result.digimon_name + "! Congratulations!");
                                            if (result.digimon_id == 1014)
                                            {
                                                //FIX DO OMEGAMON DO GABUMON
                                                digimon.NewDigievolution(4, 841);
                                            }
                                        }

                                        
                                    }
                                    else
                                    {
                                        // Falha...
                                        digimon.NewDigievolution(fase, rare);
                                        Utils.Comandos.SendGM("Yggdrasil", digimon.Tamer.Name + " got an Raremon as an evolution failure! Press F.");
                                    }

                                    digimon.ResetStatus();
                                    // Respondendo o Client
                                    digimon.CarregarEvolutions();
                                    digimon.Digivolver(0);
                                    sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(digimon));

                                    // Descontando itens
                                    sender.Tamer.RemoveItem(item.ItemName, 1);
                                    sender.Tamer.RemoveItem("Evolutor", 1);
                                    sender.Tamer.GainBit(-result.custo);
                                    sender.Tamer.AtualizarInventario();
                                    custo = result.custo;
                                }
                                else if (sender.Tamer.Bits < result.custo)
                                {
                                    Utils.Comandos.Send(sender, "BITs insuficiente!");
                                }


                            }

                            break;
                        }

                    break;
                }

            // respondendo o client
            sender.Connection.Send(new Packets.PACKET_EVOLUTION(r, custo));
        }
    }
}
