using Digimon_Project.Enums;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Packets
{
    // CC 10 Pacote enviado na confirmação da criação de Tamer
    public class PACKET_CREATE_TAMER : OutPacket
    {
        public enum ErrorCodes : int
        {
            OK = 0,
            TamerNameInUse = 102
        }

        public PACKET_CREATE_TAMER(ErrorCodes errCode, int unknown, ushort unknown2, Tamer tamer)
            : this(errCode, unknown, unknown2, (byte)tamer.Model, tamer.Name, tamer.Digimon[0].Model
                  , tamer.Digimon[0].Name)
        {
            // Overload.
        }

        public PACKET_CREATE_TAMER(ErrorCodes errCode,  int unknown, ushort unknown2, byte tamerModel, string tamerName, ushort digimonModel, string digimonName)
            : base(PacketType.PACKET_CREATE_TAMER)
        {
            ;
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 96 A3"));
            // IP Block? // 3 - 16 -> IP
            if (errCode == ErrorCodes.OK)
                Write(2); // Ok
            else
                Write(3); // Err?

            Write((int)errCode);

            Write(new byte[4]);
            Write(unknown);
            Write(unknown2);
            Write(tamerModel); // Tamer Model -> Byte
            Write(tamerName, 21); // Tamer Name -> String[20] + 0 terminator
            Write(digimonModel); // Digimon Model -> Ushort? 
            Write(digimonName, 21); // Digimon Name -> String[20] + 0 terminator
            Write(0); // Unknown 4 Bytes
        }

        // Tamer no mapa
        public PACKET_CREATE_TAMER(Tamer tamer)
            : base(PacketType.PACKET_CREATE_TAMER)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 96 A3"));
            PACKET_TAMER_DIGIMON_WRITER tamerWrite = new PACKET_TAMER_DIGIMON_WRITER();
            tamerWrite.WriteTamer(tamer, this);
        }
        public PACKET_CREATE_TAMER(Tamer tamer, short x, short y)
            : base(PacketType.PACKET_CREATE_TAMER)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 96 A3"));
            PACKET_TAMER_DIGIMON_WRITER tamerWrite = new PACKET_TAMER_DIGIMON_WRITER();
            tamerWrite.WriteTamer(tamer, x, y, this);
        }
        public PACKET_CREATE_TAMER(long GUID, string Sufix)
            : base(PacketType.PACKET_CREATE_TAMER)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 96 A3"));
            Write(GUID);
            Write(Sufix, 8);
            Write(new byte[182]);
        }

        // Teste
        public PACKET_CREATE_TAMER(short x, short y, short x2, short y2)
            : base(PacketType.PACKET_CREATE_TAMER)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 96 A3"));
            Write(Utils.StringHex.Hex2Binary("11 D4 37 53 BF 80 45 87 06 14 F3 27 F1 3C 4A D6")); // GUID
            Write(Utils.StringHex.Hex2Binary("4A 65 6E 72 79 61 32 33 39 35 00 00 00 00 00 00")); // Nick (21)
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("02")); // Tamer Model
            Write(Utils.StringHex.Hex2Binary("03 00")); // Rank (Digivice color)
            Write(Utils.StringHex.Hex2Binary("16 00")); // Tamer Level?
            Write(Utils.StringHex.Hex2Binary("01")); // Animação
                                                     // 04 - Devagar, Digimon anda calmamente
            Write(Utils.StringHex.Hex2Binary("05")); // Direção para onde estou olhando quando apareço
            Write(x); // X1
            Write(y); // Y1
            Write(x2); // X2
            Write(y2); // Y2
            Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Separação
            // Digimon
            Write(Utils.StringHex.Hex2Binary("92 20 73 B1 88 44 54 0F")); // BattleID (GUID)
            Write(Utils.StringHex.Hex2Binary("8F E4 47 C3 E9 3A 25 B7")); // Battle Sufix (GUID)
            Write(Utils.StringHex.Hex2Binary("34 00")); // Digimon Model
            Write(Utils.StringHex.Hex2Binary("52 65 6E 61 6D 6F")); // Digimon Name (21)
            Write(Utils.StringHex.Hex2Binary("6E 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("03")); // Stage
            Write(Utils.StringHex.Hex2Binary("15 00 00 00")); // Digimon Level
            Write(Utils.StringHex.Hex2Binary("00 00")); // Animação acima do Digimon (ao passar o mouse)
            Write(Utils.StringHex.Hex2Binary("00 00"));
            Write(Utils.StringHex.Hex2Binary("00")); // Party Leader?
            Write(Utils.StringHex.Hex2Binary("00"));
            Write(Utils.StringHex.Hex2Binary("00 00"));
            Write(Utils.StringHex.Hex2Binary("16 02 00 00")); // Aura
            Write(Utils.StringHex.Hex2Binary("C7 01 00 00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("05 00 00 00")); // Pet
            Write(Utils.StringHex.Hex2Binary("60 0B 00 00")); // Pet também?
            Write(2001); // Socks
            Write(3001); // Shoes
            Write(4001); // pants
            Write(5001); // Gloves
          //Write(Utils.StringHex.Hex2Binary("4D D7 C6 04 C1 1D 00 00"));
            Write(60010109); // T Shirt
            Write(7001); // Jacket
            Write(8001); // Hat
            Write(Utils.StringHex.Hex2Binary("C1 1D 00 00")); // Customer item? Afeta a velocidade de movimento
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00")); // Guild Name
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
        }
    }
}
