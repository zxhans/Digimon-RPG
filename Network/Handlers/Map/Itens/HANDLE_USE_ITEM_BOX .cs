using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    //este handler eh pra receber o pacote de usar um item do tipo box especifico, mas nao consegui
    //enviar o pacote certo, entao desisti
    // Pacote recebido ao finalizar o login no map. Requer uma resposta do mesmo tipo vazia, PACKET_CCF0
    [PacketHandler(Type = PacketType.PACKET_USE_ITEM_BOX, Connection = ConnectionType.Map)]
    public class HANDLE_USE_ITEM_BOX : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Lixo
            int unk = packet.ReadInt();
            int unk2 = packet.ReadShort();

            // Operação
            int op = packet.ReadInt();

            int item_inv_db_id = packet.ReadInt();

            // ID do item no Banco
            int itemId = packet.ReadInt();

            Console.WriteLine("itemid " + itemId);
            Console.WriteLine("item_inv_db_id " + item_inv_db_id);
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando o item

            ////////
            ///nao está funcionando!!
            sender.Connection.Send(new Packets.PACKET_USE_ITEM_BOX(1, op));

            foreach (Item item in sender.Tamer.Items)
            {
                if (item != null && item.Id == item_inv_db_id && item.ItemQuant > 0)
                {

                    if (itemId == 23068)  //EXCALIBUR
                    {
                        if (sender.Tamer.CardSpace() > 0)
                        {
                            Utils.Comandos.Send(sender, "Recebeu x100 ExcaliburX");
                            sender.Tamer.AddCard("ExcaliburX", 100, false);
                            //sender.Tamer.RemoveItem("ExcaliburXBoxCoin", 1);
                            sender.Tamer.RemoveItem(item.ItemName, 1);
                            sender.Tamer.AtualizarInventario();
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Mochila de Cards cheia!");
                        }
                        return;
                    }
                    else if (itemId == 23067)  //acc chipbox
                    {
                        if (sender.Tamer.CardSpace() > 0)
                        {
                            Utils.Comandos.Send(sender, "Recebeu x100 AccChipX");
                            sender.Tamer.AddCard("AccChipX", 100, false);
                            //sender.Tamer.RemoveItem("AccChipXBoxCoin", 1);
                            sender.Tamer.RemoveItem(item.ItemName, 1);
                            sender.Tamer.AtualizarInventario();
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Mochila de Cards cheia!");
                        }
                        return;
                    }
                    else if (itemId == 22020)  //dmg drill
                    {
                        if (sender.Tamer.CardSpace() > 0)
                        {
                            Utils.Comandos.Send(sender, "Recebeu x22 DMG Drill");
                            sender.Tamer.AddCard("DMG Drill", 22, false);
                            //sender.Tamer.RemoveItem("AccChipXBoxCoin", 1);
                            sender.Tamer.RemoveItem(item.ItemName, 1);
                            sender.Tamer.AtualizarInventario();
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Mochila de Cards cheia!");
                        }
                        return;
                    }
                    else if (itemId == 22064)  //force box
                    {
                        if (sender.Tamer.CardSpace() > 0)
                        {
                            Random r = new Random();
                            int random = r.Next(100);
                            if (random < 50)
                            {
                                Utils.Comandos.Send(sender, "Recebeu x22 Fang Card");
                                sender.Tamer.AddCard("FangCard", 22, false);
                            }
                            else if (random < 80)
                            {
                                Utils.Comandos.Send(sender, "Recebeu x22 Exa Card");
                                sender.Tamer.AddCard("ExaCard", 22, false);
                            }
                            else
                            {
                                Utils.Comandos.Send(sender, "Recebeu x22 Royal Knights Force");
                                sender.Tamer.AddCard("RoyalCard", 22, false);
                            }
                            sender.Tamer.RemoveItem(item.ItemName, 1);
                            sender.Tamer.AtualizarInventario();
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Mochila de Cards cheia!");
                        }
                        return;
                    }
                    else
                    {
                        Utils.Comandos.Send(sender, "Not working now!");
                    }
                    break;
                }
            }
        }
    }
}
