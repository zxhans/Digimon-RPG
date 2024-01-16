using Digimon_Project.Enums;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;
using System;

namespace Digimon_Project.Network.Packets
{
    public class PACKET_MAP_TAMER_AND_DIGIMONS : OutPacket
    {
        public PACKET_MAP_TAMER_AND_DIGIMONS(Tamer tamer)
            : base(PacketType.PACKET_MAP_TAMER_AND_DIGIMONS)
        {
            /**/
            PACKET_ITEM_WRITER itemWriter = new PACKET_ITEM_WRITER();

            Write(StringHex.Hex2Binary("00 00 00 00 33 35")); // Filling in
            Write(tamer.Id); // Tamer id
            Write(tamer.GUID); // GUID
            Write(tamer.Name, 8); // GUID
            Write(tamer.Name, 21); // Tamer Name
            Write((byte)tamer.Model); // Model
            Write((ushort)tamer.Rank); // Tamer Rank
            Write((ushort)tamer.Level); // Tamer Level
            Write(StringHex.Hex2Binary("00 00")); // Filling in
            Write(tamer.Reputation); // Reputation
            Write((int)tamer.XP); // Current XP
            Write(XP.MaxForTamerLevel(tamer.Level)); // Max XP
            //Write(StringHex.Hex2Binary("00 00 00 00")); // Filling in
            Write((double)tamer.Bits); // Dinheiro
            Write(tamer.Wins); // Battle Wins
            Write(tamer.Battles); // Total Battles
            Write(StringHex.Hex2Binary("00 00 00 00")); // Filling in

            // Crest 1
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.crest1 - 1], this);
            // Crest 2
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.crest2 - 1], this);
            // Crest 3
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.crest3 - 1], this);
            // Digiegg 1
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.digiegg1 - 1], this);
            // Digiegg 2
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.digiegg2 - 1], this);
            // Digiegg 3
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.digiegg3 - 1], this);

            Write(StringHex.Hex2Binary("00 00 00 00"));
            Write(StringHex.Hex2Binary("00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00"));
            Write(StringHex.Hex2Binary("00 00 00 00"));

            for (int i = 0; i < 6; i++)
            {
                // Card slots
                itemWriter.WriteCard(tamer.Cards[(int)EquipSlots.card1 + i - 1], this);
            }
            Write(StringHex.Hex2Binary("00 00 00 00"));

            // Digimons (520 Bytes cada)
            PACKET_DIGIMON_WRITER digimonWriter = new PACKET_DIGIMON_WRITER();
            for (int i = 0; i < 5; i++)
            {
                if (tamer.Digimon.Count - 1 >= i && tamer.Digimon[i] != null
                    && tamer.Digimon[i].Digistore == 0)
                {
                    digimonWriter.WriteDigimon(tamer.Digimon[i], this);
                }
                else
                {
                    Write(new byte[520]);
                }
            }

            // Continuing Tamer information
            Write((short)tamer.Location.X);
            Write((short)tamer.Location.Y);

            // Aura
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.aura - 1], this);
            // Digivice
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.digivice - 1], this);
            Write(tamer.Pet); // Pet ID
            Write(tamer.PetHP); // Pet HP
            // Sock
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.sock - 1], this);
            // Shoes
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.shoes - 1], this);
            // Pants
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.pants - 1], this);
            // Glove
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.glove - 1], this);
            // T-Shirt
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.tshirt - 1], this);
            // Jacket
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.jacket - 1], this);
            // Hat
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.hat - 1], this);
            // Customer
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.customer - 1], this);
            // Earring
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.earring - 1], this);
            // Necklace
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.necklace - 1], this);
            // Ring
            itemWriter.WriteItem(tamer.Items[(int)EquipSlots.ring - 1], this);

            // Special Effect/CARD
            Write(new byte[100]);

            // Spirit 1
            Write(new byte[100]);

            // Spirit 2
            Write(new byte[100]);

            // Spirit 3
            Write(new byte[100]);

            //coin
            Write((Int16)tamer.Coin); //em 16bits

            //MEDAL
            Write((Int16)tamer.Gold); //em 16bits

            Write(StringHex.Hex2Binary("35 1C 00 00 41 00 00 00 00 00 00 00"));

            //OS PROXIMOS BYTES SAO REFERENTES A BUFFS TEMPORARIOS DE ITENS/GM BUFF ETC

            //Write(new byte[400]);
            //234



            // Bag Expansion
            //itemWriter.WriteItem(tamer.Items[(int)EquipSlots.bagexp1 - 1], this);
            //Write(new byte[200]);
            //Write(new byte[400]);
            //Write(StringHex.Hex2Binary("00 00 00 00 35 1C 00 00 41 00 00 00 00 00 00 00"));
            //Write(StringHex.Hex2Binary("00 00 00 00 00 00 00 00 41 00 00 00 00 00 00 00"));
            Write(new byte[236]);

            /*
             #ifdef ADD_DAILY_QUEST
	WORD  tmMedal;
#endif
#ifdef MILEAGE_COIN_EVENT
	WORD  tmSaveCoin;
#endif
#ifdef ADD_HANGAWI_REDBLUETEAM_EVENT_20100916
	WORD  tmQuestItem;
#endif
	UINT  nConnectedTime; // ¼­¹öÀÇ ÇöÀç ½Ã°£
	
#ifdef MAPMERGE_TEST_20101129
	BYTE	tmMapId;	
#endif
#ifdef ADD_TAMER_BUFFSYSTEM_20110329
	sTamerBuff		tmBuff;
#endif
	short tmResonance;

#ifdef ADD_EFFECT_ITEM_EQUIPMENT

	sItem		tmEffectBody;
	sItem		tmEffectTop;
	sItem		tmEffectUnKnown;
             */
            /**/
        }
    }
}
