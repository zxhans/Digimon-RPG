using System;
using System.Collections.Generic;
using System.IO;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Este é apenas um Writer de pacotes com estrutura de digimons, só para não ficar repetindo a estrutura.
    public class PACKET_NPC_ITEM_SHOP_WRITER
    {
        // Listando o shop
        public void WriteItems(string npc, Client sender)
        {
            // Obtendo os itens deste NPC no banco
            StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>("items as value"
                , "npc_item_shop", "WHERE name=@name"
                , new Database.QueryParameters() { { "name", npc } });
            List<Item> itens = new List<Item>();
            if (result.HasRows)
            {
                string[] split = result.value.Split('-');
                foreach (string name in split)
                {
                    ItemCodex item = Emulator.Enviroment.Codex[name];
                    if (item != null)
                        itens.Add(item.GetItem());
                }
            }

            // Respondendo o client
            OutPacket p = new OutPacket(PacketType.PACKET_NPC_ITEM_SHOP);
            p.Write(new byte[6]);
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
            for (int i = 0; i < 24; i++)
            {
                if (itens.Count > i)
                    itemWriter.WriteItem(itens[i], p);
                else
                    itemWriter.WriteItem(null, p);
            }

            //Utils.Comandos.Send(sender, "[DEBUG]");

            sender.Connection.Send(p);
        }
        public void WriteCards(string npc, Client sender)
        {
            // Obtendo os itens deste NPC no banco
            StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>("cards as value"
                , "npc_card_shop", "WHERE name=@name"
                , new Database.QueryParameters() { { "name", npc } });
            List<Item> itens = new List<Item>();
            if (result.HasRows)
            {
                string[] split = result.value.Split('-');
                foreach (string name in split)
                {
                    ItemCodex item = Emulator.Enviroment.Codex[name];
                    if (item != null)
                        itens.Add(item.GetItem());
                }
            }

            // Respondendo o client
            OutPacket p = new OutPacket(PacketType.PACKET_NPC_CARD_SHOP);
            p.Write(new byte[6]);
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
            for (int i = 0; i < 24; i++)
            {
                if (itens.Count > i)
                    itemWriter.WriteCard(itens[i], p);
                else
                    itemWriter.WriteCard(null, p);
            }

            sender.Connection.Send(p);
        }

        // Comprando itens
        public void BuyItem(string npc, int npcId, int id, int quant, Client sender)
        {
            // Obtendo os itens deste NPC no banco
            StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>("items as value"
                , "npc_item_shop", "WHERE name=@name"
                , new Database.QueryParameters() { { "name", npc } });

            if (result.HasRows)
            {
                string[] split = result.value.Split('-');
                foreach (string name in split)
                {
                    ItemCodex item = Emulator.Enviroment.Codex[name];
                    if (item != null)
                    {
                        Item i = item.GetItem();
                        if(i != null && i.Id == id)
                        {
                            // O tamer tem bits suficientes?
                            if (sender != null && sender.Tamer != null
                                && sender.Tamer.Bits >= quant * i.Custo)
                            {
                                // Processando compra e respondendo o client
                                // Iniciando pacote
                                OutPacket p = new OutPacket(PacketType.PACKET_NPC_ITEM_SHOP_OP);
                                p.Write(new byte[6]);
                                p.Write(1);
                                // Adicionando o item
                                PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
                                Item n = sender.Tamer.AddItem(name, quant, false);
                                itemWriter.WriteItem(n, p);
                                // Descontando bits
                                sender.Tamer.GainBit(-quant * i.Custo);
                                // Finalizando pacote
                                p.Write(Utils.StringHex.Hex2Binary("01 00 00 00 00 00 00 00"));
                                p.Write((double)sender.Tamer.Bits);
                                p.Write(Utils.StringHex.Hex2Binary("00 00"));
                                p.Write((short)3);
                                p.Write(n.GetColuna());
                                p.Write(n.GetLinha());
                                p.Write(npcId);

                                sender.Connection.Send(p);
                                sender.Tamer.AtualizarInventario();
                            }

                            return;
                        }
                    }
                }
            }
        }

        public void BuyItemWithCoin(string npc, int npcId, int id, int quant, Client sender)
        {
            // Obtendo os itens deste NPC no banco
            StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>("items as value"
                , "npc_item_shop", "WHERE name=@name"
                , new Database.QueryParameters() { { "name", npc } });

            if (result.HasRows)
            {
                string[] split = result.value.Split('-');
                foreach (string name in split)
                {
                    ItemCodex item = Emulator.Enviroment.Codex[name];
                    if (item != null)
                    {
                        Item i = item.GetItem();
                        if (i != null && i.Id == id)
                        {
                            // O tamer tem bits suficientes?
                            if (sender != null && sender.Tamer != null
                                && sender.Tamer.Coin >= quant * i.Custo)
                            {
                                // Processando compra e respondendo o client
                                // Iniciando pacote
                                OutPacket p = new OutPacket(PacketType.PACKET_NPC_ITEM_SHOP_OP);
                                p.Write(new byte[6]);
                                p.Write(4);
                                // Adicionando o item
                                PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
                                Item n = sender.Tamer.AddItem(name, quant, false);
                                itemWriter.WriteItem(n, p);
                                // Descontando bits
                                sender.Tamer.GainCoin(-quant * i.Custo);
                                // Finalizando pacote
                                p.Write(Utils.StringHex.Hex2Binary("01 00 00 00 00 00 00 00"));
                                p.Write((double)sender.Tamer.Bits);
                                p.Write(sender.Tamer.Coin);
                                p.Write((short)3);
                                p.Write(n.GetColuna());
                                p.Write(n.GetLinha());
                                p.Write(npcId);

                                sender.Connection.Send(p);
                                sender.Tamer.AtualizarInventario();
                            }

                            return;
                        }
                    }
                }
            }
        }
        public void BuyCard(string npc, int npcId, int id, int quant, Client sender)
        {
            // Obtendo os itens deste NPC no banco
            StringValueResult result = Emulator.Enviroment.Database.Select<StringValueResult>("cards as value"
                , "npc_card_shop", "WHERE name=@name"
                , new Database.QueryParameters() { { "name", npc } });

            if (result.HasRows)
            {
                string[] split = result.value.Split('-');
                foreach (string name in split)
                {
                    ItemCodex item = Emulator.Enviroment.Codex[name];
                    if (item != null)
                    {
                        Item i = item.GetItem();
                        if (i.Id == id)
                        {
                            // O tamer tem bits suficientes?
                            if (sender.Tamer.Bits >= quant * i.Custo)
                            {
                                // Processando compra e respondendo o client
                                // Iniciando pacote
                                OutPacket p = new OutPacket(PacketType.PACKET_NPC_CARD_SHOP_OP);
                                p.Write(new byte[6]);
                                p.Write(1);
                                // Adicionando o item
                                PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
                                Item n = sender.Tamer.AddCard(name, quant, false);
                                itemWriter.WriteCard(n, p);
                                // Descontando bits
                                sender.Tamer.GainBit(-quant * i.Custo);
                                // Finalizando pacote
                                p.Write(Utils.StringHex.Hex2Binary("01 00 00 00"));
                                p.Write((double)sender.Tamer.Bits);
                                p.Write((short)3);
                                p.Write(Utils.StringHex.Hex2Binary("00 00"));
                                p.Write(n.GetColuna());
                                p.Write(n.GetLinha());
                                p.Write(npcId);

                                sender.Connection.Send(p);
                            }

                            return;
                        }
                    }
                }
            }
        }

        // Vendendo itens
        public void SellItem(int id, int quant, Client sender)
        {
            foreach(Item i in sender.Tamer.Items)
                if (i != null && i.Id == id && i.ItemQuant >= quant)
                {
                    // Processando venda e respondendo o client
                    // Iniciando pacote
                    OutPacket p = new OutPacket(PacketType.PACKET_NPC_ITEM_SHOP_OP);
                    p.Write(new byte[6]);
                    p.Write(2);
                    int linha = i.GetLinha();
                    int col = i.GetColuna();
                    // Acrescentando bits
                    sender.Tamer.GainBit(quant * (i.Custo / 2));
                    // Removendo o item
                    i.ItemQuant -= quant;
                    if (i.ItemQuant <= 0) i.Delete();

                    PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
                    itemWriter.WriteItem(i, p);
                    // Finalizando pacote
                    p.Write(Utils.StringHex.Hex2Binary("01 00 00 00 00 00 00 00"));
                    p.Write((double)sender.Tamer.Bits);
                    p.Write(Utils.StringHex.Hex2Binary("00 00"));
                    p.Write((short)3);
                    p.Write(col);
                    p.Write(linha);
                    p.Write(0);

                    sender.Connection.Send(p);

                    return;
                }
        }
        public void SellCard(int id, int quant, Client sender)
        {
            foreach (Item i in sender.Tamer.Cards)
                if (i != null && i.Id == id && i.ItemQuant >= quant)
                {
                    // Processando venda e respondendo o client
                    // Iniciando pacote
                    OutPacket p = new OutPacket(PacketType.PACKET_NPC_CARD_SHOP_OP);
                    p.Write(new byte[6]);
                    p.Write(2);
                    int linha = i.GetLinha();
                    int col = i.GetColuna();
                    // Acrescentando bits
                    sender.Tamer.GainBit(quant * (i.Custo / 2));
                    // Removendo o item
                    i.ItemQuant -= quant;
                    if (i.ItemQuant <= 0) i.Delete();

                    PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
                    itemWriter.WriteCard(i, p);
                    // Finalizando pacote
                    p.Write(Utils.StringHex.Hex2Binary("01 00 00 00"));
                    p.Write((double)sender.Tamer.Bits);
                    p.Write((short)3);
                    p.Write(Utils.StringHex.Hex2Binary("00 00"));
                    p.Write(col);
                    p.Write(linha);
                    p.Write(0);

                    sender.Connection.Send(p);

                    return;
                }
        }
    }
}
