using System;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets.Map
{
    public class PACKET_03CC : OutPacket
    {
        private Random r = new Random();

        public PACKET_03CC()
            : base(PacketType.PACKET_03CC)
        {
            // 1248 - Cards
            for (byte i = 0; i < 24; i++)
            {
                Write(new byte[52]);
            }

            int itemDatabaseId = 0;
            int itemId = 0;

            // 2112 - Items
            for (byte i = 0; i < 24; i++)
            {
                if (i % 2 == 0)
                {
                    itemDatabaseId = i;
                    itemId = 46;

                    Write(itemDatabaseId); // Item DB Id
                    Write(itemId); // Item Id??? -> Changes Item
                    Write(261); // ? -> Changes item aswell?

                    Write(1); // Item Count.
                    Write(100); // Max Stack Size.
                    Write(1000); // ?
                    Write(100); // ?
                    Write(10000); // ?
                    Write(1); // Minimum Level

                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?
                    Write(0); // ?

                    Write(9999); // ??

                    Write(itemDatabaseId + itemId); // Some check.
                    Write(0);  // ?
                }
                else
                {
                    Write(new byte[88]); // Empty slot.
                }
            }

        }
    }
}
