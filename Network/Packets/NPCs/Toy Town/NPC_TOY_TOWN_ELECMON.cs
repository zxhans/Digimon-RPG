using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;

namespace Digimon_Project.Network.Packets
{

    // NPC Patamon, Toy Town
    [NPC(Type = NPCMap.NPC_TOY_TOWN_ELECMON)]
    public class NPC_TOY_TOWN_ELECMON : NPC
    {
        public override void INPC(Client sender)
        {

        }
        public override void INPC(Client sender, int npcId, int id, int quant)
        {

        }
        public override void INPC(Client sender, int npcOp)
        {
            string titulo = "Quest Elecmon";
            // Verificando se o Client já fez essa quest
            sender.Tamer.RemoveItem("Tsunomon Ticket", 1);
            if (sender.Tamer.GetQuest(titulo) == null)
            {
                sender.Tamer.NewQuest(titulo, "especial", "especial");
                sender.Tamer.AddCard("GDrillX", 50, false);
                sender.Tamer.AddCard("TutorialCard", 50, false);
                Utils.Comandos.Send(sender, "G. DrillX received! Look your inventory (Press I - Card Tab)");
                Utils.Comandos.Send(sender, "Tutorial Card received! Look your inventory (Press I - Card Tab)");
            }
            sender.Connection.Send(new PACKET_INVENTARIO_ATT(sender.Tamer));

            // Respondendo o Client
            OutPacket p = new OutPacket(PacketType.PACKET_NPC);
            p.Write(new byte[4]);
            p.Write((int)NPCMap.NPC_TOY_TOWN_ELECMON);
            p.Write(npcOp);
            sender.Connection.Send(p);
            sender.Tamer.Teleport(1, 58, 82);
        }
    }
}
