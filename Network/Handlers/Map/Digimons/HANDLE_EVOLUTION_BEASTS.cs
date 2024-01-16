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
    [PacketHandler(Type = PacketType.PACKET_EVOLUTION_BEASTS, Connection = ConnectionType.Map)]
    public class HANDLE_EVOLUTION_BEASTS : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6); // Preenchimento

            byte type = packet.ReadByte(); // 1 - Armor, 2 - Card

            byte[] trash2 = packet.ReadBytes(3); // Preenchimento

            int ID = packet.ReadInt(); // ID do Digimon selecionado

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            byte r = 1;
            int custo = 0;

            string itemName = "Azulongmon Core";
            string digimonName = "Azulongmon";
            int digimonID = 652;
            int reqItemQntd = 12;

            if (type == 1)
            {
                //azulongmon
                itemName = "Azulongmon Core";
                digimonName = "Azulongmon";
                digimonID = 652;
            }
            else if (type == 2)
            {
                //zhuqiao
                itemName = "Zhuqiaomon Core";
                digimonName = "Zhuqiaomon";
                digimonID = 651;
            }
            else if (type == 3)
            {
                //baihu
                itemName = "Baihumon Core";
                digimonName = "Baihumon";
                digimonID = 653;
            }
            else if (type == 4)
            {
                //xuanwumon
                itemName = "Ebonwumon Core";
                digimonName = "Ebonwumon";
                digimonID = 650;
            }

            // Procurando o item
            foreach (Item item in sender.Tamer.Items)
                if (item != null && item.ItemName == itemName && item.ItemQuant >= reqItemQntd)
                {
                    //Debug.Print("Achou o item");
                    // Procurando o Digimon
                    foreach (Digimon digimon in sender.Tamer.Digimon)
                        if (digimon != null && digimon.Id == ID)
                        {
                            //Debug.Print("Achou o digimon");

                            if (digimon.Level < 100)
                            {
                                Utils.Comandos.Send(sender, "Require Digimon Level 100!");
                                r = 0;
                            }
                            else
                            {
                                Utils.Comandos.SendGM("Yggdrasil", digimon.Tamer.Name + " got an successfull special evolution to " + digimonName + "! Congratulations!");

                                digimon.ResetStatus();
                                digimon.NewDigievolutionRookie(5, digimonID);

                                // Descontando itens
                                sender.Tamer.RemoveItem(item.ItemName, reqItemQntd);
                                sender.Tamer.AtualizarInventario();
                                r = 3;
                            }
                            break;
                        }

                    break;
                }

            // respondendo o client
            sender.Connection.Send(new Packets.PACKET_EVOLUTION_BEASTS(type, r, ID, (short)digimonID));
        }
    }
}
