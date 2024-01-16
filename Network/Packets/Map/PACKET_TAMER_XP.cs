using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Tamer recebeu XP. Este pacote também envia atualização dos equipamentos no Tamer
    public class PACKET_TAMER_XP : OutPacket
    {
        public PACKET_TAMER_XP(Tamer t)
            : base(PacketType.PACKET_TAMER_XP)
        {
            Write(new byte[6]);
            Write((ushort)t.Rank);
            Write(t.Level);
            Write((ushort)t.Wins);
            Write((ushort)t.Battles);
            Write(t.XP);
            Write(t.MaxXP);

            //Write(new byte[8]);
            Write(t.Pet);
            Write(t.PetHP);

            // Itens equipados
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
