using System;
using System.IO;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // This is just a package writer with digimon structure, just to keep repeating the structure.
    public class PACKET_TAMER_DIGIMON_WRITER
    {
        public void WriteTamer(Tamer tamer, OutPacket p)
        {
            WriteTamer(tamer, (short)tamer.Location.X, (short)tamer.Location.Y, p);
        }
        public void WriteTamer(Tamer tamer, short x, short y, OutPacket p)
        {
            try
            {
                if (tamer != null && tamer.Digimon != null && tamer.Digimon.Count > 0)
                {
                    p.Write(tamer.GUID); // GUID
                    p.Write(tamer.Name, 8); // GUID
                    p.Write(tamer.Name, 21); // Tamer Name
                    p.Write((byte)tamer.Model); // Model
                    p.Write((ushort)tamer.Rank); // Tamer Rank
                    p.Write(tamer.Level); // Tamer Level
                    p.Write(Utils.StringHex.Hex2Binary("01")); // Animation
                                                               // 04 - Digimon slowly walks calmly
                    p.Write(Utils.StringHex.Hex2Binary("08")); // Direction I'm Looking at When I Appear
                    p.Write(x); // X1
                    p.Write(y); // Y1
                    p.Write(x); // X2
                    p.Write(y); // Y2
                    p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Separation
                                                                        // Digimon
                    p.Write(tamer.Digimon[0].BattleId); // BattleID (GUID)
                    p.Write(tamer.Digimon[0].BattleSufix, 8); // Battle Sufix (GUID)
                    p.Write(tamer.Digimon[0].Model); // Digimon Model
                    p.Write(tamer.Digimon[0].Name, 21);
                    p.Write((byte)tamer.Digimon[0].estage); // Stage
                    p.Write((int)tamer.Digimon[0].Level); // Digimon Level
                    p.Write(Utils.StringHex.Hex2Binary("00 00")); // Digimon above animation (when hovering)
                    p.Write(Utils.StringHex.Hex2Binary("00 00"));
                    byte party = 0;
                    if (tamer.Party != null && tamer.Party.Lider == tamer) party = 1;
                    p.Write(party); // Party Leader?
                    p.Write(Utils.StringHex.Hex2Binary("00"));
                    p.Write(Utils.StringHex.Hex2Binary("00 00"));
                    p.Write(tamer.Aura); // Aura
                    p.Write(Utils.StringHex.Hex2Binary("C7 01 00 00 00 00 00 00"));
                    p.Write(tamer.Pet); // Pet ID
                    p.Write(tamer.PetHP); // Pet HP
                    p.Write(tamer.Sock); // Socks
                    p.Write(tamer.Shoes); // Shoes
                    p.Write(tamer.Pants); // pants
                    p.Write(tamer.Glove); // Gloves
                    p.Write(tamer.Tshirt); // T Shirt
                    p.Write(tamer.Jacket); // Jacket
                    p.Write(tamer.Hat); // Hat
                    p.Write(tamer.Customer); // Customer item - Affects movement speed
                    p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")); // Guild Name
                    p.Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
                }
            }
            catch
            {

            }
        }
    }
}
