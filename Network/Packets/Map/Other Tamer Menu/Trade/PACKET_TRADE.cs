using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Enviando listagem da trade
    public class PACKET_TRADE : OutPacket
    {
        public PACKET_TRADE(Client sender, byte op, byte[] card_pos, byte[] item_pos)
            : base(PacketType.PACKET_TRADE)
        {
            if (sender.Tamer != null && sender.Trade != null)
            {
                Write(new byte[6]); // Preenchimento
                // GUID
                Write(sender.Tamer.GUID);
                Write(sender.Tamer.Name, 8);
                // Opção
                Write(op);
                Write((byte)1);

                // Escrevendo Itens e Cards
                PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
                Item[] cards = new Item[10];
                Item[] items = new Item[10];
                double bits = 0;
                if(sender.Trade.Client == sender)
                {
                    cards = sender.Trade.Cards;
                    items = sender.Trade.Items;
                    bits = sender.Trade.Bits;
                }
                else if (sender.Trade.Client2 == sender)
                {
                    cards = sender.Trade.Cards2;
                    items = sender.Trade.Items2;
                    bits = sender.Trade.Bits2;
                }
                // Escrevendo os Cards
                // Posição dos Cards?
                Write(card_pos);
                for (int i = 0; i < 10; i++)
                    itemWriter.WriteTradeCard(cards[i], this);
                // Escrevendo os Itens
                // Posição dos Items?
                Write(item_pos);
                for (int i = 0; i < 10; i++)
                    itemWriter.WriteTradeItem(items[i], this);

                // Bits
                Write(bits);
            }
        }
    }
}
