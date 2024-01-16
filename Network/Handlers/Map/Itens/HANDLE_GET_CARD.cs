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
    // Client está executando operações com itens
    [PacketHandler(Type = PacketType.PACKET_GET_CARD, Connection = ConnectionType.Map)]
    public class HANDLE_GET_CARD : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            string unk = packet.ReadString(6);

            byte op = packet.ReadByte(); // Operação executada
                                         // 1 - Pegando card no chão
                                         // 2 - Mudando CardTamer de Slot
                                         // 3 - Equipando card

            byte slot = packet.ReadByte(); // 2 - Slot Tamer

            short unk5 = packet.ReadShort();

            // Posição do Card - Caso seja uma operação 3, X e Y passam a representar a linha e coluna no inventário
            int X = packet.ReadInt();
            int Y = packet.ReadInt();

            // Linha e coluna, em caso de estarmos desequipando um Card, outro trocando um Card de lugar
            int dLinha = packet.ReadInt();
            int dColuna = packet.ReadInt();

            // Identificador do Card - Se não for operação 1, este será o ID do Card no banco
            int ID = packet.ReadInt();

            // Informação entre um Card e outro
            string trash = packet.ReadString(60);

            // ID do segundo Card. útil em caso de troca de posição de Cards
            int ID2 = packet.ReadInt();

            // O pacote ainda possui muito mais informação. A partir do identificador do card, vem todas as
            // informações do card. Contudo, o que nos interessa aqui é apenas o identificador. Logo, podemos
            // descartar o restante das informações.
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Card pego no chão
            if(op == 1)
            {
                // Agora verificamos se o item está mesmo no mapa, e se o client tem direito ao drop
                MapZone Map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                if (Map != null)
                    foreach (ItemMap item in Map.Items)
                        if (item.Item.Id == ID && (item.Dono == sender.Tamer.Id || item.Livre
                            || (sender.Tamer.Party != null && sender.Tamer.Party.CheckTamer(item.Dono))))
                        {
                            if (sender.Tamer.CardSpace(item.Item.ItemName, item.Item.ItemQuant))
                            {
                                Item i = sender.Tamer.AddCard(item.Item.ItemName, item.Item.ItemQuant, true);
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(i, item.Location));
                                sender.Connection.Send(new Packets.PACKET_CARD_DROP((short)item.Location.X
                                    , (short)item.Location.Y));
                                Map.Items.Remove(item);
                                Map.sendItem(sender);
                            }
                            return;
                        }
            }

            // Equipando Card / Trocando Card de slot / Depósito na Warehouse
            else if(op == 3)
            {
                // Primeiro, vamos procurar o Card no inventário
                for (int i = 0; i < sender.Tamer.Cards.Length; i++)
                {
                    Item item = sender.Tamer.Cards[i];
                    if (item != null && item.Id == ID)
                    {
                        // Warehouse
                        if (slot == 10)
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.WareCards[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            if (dItem == null)
                            {
                                sender.Tamer.WareCards[eSlot] = item;
                                sender.Tamer.Cards[i] = null;
                                item.Slot = eSlot + 1;
                                item.SetWarehouse(1);
                                item.Save(sender.Tamer.Id);
                            }
                            // Há item no slot de destino
                            else if (dItem != null)
                            {
                                // Se os itens são diferentes, devemos apenas efetuar a troca
                                if (item.ItemId != dItem.ItemId)
                                {
                                    sender.Tamer.WareCards[eSlot] = item;
                                    sender.Tamer.Cards[i] = dItem;
                                    item.Slot = eSlot + 1;
                                    dItem.Slot = i + 1;
                                    item.SetWarehouse(1);
                                    dItem.SetWarehouse(0);
                                    item.Save(sender.Tamer.Id);
                                    dItem.Save(sender.Tamer.Id);
                                }
                                // Se os itens são iguais, devemos somar a quantidade de origem no destino
                                else
                                {
                                    // Claro, se o item destino já estiver com quantidade completa, não faz sentido
                                    if (dItem.ItemQuant < dItem.ItemQuantMax)
                                    {
                                        int espaco = dItem.ItemQuantMax - dItem.ItemQuant;

                                        if (item.ItemQuant > espaco)
                                        {
                                            item.ItemQuant -= espaco;
                                            item.Save(sender.Tamer.Id);
                                        }
                                        else
                                        {
                                            espaco = item.ItemQuant;
                                            item.Delete();
                                            item = null;
                                            sender.Tamer.Cards[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (item != null)
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(item, dItem, op, slot, X, Y
                                    , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();
                        }
                        // Equipando item
                        if (slot == 2)
                        {
                            // Agora vamos procurar o slot onde o item será equipado. Isso é definido pela TAG
                            int eSlot = (int)Enums.EquipSlots.card1 + dColuna;

                            // Vamos guardar o card que já estava no slot
                            Item temp = sender.Tamer.Cards[eSlot - 1];
                            int old_linha = item.GetLinha();
                            int old_coluna = item.GetColuna();

                            // Efetuando a troca
                            sender.Tamer.Cards[eSlot - 1] = item;
                            item.Slot = eSlot;
                            sender.Tamer.Cards[i] = temp;
                            item.Save(sender.Tamer.Id);

                            if (temp != null)
                            {
                                temp.Slot = i + 1;
                                temp.Save(sender.Tamer.Id);

                                sender.Connection.Send(new Packets.PACKET_GET_CARD(item, temp, op, slot, X, Y
                                    , dLinha, dColuna));
                            } else

                            // Respondendo o Client
                            sender.Connection.Send(new Packets.PACKET_GET_CARD(item, op, slot, old_linha
                                , old_coluna, dColuna));

                            sender.Tamer.AtualizarInventario();
                        }
                        // Mudando item de lugar
                        else if (slot == 3)
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.Cards[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            Debug.Print("ID2: {0}, dItem: {1}", ID2, dItem);
                            if (ID2 == 0 && dItem == null)
                            {
                                sender.Tamer.Cards[eSlot] = item;
                                sender.Tamer.Cards[i] = null;
                                item.Slot = eSlot + 1;
                                item.Save(sender.Tamer.Id);
                            }
                            // Há item no slot de destino
                            else if (dItem != null && ID2 != 0)
                            {
                                // Se os itens são diferentes, devemos apenas efetuar a troca
                                if(item.ItemId != dItem.ItemId)
                                {
                                    sender.Tamer.Cards[eSlot] = item;
                                    sender.Tamer.Cards[i] = dItem;
                                    item.Slot = eSlot + 1;
                                    dItem.Slot = i + 1;
                                    item.Save(sender.Tamer.Id);
                                    dItem.Save(sender.Tamer.Id);
                                }
                                // Se os itens são iguais, devemos somar a quantidade de origem no destino
                                else
                                {
                                    // Claro, se o item destino já estiver com quantidade completa, não faz sentido
                                    if(dItem.ItemQuant < dItem.ItemQuantMax)
                                    {
                                        int espaco = dItem.ItemQuantMax - dItem.ItemQuant;
                                        
                                        if(item.ItemQuant > espaco)
                                        {
                                            item.ItemQuant -= espaco;
                                            item.Save(sender.Tamer.Id);
                                        }
                                        else
                                        {
                                            espaco = item.ItemQuant;
                                            item.Delete();
                                            item = null;
                                            sender.Tamer.Cards[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (ID2 == 0) dItem = null;
                            if (item != null)
                            sender.Connection.Send(new Packets.PACKET_GET_CARD(item, dItem, op, slot, X, Y
                                , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();

                        }

                        return;
                    }
                }
            }

            // Desequipando o Card
            else if (op == 2)
            {
                // Primeiro, vamos procurar o item no inventário
                for (int i = 0; i < sender.Tamer.Cards.Length; i++)
                {
                    Item item = sender.Tamer.Cards[i];
                    if (item != null && item.Id == ID)
                    {
                        // Slot onde o item será depositado
                        int eSlot = dColuna + (dLinha * 6);
                        // O Card está sendo depositado em outro Slot de Equipamento
                        if(slot == 2)
                        {
                            eSlot = (int)EquipSlots.card1 + dColuna - 1;
                        }
                        //  O slot está vazio
                        if (sender.Tamer.Cards[eSlot] == null)
                        {
                            // Efetuando a troca
                            sender.Tamer.Cards[eSlot] = item;
                            sender.Tamer.Cards[i] = null;
                            item.Slot = eSlot + 1;
                            item.Save(sender.Tamer.Id);

                            // Respondendo o Client
                            sender.Connection.Send(new Packets.PACKET_GET_CARD(item, op, slot, 0, Y, dLinha
                                , dColuna));
                        }
                        else
                        {
                            Item dItem = sender.Tamer.Cards[eSlot];

                            // Efetuando a troca
                            sender.Tamer.Cards[eSlot] = item;
                            item.Slot = eSlot + 1;
                            item.Save(sender.Tamer.Id);

                            sender.Tamer.Cards[i] = dItem;
                            dItem.Slot = i + 1;
                            dItem.Save(sender.Tamer.Id);

                            sender.Connection.Send(new Packets.PACKET_GET_CARD(item, dItem, op, slot, X, Y
                                , dLinha, dColuna));
                        }

                        sender.Tamer.AtualizarInventario();

                        return;
                    }
                }
            }

            // Retirada de Card da Warehouse
            else if (op == 10)
            {
                // Primeiro, vamos procurar o item no inventário do Warehouse
                for (int i = 0; i < sender.Tamer.WareCards.Length; i++)
                {
                    Item item = sender.Tamer.WareCards[i];
                    if (item != null && item.Id == ID)
                    {
                        // Warehouse
                        if (slot == 3)
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.Cards[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            if (ID2 == 0)
                            {
                                sender.Tamer.Cards[eSlot] = item;
                                sender.Tamer.WareCards[i] = null;
                                item.Slot = eSlot + 1;
                                item.SetWarehouse(0);
                                item.Save(sender.Tamer.Id);
                            }
                            // Há item no slot de destino
                            else if (dItem != null && ID2 != 0)
                            {
                                // Se os itens são diferentes, devemos apenas efetuar a troca
                                if (item.ItemId != dItem.ItemId)
                                {
                                    sender.Tamer.Cards[eSlot] = item;
                                    sender.Tamer.WareCards[i] = dItem;
                                    item.Slot = eSlot + 1;
                                    dItem.Slot = i + 1;
                                    item.SetWarehouse(0);
                                    dItem.SetWarehouse(1);
                                    item.Save(sender.Tamer.Id);
                                    dItem.Save(sender.Tamer.Id);
                                }
                                // Se os itens são iguais, devemos somar a quantidade de origem no destino
                                else
                                {
                                    // Claro, se o item destino já estiver com quantidade completa, não faz sentido
                                    if (dItem.ItemQuant < dItem.ItemQuantMax)
                                    {
                                        int espaco = dItem.ItemQuantMax - dItem.ItemQuant;

                                        if (item.ItemQuant > espaco)
                                        {
                                            item.ItemQuant -= espaco;
                                            item.Save(sender.Tamer.Id);
                                        }
                                        else
                                        {
                                            espaco = item.ItemQuant;
                                            item.Delete();
                                            item = null;
                                            sender.Tamer.Cards[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (item != null)
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(item, dItem, op, slot, X, Y
                                    , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();

                            return;
                        }

                        // Mudando item de lugar
                        else if (slot == 10)
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.WareCards[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            if (dItem == null)
                            {
                                sender.Tamer.WareCards[eSlot] = item;
                                sender.Tamer.WareCards[i] = null;
                                item.Slot = eSlot + 1;
                                item.Save(sender.Tamer.Id);
                            }
                            // Há item no slot de destino
                            else if (dItem != null)
                            {
                                // Se os itens são diferentes, devemos apenas efetuar a troca
                                if (item.ItemId != dItem.ItemId)
                                {
                                    sender.Tamer.WareCards[eSlot] = item;
                                    sender.Tamer.WareCards[i] = dItem;
                                    item.Slot = eSlot + 1;
                                    dItem.Slot = i + 1;
                                    item.Save(sender.Tamer.Id);
                                    dItem.Save(sender.Tamer.Id);
                                }
                                // Se os itens são iguais, devemos somar a quantidade de origem no destino
                                else
                                {
                                    // Claro, se o item destino já estiver com quantidade completa, não faz sentido
                                    if (dItem.ItemQuant < dItem.ItemQuantMax)
                                    {
                                        int espaco = dItem.ItemQuantMax - dItem.ItemQuant;

                                        if (item.ItemQuant > espaco)
                                        {
                                            item.ItemQuant -= espaco;
                                            item.Save(sender.Tamer.Id);
                                        }
                                        else
                                        {
                                            espaco = item.ItemQuant;
                                            item.Delete();
                                            item = null;
                                            sender.Tamer.WareCards[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (item != null)
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(item, dItem, op, slot, X, Y
                                    , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_CARD(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();

                            return;
                        }
                    }
                }
            }
        }

        private int GetInternalSlot(int slot)
        {
            int eSlot = 0;
            switch (slot)
            {
                case 4:
                    eSlot = (int)EquipSlots.crest1;
                    break;
                case 5:
                    eSlot = (int)EquipSlots.crest2;
                    break;
                case 6:
                    eSlot = (int)EquipSlots.crest3;
                    break;
                case 7:
                    eSlot = (int)EquipSlots.digiegg1;
                    break;
                case 8:
                    eSlot = (int)EquipSlots.digiegg2;
                    break;
                case 9:
                    eSlot = (int)EquipSlots.digiegg3;
                    break;
                case 11:
                    eSlot = (int)EquipSlots.aura;
                    break;
                case 12:
                    eSlot = (int)EquipSlots.digivice;
                    break;
                case 14:
                    eSlot = (int)EquipSlots.sock;
                    break;
                case 15:
                    eSlot = (int)EquipSlots.shoes;
                    break;
                case 16:
                    eSlot = (int)EquipSlots.pants;
                    break;
                case 17:
                    eSlot = (int)EquipSlots.glove;
                    break;
                case 18:
                    eSlot = (int)EquipSlots.tshirt;
                    break;
                case 19:
                    eSlot = (int)EquipSlots.jacket;
                    break;
                case 20:
                    eSlot = (int)EquipSlots.hat;
                    break;
                case 21:
                    eSlot = (int)EquipSlots.customer;
                    break;
                case 22:
                    eSlot = (int)EquipSlots.earring;
                    break;
                case 23:
                    eSlot = (int)EquipSlots.necklace;
                    break;
                case 24:
                    eSlot = (int)EquipSlots.ring;
                    break;
            }
            return eSlot;
        }

    }
}
