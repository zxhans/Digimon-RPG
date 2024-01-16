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
    [PacketHandler(Type = PacketType.PACKET_GET_ITEM, Connection = ConnectionType.Map)]
    public class HANDLE_GET_ITEM : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            string unk = packet.ReadString(6);

            byte op = packet.ReadByte(); // Operação executada
                                         // 1 - Pegando item no chão
                                         // 3 - Equipando item

            byte slot = packet.ReadByte(); // Slot onde o tem foi equipado, se houver.
                                           // Este valor não bate com os valores internos. Logo, vamos precisar
                                           // Fazer um Switch Case para encontrar o slot interno, e devemos usar
                                           // Este mesmo valor para responder o client.

            short unk5 = packet.ReadShort();

            // Posição do Item - Caso seja uma operação 3, X e Y passam a representar a linha e coluna no inventário
            int X = packet.ReadInt();
            int Y = packet.ReadInt();

            // Linha e coluna, em caso de estarmos desequipando um item, outro trocando um item de lugar
            int dLinha = packet.ReadInt();
            int dColuna = packet.ReadInt();

            // Identificador do item - Se não for operação 1, este será o ID do item no banco
            int ID = packet.ReadInt();

            // Informação entre um item e outro
            string trash = packet.ReadString(96);

            // ID do segundo item. útil em caso de troca de posição de itens
            int ID2 = packet.ReadInt();

            // O pacote ainda possui muito mais informação. A partir do identificador do item, vem todas as
            // informações do item. Contudo, o que nos interessa aqui é apenas o identificador. Logo, podemos
            // descartar o restante das informações.
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Utils.Comandos.Send(sender, "Movendo  " + dLinha + " " + dColuna + " " + X + " " + Y + " " + slot + " " + ID);

            /*
            #if defined(ADD_INVENTORY_EXPANSION)
            UP_CARD_BAG_EQUIP_01	= 31,
            UP_CARD_BAG_EQUIP_02	= 32,
            UP_CARD_BAG_EQUIP_03	= 33,
            UP_CARD_BAG_EQUIP_04	= 34,
            UP_CARD_BAG_EQUIP_05	= 35,
            UP_CARD_BAG_EQUIP_06	= 36,
            UP_CARD_BAG_EQUIP_07	= 37,
            UP_CARD_BAG_INSIDE_01	= 41,
            UP_CARD_BAG_INSIDE_02	= 42,
            UP_CARD_BAG_INSIDE_03	= 43,
            UP_CARD_BAG_INSIDE_04	= 44,
            UP_CARD_BAG_INSIDE_05	= 45,
            UP_CARD_BAG_INSIDE_06	= 46,
            UP_CARD_BAG_INSIDE_07	= 47,
            UP_ITEM_BAG_EQUIP_01	= 51,
            UP_ITEM_BAG_EQUIP_02	= 52,
            UP_ITEM_BAG_EQUIP_03	= 53,
            UP_ITEM_BAG_EQUIP_04	= 54,
            UP_ITEM_BAG_EQUIP_05	= 55,
            UP_ITEM_BAG_EQUIP_06	= 56,
            UP_ITEM_BAG_EQUIP_07	= 57,
            UP_ITEM_BAG_INSIDE_01	= 61,
            UP_ITEM_BAG_INSIDE_02	= 62,
            UP_ITEM_BAG_INSIDE_03	= 63,
            UP_ITEM_BAG_INSIDE_04	= 64,
            UP_ITEM_BAG_INSIDE_05	= 65,
            UP_ITEM_BAG_INSIDE_06	= 66,
            UP_ITEM_BAG_INSIDE_07	= 67,
            #endif	// #if defined(ADD_INVENTORY_EXPANSION)
             * */

            // Item pego no chão
            if (op == 1)
            {
                // Agora verificamos se o item está mesmo no mapa, e se o client tem direito ao drop
                MapZone Map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                if (Map != null)
                    foreach (ItemMap item in Map.Items)
                        if (item != null && item.Item.Id == ID && (item.Dono == sender.Tamer.Id || item.Livre
                            || (sender.Tamer.Party != null && sender.Tamer.Party.CheckTamer(item.Dono))))
                        {
                            if (sender.Tamer.ItemSpace(item.Item.ItemName, item.Item.ItemQuant))
                            {
                                Item i = sender.Tamer.AddItem(item.Item.ItemName, item.Item.ItemQuant, true);
                                if (i != null)
                                {
                                    sender.Connection.Send(new Packets.PACKET_GET_ITEM(i, item.Location));
                                    Map.removeItem(item.Location);
                                    Map.Items.Remove(item);
                                    Map.sendItem(sender);
                                }
                            }
                            return;
                        }
            }

            // Equipando item / Trocando item de slot / Deposito na Warehouse
            else if(op == 3)
            {
                // Primeiro, vamos procurar o item no inventário
                for (int i = 0; i < sender.Tamer.Items.Length; i++)
                {
                    Item item = sender.Tamer.Items[i];
                    if (item != null && item.Id == ID)
                    {
                        // Warehouse
                        if (slot == 10)
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.WareItems[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            if (dItem == null)
                            {
                                sender.Tamer.WareItems[eSlot] = item;
                                sender.Tamer.Items[i] = null;
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
                                    sender.Tamer.WareItems[eSlot] = item;
                                    sender.Tamer.Items[i] = dItem;
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
                                            sender.Tamer.Items[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (item != null)
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(item, dItem, op, slot, X, Y
                                    , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();
                        }
                        // Equipando item
                        else if (slot != 3 && slot != 61)
                        {
                            Debug.Print("Slot:" + slot);
                            // Agora vamos procurar o slot onde o item será equipado. Isso é definido pela TAG
                            int eSlot = GetInternalSlot(slot);
                            Debug.Print("ESlot:" + eSlot);
                            if (eSlot != 0)
                            {
                                // Vamos guardar o item que já estava no slot
                                Item temp = sender.Tamer.Items[eSlot - 1];
                                int old_linha = item.GetLinha();
                                int old_coluna = item.GetColuna();

                                // Efetuando a troca
                                sender.Tamer.Items[eSlot - 1] = item;
                                item.Slot = eSlot;
                                sender.Tamer.Items[i] = temp;
                                item.Save(sender.Tamer.Id);

                                if (temp != null)
                                {
                                    temp.Slot = i + 1;
                                    temp.Save(sender.Tamer.Id);
                                }

                                // Respondendo o Client
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(item, temp, op, slot, X, Y
                                    , dLinha, dColuna));
                                sender.Tamer.AtualizarInventario();
                            }
                        }
                        // Mudando item de lugar
                        else
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.Items[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            if (ID2 == 0)
                            {
                                sender.Tamer.Items[eSlot] = item;
                                sender.Tamer.Items[i] = null;
                                item.Slot = eSlot + 1;
                                item.Save(sender.Tamer.Id);
                            }
                            // Há item no slot de destino
                            else if (dItem != null && ID2 != 0)
                            {
                                // Se os itens são diferentes, devemos apenas efetuar a troca
                                if(item.ItemId != dItem.ItemId)
                                {
                                    sender.Tamer.Items[eSlot] = item;
                                    sender.Tamer.Items[i] = dItem;
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
                                            sender.Tamer.Items[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (item != null)
                            sender.Connection.Send(new Packets.PACKET_GET_ITEM(item, dItem, op, slot, X, Y
                                , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();

                        }

                        return;
                    }
                }
            }

            // Retirada de item da Warehouse
            else if (op == 10)
            {
                // Primeiro, vamos procurar o item no inventário do Warehouse
                for (int i = 0; i < sender.Tamer.WareItems.Length; i++)
                {
                    Item item = sender.Tamer.WareItems[i];
                    if (item != null && item.Id == ID)
                    {
                        // Warehouse
                        if (slot == 3)
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.Items[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            if (ID2 == 0)
                            {
                                sender.Tamer.Items[eSlot] = item;
                                sender.Tamer.WareItems[i] = null;
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
                                    sender.Tamer.Items[eSlot] = item;
                                    sender.Tamer.WareItems[i] = dItem;
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
                                            sender.Tamer.Items[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (item != null)
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(item, dItem, op, slot, X, Y
                                    , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();

                            return;
                        }

                        // Mudando item de lugar
                        else if (slot == 10)
                        {
                            int eSlot = dColuna + (dLinha * 6);
                            Item dItem = sender.Tamer.WareItems[eSlot];
                            // Se o ID é zero, basta mudar a posição do item
                            // Slot onde o item será depositado
                            if (dItem == null)
                            {
                                sender.Tamer.WareItems[eSlot] = item;
                                sender.Tamer.WareItems[i] = null;
                                item.Slot = eSlot + 1;
                                item.Save(sender.Tamer.Id);
                            }
                            // Há item no slot de destino
                            else if (dItem != null)
                            {
                                // Se os itens são diferentes, devemos apenas efetuar a troca
                                if (item.ItemId != dItem.ItemId)
                                {
                                    sender.Tamer.WareItems[eSlot] = item;
                                    sender.Tamer.WareItems[i] = dItem;
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
                                            sender.Tamer.WareItems[i] = null;
                                        }
                                        dItem.ItemQuant += espaco;
                                        dItem.Save(sender.Tamer.Id);
                                    }
                                }
                            }

                            if (item != null)
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(item, dItem, op, slot, X, Y
                                    , dLinha, dColuna));
                            else
                                sender.Connection.Send(new Packets.PACKET_GET_ITEM(dItem, item, op, slot, X, Y
                                , dLinha, dColuna));

                            sender.Tamer.AtualizarInventario();

                            return;
                        }
                    }
                }
            }

            // Desequipando o item
            else if (op >= 4)
            {
                // Primeiro, vamos procurar o item no inventário
                for (int i = 0; i < sender.Tamer.Items.Length; i++)
                {
                    Item item = sender.Tamer.Items[i];
                    if (item != null && item.Id == ID)
                    {
                        // Slot onde o item será depositado
                        int eSlot = dColuna + (dLinha * 6);
                        //  O slot precisa estar vazio
                        if (sender.Tamer.Items[eSlot] == null)
                        {
                            // Efetuando a troca
                            sender.Tamer.Items[i] = null;
                            sender.Tamer.Items[eSlot] = item;
                            item.Slot = eSlot + 1;
                            item.Save(sender.Tamer.Id);

                            // Respondendo o Client
                            sender.Connection.Send(new Packets.PACKET_GET_ITEM(item, op, slot, 0, 0, dLinha
                                , dColuna));

                            sender.Tamer.AtualizarInventario();
                        }

                        return;
                    }
                }
            }

            // Opção ainda não mapeada?
            Debug.Print("GET_ITEM nova op: {0}, slot: {1}, x: {2}, y: {3}, l: {4}, c: {5}"
                , op, slot, X, Y, dLinha, dColuna);
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
                case 51:
                    eSlot = (int)EquipSlots.bagexp1;
                    break;
            }
            return eSlot;
        }

    }
}
