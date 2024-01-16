using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviando o resultado da batalha
    public class PACKET_BATTLE_RESULT : OutPacket
    {
        public PACKET_BATTLE_RESULT(byte result, Client c)
            : base(PacketType.PACKET_BATTLE_RESULT)
        {
            Write(new byte[6]);

            Write(result); // Apenas um byte de argumento: 01-Win 02-Lose

            // Interrompendo a batalha dos Digimons do Client
            foreach (Digimon d in c.Tamer.Digimon)
                if (d != null)
                {
                    d.IcrBattles(result);
                    d.StopBattle();
                }

            c.Tamer.AddPetHP(-1);
        }
    }
}
