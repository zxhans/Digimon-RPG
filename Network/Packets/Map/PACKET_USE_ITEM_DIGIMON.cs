using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_USE_ITEM_DIGIMON : OutPacket
    {
        public PACKET_USE_ITEM_DIGIMON(Item i, Digimon d, int quantDecr, int op)
            : base(PacketType.PACKET_USE_ITEM_DIGIMON)
        {
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
            PACKET_DIGIMON_WRITER digimonWrite = new PACKET_DIGIMON_WRITER();

            Write(new byte[6]); // Preenchimento
            Write(op);
            itemWriter.WriteItem(i, quantDecr, this);
            digimonWrite.WriteDigimon(d, this);
        }
    }
}
