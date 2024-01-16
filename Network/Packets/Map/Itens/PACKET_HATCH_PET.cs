using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Client chocou um Pet Egg
    public class PACKET_HATCH_PET : OutPacket
    {
        public PACKET_HATCH_PET(int id, int idx)
            : base(PacketType.PACKET_HATCH_PET)
        {
            Write(new byte[6]); // Preechimento
            Write(3); // Código de erro
            Write(id); // Id do item no banco
            Write(idx); // Id do item no codex
        }
    }
}
