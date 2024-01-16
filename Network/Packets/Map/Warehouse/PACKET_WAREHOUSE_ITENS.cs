using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

namespace Digimon_Project.Network.Packets
{
    // Listagem do Warehouse
    public class PACKET_WAREHOUSE_ITENS : OutPacket
    {
        public PACKET_WAREHOUSE_ITENS(Client c)
            : base(PacketType.PACKET_WAREHOUSE_ITENS)
        {
            Write(StringHex.Hex2Binary("13 00 00 00 93 46 00"));
            Write(c.User.Username, 21); // Username
            Write(new byte[19]); // Espaço vazio
            Write((byte)1); // ????
            Write(new byte[2]); // ????

            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();

            // Cards
            for (int i = 0; i < 24; i++)
                itemWrite.WriteCard(c.Tamer.WareCards[i], this);

            // Itens
            for (int i = 0; i < 24; i++)
                itemWrite.WriteItem(c.Tamer.WareItems[i], this);

            // Wharehouse upgrade
            Write(c.User.WareExp); // 1: 6
                                   // 2: 12
                                   // 3: 18
                                   // 4 || 0: 24

            Write(new byte[3]);

            Write(c.User.WareBits); // Bits na Warehouse
        }
    }
}
