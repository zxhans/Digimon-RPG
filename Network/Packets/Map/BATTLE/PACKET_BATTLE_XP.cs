using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste 
    public class PACKET_BATTLE_XP : OutPacket
    {
        // Pacote que envia BITs e XP recebida durante batalha
        public PACKET_BATTLE_XP(long id, string sufix, long xp, double bit)
            : base(PacketType.PACKET_BATTLE_XP)
        {
            Write(new byte[6]);

            Write(id); // ID do Digimon em batalha
            Write(sufix, 8);
            Write(xp); // XP ganha
            Write(bit); // Bits

        }

        public PACKET_BATTLE_XP(Tamer t, double bit)
            : base(PacketType.PACKET_BATTLE_XP)
        {
            Write(new byte[6]);

            Write(t.Id); 
            Write(t.GUID);
            Write((long)0);
            Write(bit); // Bits

        }
    }
}
