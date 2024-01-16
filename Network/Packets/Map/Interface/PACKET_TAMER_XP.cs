using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer received XP. This package also sends equipment updates on Tamer
    public class PACKET_TAMER_XP : OutPacket
    {
        public PACKET_TAMER_XP(Tamer t)
            : base(PacketType.PACKET_TAMER_XP)
        {
            Write(new byte[6]);
            Write((ushort)t.Rank);
            Write(t.Level);
            Write(t.Reputation);
            Write((int)t.XP);
            Write((int)t.MaxXP);

            //Write(new byte[8]);
            Write(t.Wins);
            Write(t.Battles);

            // Equipped items
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();

            // Crest 1
            itemWriter.WriteItem(t.Items[(int)EquipSlots.crest1 - 1], this);
            // Crest 2
            itemWriter.WriteItem(t.Items[(int)EquipSlots.crest2 - 1], this);
            // Crest 3
            itemWriter.WriteItem(t.Items[(int)EquipSlots.crest3 - 1], this);
            // Digiegg 1
            itemWriter.WriteItem(t.Items[(int)EquipSlots.digiegg1 - 1], this);
            // Digiegg 2
            itemWriter.WriteItem(t.Items[(int)EquipSlots.digiegg2 - 1], this);
            // Digiegg 3
            itemWriter.WriteItem(t.Items[(int)EquipSlots.digiegg3 - 1], this);

            Write(new byte[28]);

        }
    }
}
