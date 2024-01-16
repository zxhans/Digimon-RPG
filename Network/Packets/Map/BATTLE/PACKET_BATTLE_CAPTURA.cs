using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado durante a Digievolução em batalha
    public class PACKET_BATTLE_CAPTURA : OutPacket
    {
        public PACKET_BATTLE_CAPTURA(Tamer t, Digimon c, Digimon d, int r)
            : base(PacketType.PACKET_BATTLE_CAPTURA)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 75 A8"));
            Write(r); // Resultado 02 - Falha, 01 - Sucesso
            Write(t.GUID);
            Write(t.Name, 8);
            Write(new byte[8]);
            Write(c.BattleId);
            Write(c.BattleSufix, 8);
            Write(new byte[504]);
            Write(d.BattleId);
            Write(d.BattleSufix, 8);
            Write(new byte[4]);
        }
    }
}
