using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client descartou um Card
    [PacketHandler(Type = PacketType.PACKET_DELETE_CARD, Connection = ConnectionType.Map)]
    public class HANDLE_DELETE_CARD : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(26);

            // ID do item
            int ID = packet.ReadInt();

            // O pacote contém todos os dados do card. Contudo, só precisamos do ID


            byte[] trash2 = packet.ReadBytes(126);

            // No caso dos cards, o pacote também possui um tipo de token identificador, que deve ser retornado
            // para o client
            short token = packet.ReadShort();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando o Card no inventário
            foreach(Item i in sender.Tamer.Cards)
                if(i != null && i.Id == ID)
                {
                    sender.Connection.Send(new Packets.PACKET_DELETE_CARD(trash, i, token));
                    sender.Tamer.RemoveCard(i);
                    /**
                    i.ItemQuant = 0;
                    i.Delete();
                    /**/
                    //sender.Connection.Send(new Packets.PACKET_CARD_ATT(i, linha, coluna));
                    return;
                }

        }
    }
}
