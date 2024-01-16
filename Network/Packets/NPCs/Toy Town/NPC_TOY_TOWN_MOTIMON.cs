using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;

namespace Digimon_Project.Network.Packets
{

    // NPC Patamon, Toy Town
    [NPC(Type = NPCMap.NPC_TOY_TOWN_MOTIMON)]
    public class NPC_TOY_TOWN_MOTIMON : NPC
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
            if(sender.Tamer.ItemCount("Tutorial Book 3") == 0)
            {
                sender.Tamer.AddItem("Tutorial Book 3", 1, false);
                Utils.Comandos.Send(sender, "Tutorial Book 3 received! Look your inventory (Press I)");
                sender.Connection.Send(new Network.Packets.PACKET_INVENTARIO_ATT(sender.Tamer));
            }

            // Respondendo o Client
            OutPacket p = new OutPacket(PacketType.PACKET_NPC);
            p.Write(new byte[4]);
            p.Write((int)NPCMap.NPC_TOY_TOWN_MOTIMON);
            p.Write(npcOp);
            sender.Connection.Send(p);
        }
    }
}
