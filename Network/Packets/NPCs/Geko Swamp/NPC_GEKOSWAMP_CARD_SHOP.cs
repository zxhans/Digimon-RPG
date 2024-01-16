using System;
using System.Collections.Generic;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{

    // NPC Item Shop, Happy Park
    [NPC(Type = NPCMap.NPC_GEKOSWAMP_CARD_SHOP)]
    public class NPC_GEKOSWAMP_CARD_SHOP : NPC
    {
        private string name = "NPC_GEKOSWAMP_CARD_SHOP";
        public override void INPC(Client sender, int npcOp)
        {

        }
        public override void INPC(Client sender)
        {
            PACKET_NPC_ITEM_SHOP_WRITER write = new PACKET_NPC_ITEM_SHOP_WRITER();
            write.WriteCards(name, sender);
        }
        public override void INPC(Client sender, int npcId, int id, int quant)
        {
            PACKET_NPC_ITEM_SHOP_WRITER write = new PACKET_NPC_ITEM_SHOP_WRITER();
            write.BuyCard(name, npcId, id, quant, sender);
        }
    }
}
