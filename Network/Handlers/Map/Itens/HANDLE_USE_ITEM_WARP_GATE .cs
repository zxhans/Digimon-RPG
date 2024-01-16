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
    [PacketHandler(Type = PacketType.PACKET_TELEPORT_GATE, Connection = ConnectionType.Map)]
    public class HANDLE_USE_ITEM_WARP_GATE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            
            // Lixo
            int unk = packet.ReadInt();
            int unk2 = packet.ReadShort();

            /*
            // Posição para onde foi teleportado
            int trash1 = packet.ReadInt();

            int trash2 = packet.ReadInt();

            int itemIDX = packet.ReadInt();
            */

            byte[] trash2 = packet.ReadBytes(8);

            int itemIDX = packet.ReadInt();

            byte[] trash3 = packet.ReadBytes(92);

            byte mapID = packet.ReadByte();

            // O pacote contem todas as informações do item. Contudo, apenas o ID já é suficiente neste processo
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            if (itemIDX == 522 || itemIDX == 568 || itemIDX == 570 || itemIDX == 571    //JUMP GATE
                || itemIDX == 571)                                                      //DUNGEON GATE
            {
                if (sender.Tamer.MapId != mapID)
                {
                    if (mapID == 1)
                    {
                        sender.Tamer.Teleport(mapID, 61, 89); //DIGITAL CITY
                    }
                    else if (mapID == 2)
                    {
                        sender.Tamer.Teleport(mapID, 162, 21); //HEAVEN W
                    }
                    else if (mapID == 3)
                    {
                        sender.Tamer.Teleport(mapID, 23, 16); //HEAVEN S
                    }
                    else if (mapID == 4)
                    {
                        sender.Tamer.Teleport(mapID, 17, 16); //HAPPY PARK N
                    }
                    else if (mapID == 5)
                    {
                        sender.Tamer.Teleport(mapID, 64, 101); //HAPPY PARK
                    }
                    else if (mapID == 6)
                    {
                        sender.Tamer.Teleport(mapID, 24, 12); //HAPPY PARK S
                    }
                    else if (mapID == 7)
                    {
                        sender.Tamer.Teleport(mapID, 10, 236); //HAPPY PARK E
                    }
                    else if (mapID == 8)
                    {
                        sender.Tamer.Teleport(mapID, 21, 13); //FOREST IN
                    }
                    else if (mapID == 9)
                    {
                        sender.Tamer.Teleport(mapID, 184, 14); //TWIST PARK
                    }
                    else if (mapID == 10)
                    {
                        sender.Tamer.Teleport(mapID, 9, 131); //REST PARK
                    }
                    else if (mapID == 11)
                    {
                        sender.Tamer.Teleport(mapID, 92, 14); //LEARN PARK
                    }
                    else if (mapID == 12)
                    {
                        sender.Tamer.Teleport(mapID, 154, 165); //CALM FOREST
                    }
                    else if (mapID == 13)
                    {
                        sender.Tamer.Teleport(mapID, 10, 58); //PARK TOWN
                    }
                    else if (mapID == 14)
                    {
                        sender.Tamer.Teleport(mapID, 10, 58); //PARK TOWN
                    }
                    else if (mapID == 18)
                    {
                        if (sender.Tamer.Level >= 41)
                        {
                            sender.Tamer.Teleport(mapID, 148, 20); //UNDERGROUND 2
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Require Tamer Level 11 or above!");
                            mapID = 0;
                        }
                    }
                    else if (mapID == 19)
                    {
                        if (sender.Tamer.Level >= 41)
                        {
                            sender.Tamer.Teleport(mapID, 13, 22); //UNDERGROUND 1
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Require Tamer Level 11 or above!");
                            mapID = 0;
                        }
                        
                    }
                    else if (mapID == 20)
                    {
                        sender.Tamer.Teleport(mapID, 50, 70); //MUD
                    }
                    else if (mapID == 21)
                    {
                        sender.Tamer.Teleport(mapID, 9, 88); //SQUARE WORLD
                    }
                    else if (mapID == 22)
                    {
                        sender.Tamer.Teleport(mapID, 90, 254); //SQUARE WORLD N
                    }
                    else if (mapID == 23)
                    {
                        sender.Tamer.Teleport(mapID, 144, 256); //SQUARE WORLD W
                    }
                    else if (mapID == 24)
                    {
                        sender.Tamer.Teleport(mapID, 8, 88); //SQUARE WORLD S
                    }
                    else if (mapID == 37)
                    {
                        sender.Tamer.Teleport(mapID, 10, 15); //LAB 3F
                    }
                    else if (mapID == 38)
                    {
                        sender.Tamer.Teleport(mapID, 33, 37); //LAB 2F
                    }
                    else if (mapID == 39)
                    {
                        sender.Tamer.Teleport(mapID, 18, 162); //LAB 1F
                    }
                    else if (mapID == 40)
                    {
                        sender.Tamer.Teleport(mapID, 88, 161); //GEKO SWAMP
                    }
                    else if (mapID == 41)
                    {
                        sender.Tamer.Teleport(mapID, 60, 17); //GEKO FOREST
                    }
                    else if (mapID == 42)
                    {
                        sender.Tamer.Teleport(mapID, 24, 4); //GEKO FOREST W
                    }
                    else if (mapID == 43)
                    {
                        sender.Tamer.Teleport(mapID, 10, 48); //GEKO FOREST E
                    }
                    else if (mapID == 44)
                    {
                        sender.Tamer.Teleport(mapID, 194, 53); //GEKO SWAMP W
                    }
                    else if (mapID == 45)
                    {
                        sender.Tamer.Teleport(mapID, 67, 199); //GEKO SWAMP LAKE
                    }
                    else if (mapID == 60)
                    {
                        sender.Tamer.Teleport(mapID, 86, 119); //TIME VILLAGE
                    }
                    else if (mapID == 61)
                    {
                        sender.Tamer.Teleport(mapID, 110, 15); //ICE WORLD
                    }
                    else if (mapID == 62)
                    {
                        sender.Tamer.Teleport(mapID, 100, 100); //ICE WORLD W
                    }
                    else if (mapID == 63)
                    {
                        sender.Tamer.Teleport(mapID, 90, 13); //ICE WORLD S
                    }
                    else if (mapID == 64)
                    {
                        sender.Tamer.Teleport(mapID, 10, 56); //ICE WORLD E
                    }
                    else if (mapID == 65)
                    {
                        sender.Tamer.Teleport(mapID, 103, 144); //TIME VILLAGE W
                    }
                    //DUNGEON
                    else if (mapID == 35)
                    {
                        if (sender.Tamer.Level >= 41)
                        {
                            sender.Tamer.Teleport(mapID, 156, 243); //DOD 2
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Require Tamer Level 41 or above!");
                            mapID = 0;
                        }
                    }
                    else if (mapID == 36)
                    {
                        if (sender.Tamer.Level >= 41)
                        {
                            sender.Tamer.Teleport(mapID, 170, 27); //DOD 1
                        }
                        else
                        {
                            Utils.Comandos.Send(sender, "Require Tamer Level 41 or above!");
                            mapID = 0;
                        }
                    }
                    else
                    {
                        Utils.Comandos.Send(sender, "Not mapped info");
                        Utils.Comandos.Send(sender, "itemInfo: " + itemIDX);
                        Utils.Comandos.Send(sender, "mapID: " + mapID);
                        mapID = 0;
                    }

                    if (mapID != 0)
                    {
                        foreach (Item i  in sender.Tamer.Items)
                        {
                            if (i != null)
                            {
                                if (i.ItemId == itemIDX)
                                {
                                    sender.Tamer.RemoveItem(i.ItemName, 1);
                                    sender.Tamer.AtualizarInventario();
                                    break;
                                }
                            }
                            
                        }
                    }
                }
                else
                {
                    Utils.Comandos.Send(sender, "You are already in this map!");
                    mapID = 0;
                }
            }
            sender.Connection.Send(new Packets.PACKET_USE_ITEM_TP_WARP_GATE((byte)mapID));
        }
    }
}
