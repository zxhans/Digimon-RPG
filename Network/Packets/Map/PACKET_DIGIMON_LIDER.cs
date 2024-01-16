using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado ao alterar o Digimon líder
    public class PACKET_DIGIMON_LIDER : OutPacket
    {
        public PACKET_DIGIMON_LIDER(int id, int id2)
            : base(PacketType.PACKET_DIGIMON_LIDER)
        {
            Write(new byte[6]); // Preenchimento
            Write(1);
            Write(id);
            Write(id2);
        }
    }
}
