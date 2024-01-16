using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

namespace Digimon_Project.Network.Packets
{
    // Resposta aos processo de expansão do Digistore
    public class PACKET_DIGISTORE_EXPANSION : OutPacket
    {
        public PACKET_DIGISTORE_EXPANSION(byte result)
            : base(PacketType.PACKET_DIGISTORE_EXPANSION)
        {
            Write(new byte[6]);

            // Resultado do processo
            Write(result);
            // [0: Sucesso, 1: Sem item, 2: Todos slots liberados, 3: Error]
        }
    }
}
