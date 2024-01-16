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
    // Tamer solicitou troca com outro tamer
    [PacketHandler(Type = PacketType.PACKET_TRADE, Connection = ConnectionType.Map)]
    public class HANDLE_TRADE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6);

            // GUID do solicitante
            long GUID = packet.ReadLong();
            string GUIDSufix = packet.ReadString(8);

            byte op = packet.ReadByte(); // Operação realizada
            byte unk = packet.ReadByte();

            // Se o client que enviou este pacote não está em trade, nada pode continuar
            if (sender.Trade != null)
            {
                // Lendo e validando a lista de Cards
                byte[] card_pos = packet.ReadBytes(10);
                for (int i = 0; i < 10; i++)
                {
                    int ID = packet.ReadInt(); // ID do card
                    trash = packet.ReadBytes(8);
                    byte quant = packet.ReadByte(); // Quantidade inserida na troca

                    // Restante da estrutura do card
                    trash = packet.ReadBytes(51);

                    // Procurando o card no inventário do client
                    foreach (Item item in sender.Tamer.Cards)
                        if (item != null && item.Id == ID)
                        {
                            if (item.ItemQuant >= quant)
                            {
                                item.TradeQuant = quant;
                                if (sender.Trade.Client == sender)
                                    sender.Trade.Cards[i] = item;
                                else if (sender.Trade.Client2 == sender)
                                    sender.Trade.Cards2[i] = item;
                            }
                            break;
                        }
                }

                // Lendo e validando a lista de Itens
                byte[] item_pos = packet.ReadBytes(12);
                for (int i = 0; i < 10; i++)
                {
                    int ID = packet.ReadInt(); // ID do item
                    trash = packet.ReadBytes(8);
                    int quant = packet.ReadInt(); // Quantidade inserida na troca

                    // Restante da estrutura do item
                    trash = packet.ReadBytes(84);

                    // Procurando o item no inventário do client
                    foreach (Item item in sender.Tamer.Items)
                        if (item != null && item.Id == ID)
                        {
                            if (item.ItemQuant >= quant)
                            {
                                item.TradeQuant = quant;
                                if (sender.Trade.Client == sender)
                                    sender.Trade.Items[i] = item;
                                else if (sender.Trade.Client2 == sender)
                                    sender.Trade.Items2[i] = item;
                            }
                            break;
                        }
                }

                //Client a ser respondido
                Client c = sender;

                // Lendo e validando os Bits
                double bits = packet.ReadDouble();

                if (sender.Trade.Client == sender)
                {
                    c = sender.Trade.Client2;
                    if (bits <= sender.Tamer.Bits)
                        sender.Trade.Bits = bits;
                }
                else if (sender.Trade.Client2 == sender)
                {
                    c = sender.Trade.Client;
                    if (bits <= sender.Tamer.Bits)
                        sender.Trade.Bits2 = bits;
                }
                // Processamento e resposta
                switch (op)
                {
                    default:
                        c.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        sender.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        break;
                    // Primeiro OK
                    case 1:
                        sender.confirmaTrade = 1;
                        c.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        sender.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        break;
                    // Trade confirmado
                    case 3:
                        // O outro Tamer também confirmou?
                        if(c.confirmaTrade == 6)
                        {
                            // Efetuando a troca
                            byte res = sender.Trade.Trocar();
                            c.Connection.Send(new Packets.PACKET_TRADE(sender, res, card_pos, item_pos));
                            sender.Connection.Send(new Packets.PACKET_TRADE(sender, res, card_pos, item_pos));
                            sender.Trade.Cancelar();
                            sender.Tamer.AtualizarInventario();
                            c.Tamer.AtualizarInventario();
                        }
                        break;
                    // Trade cancelado
                    case 4:
                        c.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        sender.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        sender.Trade.Cancelar();
                        sender.Tamer.AtualizarInventario();
                        c.Tamer.AtualizarInventario();
                        break;
                    // Última confirmação
                    case 6:
                        sender.confirmaTrade = 6;
                        c.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        sender.Connection.Send(new Packets.PACKET_TRADE(sender, op, card_pos, item_pos));
                        break;
                }
            }

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();
        }
    }
}
