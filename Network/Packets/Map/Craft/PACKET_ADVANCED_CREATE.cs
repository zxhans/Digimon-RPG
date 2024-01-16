using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

namespace Digimon_Project.Network.Packets
{
    // Response to the process of creating crests and basic Digieggs
    public class PACKET_ADVANCED_CREATE : OutPacket
    {
        public PACKET_ADVANCED_CREATE(int receita, int inventario, int x, int y)
            : base(PacketType.PACKET_ADVANCED_CREATE)
        {
            Write(new byte[6]);

            // Recipe ID
            Write(receita);

            // Inventory where the item was created
            Write(inventario);

            // Coordinates where the item was created
            Write(x);
            Write(y);

            // If the reported data is reset, then there were no items to execute the recipe.
        }
    }
}
