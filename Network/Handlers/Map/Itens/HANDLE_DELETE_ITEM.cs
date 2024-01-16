using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client descartou um item
    [PacketHandler(Type = PacketType.PACKET_DELETE_ITEM, Connection = ConnectionType.Map)]
    public class HANDLE_DELETE_ITEM : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(26);

            // ID do item
            int ID = packet.ReadInt();

            // O pacote contém todos os dados do item. Contudo, só precisamos do ID

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando o item no inventário
            foreach(Item i in sender.Tamer.Items)
                if(i != null && i.Id == ID)
                {
                    sender.Connection.Send(new Packets.PACKET_DELETE_ITEM(trash, i));
                    sender.Tamer.RemoveItem(i);
                    /**
                    i.ItemQuant = 0;
                    i.Delete();
                    /**/
                    return;
                }

        }
    }
}
