using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado para fugir da batalha
    public class PACKET_BATTLE_FUGA : OutPacket
    {
        public PACKET_BATTLE_FUGA(long id, string sufix)
            : base(PacketType.PACKET_BATTLE_FUGA)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            Write(id); // ID do solicitante
            Write(sufix, 8);
        }
    }
}
