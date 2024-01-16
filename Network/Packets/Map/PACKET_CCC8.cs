using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_CCC8 : OutPacket
    {
        public PACKET_CCC8(Item item)
            : base(PacketType.PACKET_CCC8)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 E2 9D"));
            Write(Utils.StringHex.Hex2Binary("01 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00"));
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
            itemWriter.WriteCard(item, this);
        }
    }
}
