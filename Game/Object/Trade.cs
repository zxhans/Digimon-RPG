using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace Digimon_Project.Game
{
    // Classe que controla o processo de trade
    public class Trade
    {
        public Client Client { get; set; }
        public Client Client2 { get; set; }
        public Item[] Items, Items2, Cards, Cards2;
        public double Bits = 0, Bits2 = 0;

        // Iniciando a troca
        public Trade(Client solicitante, Client solicidato)
        {
            Client = solicitante;
            Client2 = solicidato;

            // Assimilando o trade aos clients
            Client.Trade = this;
            Client2.Trade = this;

            // Zerando aguardo de troca
            Client.Solicitado = null;
            Client2.Solicitado = null;

            // Iniciando vetores
            Items = new Item[10];
            Items2 = new Item[10];
            Cards = new Item[10];
            Cards2 = new Item[10];
        }

        // Função para efetuar a troca
        public byte Trocar()
        {
            // Itens e Cards
            // Verificando se há espaço
            int itens = 0;
            foreach (Item item in Items)
                if (item != null)
                    itens++;
            if (Client2.Tamer.ItemSpace() < itens)
                return 5;
            itens = 0;
            foreach (Item item in Cards)
                if (item != null)
                    itens++;
            if (Client2.Tamer.CardSpace() < itens)
                return 5;
            itens = 0;
            foreach (Item item in Items2)
                if (item != null)
                    itens++;
            if (Client.Tamer.ItemSpace() < itens)
                return 5;
            itens = 0;
            foreach (Item item in Cards2)
                if (item != null)
                    itens++;
            if (Client.Tamer.CardSpace() < itens)
                return 5;
            // Se chegou até aqui, ambos os Tamers tem espaço disponível para receber os itens da troca
            // Vamos efetuar a troca
            foreach(Item item in Items)
                if(item != null)
                {
                    Client2.Tamer.AddItem(item.ItemName, item.TradeQuant, false);
                    Client.Tamer.RemoveItem(item.ItemName, item.TradeQuant);
                }
            foreach (Item item in Cards)
                if (item != null)
                {
                    Client2.Tamer.AddCard(item.ItemName, item.TradeQuant, false);
                    Client.Tamer.RemoveCard(item.ItemName, (byte)item.TradeQuant);
                }
            foreach (Item item in Items2)
                if (item != null)
                {
                    Client.Tamer.AddItem(item.ItemName, item.TradeQuant, false);
                    Client2.Tamer.RemoveItem(item.ItemName, item.TradeQuant);
                }
            foreach (Item item in Cards2)
                if (item != null)
                {
                    Client.Tamer.AddCard(item.ItemName, item.TradeQuant, false);
                    Client2.Tamer.RemoveCard(item.ItemName, (byte)item.TradeQuant);
                }

            // Bits
            Client.Tamer.GainBit(-Bits);
            Client2.Tamer.GainBit(Bits);
            Client2.Tamer.GainBit(-Bits2);
            Client.Tamer.GainBit(Bits2);

            return 3;
        }

        // Função para cancelar a troca
        public void Cancelar()
        {
            // Removendo o trade dos clients
            Client.Trade = null;
            Client2.Trade = null;
            Client.confirmaTrade = 0;
            Client2.confirmaTrade = 0;

            // Zerando variáveis
            Items = null;
            Items2 = null;
            Cards = null;
            Cards2 = null;
            Client = null;
            Client2 = null;
        }

        // Função para desfazer a troca
        public void Desfazer()
        {
            if(Client != null)
                Client.Connection.Send(new Network.Packets.PACKET_TRADE(Client2, 4, new byte[10], new byte[12]));
            if(Client2 != null)
                Client2.Connection.Send(new Network.Packets.PACKET_TRADE(Client, 4, new byte[10], new byte[12]));
            Cancelar();
        }

        // Destructor
        ~Trade()
        {
            Debug.Print("trade destruida");
        }
    }
}
