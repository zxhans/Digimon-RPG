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
    // Processo de evolução - 4 BESTAS
    [PacketHandler(Type = PacketType.PACKET_EVOLUTION_FANG, Connection = ConnectionType.Map)]
    public class PACKET_EVOLUTION_FANG : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6); // Preenchimento


            int op = packet.ReadInt(); // ID do Digimon selecionado

            int ID = packet.ReadInt(); // ID do Digimon selecionado

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            //Utils.Comandos.Send(sender, "fang" + ID + "| op: " + op);

            //fanglongmon
            //op 42 pra usar os 50 itens
            if (op == 20)
            {
                sender.Tamer.DigimonIDToBecomeFanglong = ID;
                Random r = new Random();
                int random = r.Next(100);

                bool valid = false;
                bool azulong = false;
                bool xuan = false;
                bool zhuqiao = false;
                bool baihu = false;

                foreach (Digimon digimon in sender.Tamer.Digimon)
                {
                    if (digimon != null && digimon.Slot >= 1 && digimon.Slot <= 4)
                    {
                        //Utils.Comandos.Send(sender, digimon.Name + " Slot " + digimon.Slot + " - " + digimon.MegaForm);

                        if (digimon.MegaForm == 652)
                        {
                            //AZULONG
                            azulong = true;
                        }

                        if (digimon.MegaForm == 651)
                        {
                            //ZHUQIAO
                            zhuqiao = true;
                        }

                        if (digimon.MegaForm == 653)
                        {
                            //BAIHU
                            baihu = true;
                        }

                        if (digimon.MegaForm == 650)
                        {
                            //XUAN
                            xuan = true;
                        }
                        //sender.Connection.Send(new Packets.PACKET_EVOLUTION_FANG(27, ID, digimon));
                    }
                }

                if (xuan && baihu && azulong && zhuqiao)
                {
                    valid = true;
                }

                if (valid)
                {
                    if (random <= 5)//rate de 5%
                    {
                        //SUCESSO!
                        foreach (Digimon digimon in sender.Tamer.Digimon)
                        {
                            if (digimon != null && digimon.Id == ID)
                            {
                                sender.Connection.Send(new Packets.PACKET_EVOLUTION_FANG(27, ID));
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (Digimon digimon in sender.Tamer.Digimon)
                        {
                            if (digimon != null && digimon.Id == ID)
                            {
                                sender.Connection.Send(new Packets.PACKET_EVOLUTION_FANG(26, ID));
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Utils.Comandos.Send(sender, "Need all Four Holy Beast in your team, all Level 100 or more!");
                    sender.Connection.Send(new Packets.PACKET_EVOLUTION_FANG(28, ID));
                }
                
            }
            else if (op == 26)
            {
                //REALIZANDO ACOES PQ FALHOU
                Utils.Comandos.Send(sender, "Jogress failed!");
                sender.Tamer.AddItem("4HolyBeastItem", 1, true);
                sender.Tamer.AtualizarInventario();
            }
            else if (op == 27)
            {
                //REALIZANDO ACOES PQ TEVE SUCESSO
                ID = sender.Tamer.DigimonIDToBecomeFanglong;
                Utils.Comandos.Send(sender, "Jogress done! Congratulations!");
                foreach (Digimon digimon in sender.Tamer.Digimon)
                {
                    if (digimon != null && digimon.Id == ID)
                    {
                        Utils.Comandos.SendGM("Yggdrasil", digimon.Tamer.Name + " got an successfull special evolution to Fanglongmon! Congratulations!");

                        digimon.ResetStatus();

                        int digimonID = 718;
                        digimon.NewDigievolutionRookie(5, digimonID);
                        break;
                    }
                }

            }
            else if (op == 42)
            {
                //26 = FAIL
                //27 = SUCESSO
                //Utils.Comandos.Send(sender, "opcao 3");
                sender.Tamer.DigimonIDToBecomeFanglong = ID;
                string itemName = "4HolyBeastItem";
                int itemQntd = 50;
                if (sender.Tamer.ItemCount(itemName) >= itemQntd)
                {
                    sender.Tamer.RemoveItem(itemName, itemQntd);
                    sender.Tamer.AtualizarInventario();
                    foreach (Digimon digimon in sender.Tamer.Digimon)
                    {
                        if (digimon != null && digimon.Id == ID)
                        {
                            sender.Connection.Send(new Packets.PACKET_EVOLUTION_FANG(27, ID));
                            break;
                        }
                    }
                }
                else
                {
                    foreach (Digimon digimon in sender.Tamer.Digimon)
                    {
                        if (digimon != null && digimon.Id == ID)
                        {
                            sender.Connection.Send(new Packets.PACKET_EVOLUTION_FANG(0, ID));
                            break;
                        }
                    }
                }
                
            }
            else
            {
                ID = sender.Tamer.DigimonIDToBecomeFanglong;
                sender.Connection.Send(new Packets.PACKET_EVOLUTION_FANG(0, ID));
            }
        }
    }
}
