using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Package sent when the client requests information from another phase of evolution by pressing D
    public class PACKET_D_INFO : OutPacket
    {
        public PACKET_D_INFO(byte fase, Digimon d)
            : base(PacketType.PACKET_D_INFO)
        {
            Write(new byte[6]); // Filling
            Write(fase);
            Write(new byte[7]); // Filling


            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();
            digimonWrite.WriteDigimon(d, fase, this);
        }
    }
}
