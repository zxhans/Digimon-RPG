using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client clicou em alguma opção de algum NPC
    [PacketHandler(Type = PacketType.PACKET_NPC, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_NPC : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unknown = packet.ReadInt();
            // Lendo o ID do NPC
            ushort npcId = packet.ReadUShort();
            // Lendo o número da opção que o Client clicou
            int npcOp = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            NPC npc = null;
            npc = NPCATable.Instance.Get((NPCMap)npcId);
            if (npc != null)
            {
                npc.INPC(sender, npcOp);
            }
            else
            {
                if (Emulator.Enviroment.Teste)
                    Console.WriteLine("NPC ID inexistente: {0}", npcId);
            }
        }
    }
}
