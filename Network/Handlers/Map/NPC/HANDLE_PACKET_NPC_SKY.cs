using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client clicou em alguma opção de algum NPC
    [PacketHandler(Type = PacketType.PACKET_NPC_SKY, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_NPC_SKY : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unknown = packet.ReadInt();
            short unk = packet.ReadShort();
            // Lendo o ID do NPC
            int npcId = packet.ReadInt();

            Utils.Comandos.Send(sender, "NPC ID:" + npcId);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();


            sender.Connection.Send(new Packets.PACKET_NPC_SKY(npcId));

            if (npcId == 11)
            {
                string itemName = "Data Sky3";
                int qntd = 25;
                if (sender.Tamer.ItemCount(itemName) < qntd)
                {
                    Utils.Comandos.Send(sender, "Requer x" + qntd + " " + itemName);
                }
                else
                {
                    sender.Tamer.RemoveItem(itemName, qntd);
                    sender.Tamer.AtualizarInventario();
                    Utils.Comandos.Send(sender, "Consumiu x" + qntd + " " + itemName);
                    sender.Tamer.Teleport(77, 67, 174);
                }
            }
        }
    }
}
