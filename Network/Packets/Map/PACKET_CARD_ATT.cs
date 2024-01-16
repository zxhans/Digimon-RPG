using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_CARD_ATT : OutPacket
    {
        public PACKET_CARD_ATT(Item item, int linha, int coluna)
            : base(PacketType.PACKET_CARD_ATT)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 E2 9D"));
            Write(linha);
            Write(coluna);
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00"));
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();
            itemWriter.WriteCard(item, this);
            Write(new byte[304]);
        }
    }
}
