using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado para encher a barra de TP (Yellow bar) em batalha
    public class PACKET_BATTLE_TP : OutPacket
    {
        public PACKET_BATTLE_TP(long id, int quant)
            : base(PacketType.PACKET_BATTLE_TP)
        {
            Write(new byte[6]);

            Write(id);
            Write(new byte[8]);

            Write(quant);
            Write(new byte[4]);

        }

        public PACKET_BATTLE_TP(long id, string sufix, int quant)
            : base(PacketType.PACKET_BATTLE_TP)
        {
            Write(new byte[6]);

            Write(id);
            Write(sufix, 8);

            Write(quant);
            Write(new byte[4]);

        }
    }
}
