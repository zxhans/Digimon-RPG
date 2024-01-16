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
    // Client desequipou um Card no Card Slash durante a batalha
    [PacketHandler(Type = PacketType.PACKET_BATTLE_CARD_SLASH_REMOVE, Connection = ConnectionType.Map)]
    public class HANDLE_BATTLE_CARD_SLASH_REMOVE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // Posição no card Slash onde o item foi desequipado
            int pos = packet.ReadInt();

            // ID do item que foi desequipado
            int ID = packet.ReadInt();

            // O pacote contem o informativo geral do item. Contudo, apenas o ID nos interessa.
            string trash = packet.ReadString(60);

            // Foram removidos todos os cards? 0 - Não, 1 - Sim
            int RemoveAll = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Removendo o card
            if (RemoveAll == 1 || pos == 0)
                sender.Tamer.Slash1 = 0;
            if (RemoveAll == 1 || pos == 1)
                sender.Tamer.Slash2 = 0;
            if (RemoveAll == 1 || pos == 2)
                sender.Tamer.Slash3 = 0;
        }
    }
    /**/
}
