using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_USE_ITEM_BOX : OutPacket
    {
        public PACKET_USE_ITEM_BOX(int quantDecr, int op)
            : base(PacketType.PACKET_USE_ITEM_BOX)
        {
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();

            Write(new byte[6]); // Preenchimento
            Write(Utils.StringHex.Hex2Binary("00 04"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));

            Write(new byte[6]); // Preenchimento
        }
    }
}
