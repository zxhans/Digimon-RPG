using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote que envia item pego no chão
    public class PACKET_GAIN_ITEM_INFO : OutPacket
    {
        public PACKET_GAIN_ITEM_INFO(Item item)
            : base(PacketType.PACKET_GAIN_ITEM_INFO)
        {
            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 5F 34")); // Preenchimento
            Write(Utils.StringHex.Hex2Binary("02 00 00 00 03 00 00 00")); // Preenchimento

            // Posição do Item no Inventário
            Write(item.GetLinha()); // Linha 
            Write(item.GetColuna()); // Coluna

            // Escrevendo o Item no pacote
            itemWrite.WriteItem(item, this);

            //Write(new byte[104]);
        }
    }
}
