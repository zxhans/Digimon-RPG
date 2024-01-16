using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Network.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client clicou em alguma opção de algum NPC
    [PacketHandler(Type = PacketType.PACKET_NPC_ITEM_SHOP_OP, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_NPC_ITEM_SHOP_OP : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unknown = packet.ReadInt();
            short unk = packet.ReadShort();
            int op = packet.ReadInt();
            // Lendo os dados do item processado
            int ID = packet.ReadInt();

            // Depois vem as informações básicas do item. Contudo, só precisamos do ID
            byte[] trash = packet.ReadBytes(96);

            // Quantidade processada
            int quant = packet.ReadInt();

            byte[] trash2 = packet.ReadBytes(14);
            int unk4 = packet.ReadInt();
            byte[] trash3 = packet.ReadBytes(6);

            // ID do NPC
            int npcId = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Comprando o item
            if (npcId != 0 && op == 1)
            {
                NPC npc = null;
                npc = NPCATable.Instance.Get((NPCMap)npcId);
                if (npc != null)
                {
                    npc.INPC(sender, npcId, ID, quant);
                }
                else
                {
                    if (Emulator.Enviroment.Teste)
                    {
                        Console.WriteLine("NPC ID inexistente: {0}", npcId);
                        Debug.Print("NPC ID inexistente: {0}", npcId);
                    }
                }
            }
            // Vendendo o item
            else if (npcId == 0 && op == 2)
            {
                PACKET_NPC_ITEM_SHOP_WRITER write = new PACKET_NPC_ITEM_SHOP_WRITER();
                write.SellItem(ID, quant, sender);
            }
        }
    }
}
