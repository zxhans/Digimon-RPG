using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado ao solicita lista de evoluções disponíveis em batalha 
    public class PACKET_BATTLE_EVO_LIST : OutPacket
    {
        public PACKET_BATTLE_EVO_LIST(Digimon d)
            : base(PacketType.PACKET_BATTLE_EVO_LIST)
        {
            Write(new byte[8]); // Preenchimento
            Write((short)d.ChampForm); 
            Write((short)d.UltimForm); 
            Write((short)d.MegaForm); 
        }
    }
}
