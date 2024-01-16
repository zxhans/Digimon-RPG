using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client solicita alteração em seu Digimon, como o Nome
    [PacketHandler(Type = PacketType.PACKET_DIGIMON_ITEM_CHANGE_NAME, Connection = ConnectionType.Map)]
    public class HANDLE_DIGIMON_ITEM_CHANGE_NAME : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(10);

            // ID do Digimon
            int ID = packet.ReadInt();

            // Nome
            string name = packet.ReadString(20);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0)
            {
                b = packet.ReadByte();
            }

            /**/
            if (sender.Tamer != null)
            {
                Digimon d = sender.Tamer.Digimon[0];
                if (d != null)
                    foreach (Digimon d2 in sender.Tamer.Digimon)
                    {
                        if (d2 != null && d2.Id == ID)
                        {
                            int i = d2.Slot;

                            sender.Tamer.Digimon[i].Name = name;

                            d2.ChangeName(name);
                            sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(sender.Tamer.Digimon[i]));
                            return;
                        }
                    }
            }
            /**/

        }
    }
}
