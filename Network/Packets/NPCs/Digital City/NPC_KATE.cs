using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;

namespace Digimon_Project.Network.Packets
{

    // NPC Patamon, Toy Town
    [NPC(Type = NPCMap.NPC_KATE)]
    public class NPC_KATE : NPC
    {
        public override void INPC(Client sender)
        {

        }
        public override void INPC(Client sender, int npcId, int id, int quant)
        {

        }
        public override void INPC(Client sender, int npcOp)
        {
            string titulo = "Quest Kate Iniciante";
            // Verificando se o Client já fez essa quest
            if(sender.Tamer.GetQuest(titulo) == null)
            {
                if (sender.Tamer.ItemSpace() >= 12)
                {
                    sender.Tamer.NewQuest(titulo, "especial", "especial");
                    sender.Tamer.AddItem("TeleportQuest", 50, false);
                    sender.Tamer.AddItem("TeleportQuest", 50, false);
                    sender.Tamer.AddItem("Miracle DigiEgg+5T", 2592000, false);
                    sender.Tamer.AddItem("Miracle DigiEgg+5T", 2592000, false);
                    sender.Tamer.AddItem("Miracle DigiEgg+5T", 2592000, false);
                    sender.Tamer.AddItem("Crest of Courage+5T", 2592000, false);
                    sender.Tamer.AddItem("Crest of Courage+5T", 2592000, false);
                    sender.Tamer.AddItem("Crest of Courage+5T", 2592000, false);
                    sender.Tamer.AddItem("Digivice 11+T", 2592000, false); //esse item eh uma bosta pq n da pra joga fora nem vender
                    sender.Tamer.AddItem("Pichimon egg", 1, false);
                    sender.Tamer.AddItem("RokieCatchNetX", 1, false);
                    sender.Tamer.AddItem("FreshCatchNetX", 1, false);
                    Utils.Comandos.Send(sender, "Newbie Kit received! (Press I - Item Tab)");
                    sender.Connection.Send(new PACKET_INVENTARIO_ATT(sender.Tamer));
                } else
                    Utils.Comandos.Send(sender, "Mochila cheia! Requer 12 slots livre!");
            }
            else
            {
                Utils.Comandos.Send(sender, "Quest completa.");
            }

            // Respondendo o Client
            OutPacket p = new OutPacket(PacketType.PACKET_NPC);
            p.Write(new byte[4]);
            p.Write((int)NPCMap.NPC_KATE);
            p.Write(npcOp);
            sender.Connection.Send(p);
        }
    }
}
