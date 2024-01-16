using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client equipou um Card no Card Slash durante a batalha
    [PacketHandler(Type = PacketType.PACKET_BATTLE_CARD_SLASH, Connection = ConnectionType.Map)]
    public class HANDLE_BATTLE_CARD_SLASH : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // Posição no card Slash onde o item foi equipado
            int pos = packet.ReadInt();

            // ID do item que foi equipado
            int ID = packet.ReadInt();

            // O pacote contem o informativo geral do item. Contudo, apenas o ID nos interessa.
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Primeiro, vamos procurar o card no inventário
            for(int i = (int)EquipSlots.card1 - 1; i < (int)EquipSlots.card6; i++)
            {
                Item card = sender.Tamer.Cards[i];
                if(card != null && card.Id == ID)
                {
                    switch (pos)
                    {
                        case 0:
                            sender.Tamer.Slash1 = i;
                            break;
                        case 1:
                            sender.Tamer.Slash2 = i;
                            break;
                        case 2:
                            sender.Tamer.Slash3 = i;
                            break;
                    }
                    return;
                }
            }
        }
    }
    /**/
}
