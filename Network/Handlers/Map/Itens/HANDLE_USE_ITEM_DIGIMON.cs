using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido ao finalizar o login no map. Requer uma resposta do mesmo tipo vazia, PACKET_CCF0
    [PacketHandler(Type = PacketType.PACKET_USE_ITEM_DIGIMON, Connection = ConnectionType.Map)]
    public class HANDLE_USE_ITEM_DIGIMON : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Lixo
            int unk = packet.ReadInt();
            int unk2 = packet.ReadShort();

            // Operação
            int op = packet.ReadInt();
            // ID do item no Banco
            int itemId = packet.ReadInt();
            // O pacote contem todas as informações do item. Contudo, apenas o ID já é suficiente neste processo
            string trash = packet.ReadString(96);

            // Digimon
            int DigimonId = packet.ReadInt();
            long GUID = packet.ReadLong();
            long GUIDSufix = packet.ReadLong();

            // Da mesma forma, o pacote também vem com todas as outras informações do Digimon alvo do item.
            // Mas só precisamos do ID por hora.
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando o item
            foreach(Item i in sender.Tamer.Items)
                if(i != null && i.Id == itemId)
                {
                    // Encontramos o item, agora vamos procurar o Digimon
                    if (i.ItemQuant > 0)
                    {
                        // Capture Nets
                        if(i.ItemEffect1 == 62 && sender.Tamer.Digimon.Count < 5)
                        {
                            // Procurando o Digimon no map
                            MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                            if(map != null)
                            {
                                foreach(Spawn s in map.spawn)
                                    if(s != null && GUID == s.Id && GUIDSufix == s.GUID)
                                    {
                                        if (s.estage == i.ItemEffect1Value)
                                        {
                                            Digimon d = sender.Tamer.AddDigimon(s);
                                            sender.Connection.Send(new Packets.PACKET_DIGIMON_CATCHED_LOPEN(d));
                                            sender.Connection.Send(new Packets.PACKET_USE_ITEM_DIGIMON(i, d, 1, op));
                                            i.Consumir();
                                            sender.Tamer.AtualizarInventario();
                                        }

                                        return;
                                    }
                            }
                        }

                        // Itens comuns
                        foreach (Digimon d in sender.Tamer.Digimon)
                            if (d != null && d.Id == DigimonId)
                            {
                                int consumiu = i.ExecuteItem(d);
                                /**/
                                int quantDecr = 0;
                                if (consumiu > 0)
                                    quantDecr = 1;
                                sender.Connection.Send(new Packets.PACKET_USE_ITEM_DIGIMON(i, d, quantDecr, op));
                                if (consumiu > 0)
                                    i.Consumir();
                                if (consumiu > 1)
                                    sender.Tamer.AtualizarInventario();
                                /**/
                                return;
                            }
                    }
                    return;
                }
        }
    }
}
