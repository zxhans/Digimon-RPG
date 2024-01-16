using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{

    // NPC Patamon, Toy Town
    [NPC(Type = NPCMap.NPC_TOY_TOWN_TSUNOMON)]
    public class NPC_TOY_TOWN_TSUNOMON : NPC
    {
        public override void INPC(Client sender)
        {

        }
        public override void INPC(Client sender, int npcId, int id, int quant)
        {

        }
        public override void INPC(Client sender, int npcOp)
        {
            // Verificando se o Client já tem o Tutorial Book 1
            if(sender.Tamer.ItemCount("Tsunomon Ticket") == 0)
            {
                Item item = sender.Tamer.AddItem("Tsunomon Ticket", 1, false);
                Utils.Comandos.Send(sender, "Tsunomon Ticket received! Look your inventory (Press I)");
                sender.Tamer.RemoveItem("Yuramon Drop", 5);
                //sender.Connection.Send(new PACKET_GAIN_ITEM_INFO(item));
                sender.Connection.Send(new PACKET_INVENTARIO_ATT(sender.Tamer));
            }
            else
            {
                Utils.Comandos.Send(sender, "Bring me x5 Yuramon Drop!");
            }

            // Respondendo o Client
            OutPacket p = new OutPacket(PacketType.PACKET_NPC);
            p.Write(new byte[4]);
            p.Write((int)NPCMap.NPC_TOY_TOWN_TSUNOMON);
            p.Write(npcOp);
            sender.Connection.Send(p);
        }
    }
}
