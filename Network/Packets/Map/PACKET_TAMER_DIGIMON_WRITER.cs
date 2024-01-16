using System;
using System.IO;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Este é apenas um Writer de pacotes com estrutura de digimons, só para não ficar repetindo a estrutura.
    public class PACKET_TAMER_DIGIMON_WRITER
    {
        public void WriteTamer(Tamer tamer, OutPacket p)
        {
            p.Write(tamer.GUID); // GUID
            p.Write(tamer.Name, 8); // GUID
            p.Write(tamer.Name, 21); // Tamer Name
            p.Write((byte)tamer.Model); // Model
            p.Write((ushort)tamer.Rank); // Tamer Rank
            p.Write(tamer.Level); // Tamer Level
            p.Write(Utils.StringHex.Hex2Binary("01")); // Animação
                                                     // 04 - Devagar, Digimon anda calmamente
            p.Write(Utils.StringHex.Hex2Binary("08")); // Direção para onde estou olhando quando apareço
            p.Write((short)tamer.Location.X); // X1
            p.Write((short)tamer.Location.Y); // Y1
            p.Write((short)tamer.Location.X); // X2
            p.Write((short)tamer.Location.Y); // Y2
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Separação
            // Digimon
            p.Write(tamer.Digimon[0].BattleId); // BattleID (GUID)
            p.Write(tamer.Digimon[0].BattleSufix, 8); // Battle Sufix (GUID)
            p.Write(tamer.Digimon[0].Model); // Digimon Model
            p.Write(tamer.Digimon[0].Name, 21);
            p.Write((byte)tamer.Digimon[0].estage); // Stage
            p.Write((int)tamer.Digimon[0].Level); // Digimon Level
            p.Write(Utils.StringHex.Hex2Binary("00 00")); // Animação acima do Digimon (ao passar o mouse)
            p.Write(Utils.StringHex.Hex2Binary("00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00")); // Party Leader?
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
            p.Write(tamer.Customer); // Customer item - Afeta a velocidade de movimento
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")); // Guild Name
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
        }
    }
}
