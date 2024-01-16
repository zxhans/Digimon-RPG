using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_USE_ITEM_TP_WARP_GATE : OutPacket
    {
        public PACKET_USE_ITEM_TP_WARP_GATE(byte mapID)
            : base(PacketType.PACKET_TELEPORT_GATE)
        {
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();

            Write(new byte[6]); // Preenchimento
            //itemWriter.WriteItem(i, this);
            Write(new byte[104]); // Preenchimento
            Write(mapID);
            //Write(mapID);
        }
    }
}
