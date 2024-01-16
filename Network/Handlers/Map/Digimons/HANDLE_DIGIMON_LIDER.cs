using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client solicita mudança de Digimon líder
    [PacketHandler(Type = PacketType.PACKET_DIGIMON_LIDER, Connection = ConnectionType.Map)]
    public class HANDLE_DIGIMON_LIDER : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();
            int unk3 = packet.ReadInt();

            int Id = packet.ReadInt();
            int Id2 = packet.ReadInt();

            byte b; // Lendo o restante do pacote (Apesar de conter informações, praticamente na mesma estrutura
            // inclusive, as informações são irrelevantes. Tudo o que precisamos é do ID para responder o Client)
            while (packet.Remaining > 0) b = packet.ReadByte();

            sender.Tamer.SortDigimonSlots();

            /**/
            if(sender.Tamer != null)
            {
                Digimon d = sender.Tamer.Digimon[0];
                if (d != null)
                    foreach (Digimon d2 in sender.Tamer.Digimon)
                    {
                        if (d2 != null && d2.Id == Id2)
                        {
                            int i = d.Slot;
                            int j = d2.Slot;

                            sender.Tamer.Digimon[i] = d2;
                            sender.Tamer.Digimon[j] = d;
                            d.Slot = j;
                            d.SaveSlot();
                            d2.Slot = i;
                            d2.SaveSlot();

                            sender.Connection.Send(new Packets.PACKET_DIGIMON_LIDER(Id, Id2));

                            MapZone Map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                            if (Map != null)
                                Map.SendTamer(sender.Tamer);

                            return;
                        }
                    }
            }
            /**/
        }
    }
}
