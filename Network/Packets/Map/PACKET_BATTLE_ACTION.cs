using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado para encher a barra de TP (Yellow bar) em batalha
    public class PACKET_BATTLE_ACTION : OutPacket
    {
        public PACKET_BATTLE_ACTION(long id, long id2, string arg)
            : base(PacketType.PACKET_BATTLE_ACTION)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            //Write(Utils.StringHex.Hex2Binary("02 0A"));
            Write(Utils.StringHex.Hex2Binary(arg)); // ID da ação
                                                    // 01 0B (2817) - Prepara ataque (O punho que fica acima do Digimon a atacar)
                                                    // 03 00 (3) - Zera a barra amarela
            Write(Utils.StringHex.Hex2Binary("00 00"));

            Write(id);
            Write(new byte[8]);

            Write(id2);
            Write(new byte[8]);
        }

        public PACKET_BATTLE_ACTION(long id, string sufix, long id2, string sufix2, byte arg1, byte arg2)
            : base(PacketType.PACKET_BATTLE_ACTION)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            Write(arg1); // ID da ação
                         // 01 - Prepara ataque (O punho que fica acima do Digimon a atacar)
                         // 03 - Zera a barra amarela
            Write(arg2); // Valor da ação
                         // No caso do ID 01, o valor será o ID do ataque

            Write(Utils.StringHex.Hex2Binary("00 00"));

            Write(id); // ID do solicitante
            Write(sufix, 8);

            Write(id2); // ID do alvo
            Write(sufix2, 8);
        }
    }
}
