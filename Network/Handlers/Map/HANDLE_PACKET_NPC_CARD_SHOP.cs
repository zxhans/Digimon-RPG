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
    [PacketHandler(Type = PacketType.PACKET_NPC_CARD_SHOP, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_NPC_CARD_SHOP : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unknown = packet.ReadInt();
            short unk = packet.ReadShort();
            // Lendo o ID do NPC
            int npcId = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            NPC npc = null;
            npc = NPCATable.Instance.Get((NPCMap)npcId);
            if (npc != null)
            {
                npc.INPC(sender);
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
    }
}
