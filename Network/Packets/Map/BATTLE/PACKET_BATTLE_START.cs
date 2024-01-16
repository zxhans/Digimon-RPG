using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Primeira resposta ao iniciar batalha
    public class PACKET_BATTLE_START : OutPacket
    {
        public PACKET_BATTLE_START(long spawn_id)
            : base(PacketType.PACKET_BATTLE_START)
        {
            Write(new byte[6]); // Preenchimento

            Write(spawn_id); // Identificador do Spawn
            Write(new byte[8]); // Restante do Identificador
            
            Write(Utils.StringHex.Hex2Binary("01 02 00 00"));

        }
    }
}
