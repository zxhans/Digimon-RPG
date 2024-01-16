using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado quando o client solicita informação de outra fase de evolução ao pressionar D
    public class PACKET_D_INFO : OutPacket
    {
        public PACKET_D_INFO(byte fase, Digimon d)
            : base(PacketType.PACKET_D_INFO)
        {
            Write(new byte[6]); // Preenchimento
            Write(fase);
            Write(new byte[7]); // Preenchimento
            

            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, fase, this);
        }
    }
}
