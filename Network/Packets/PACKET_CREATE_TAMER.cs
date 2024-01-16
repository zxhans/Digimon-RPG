using Digimon_Project.Enums;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Packets
{
    /// <summary>
    /// Reponse to a tamer creation request.
    /// </summary>
    public class PACKET_CREATE_TAMER : OutPacket
    {
        public enum ErrorCodes : int
        {
            OK = 0,
            TamerNameInUse = 102
        }

        public PACKET_CREATE_TAMER(ErrorCodes errCode, int unknown, ushort unknown2, Tamer tamer)
            : this(errCode, unknown, unknown2, (byte)tamer.Model, tamer.Name, tamer.Digimon.Model, tamer.Digimon.Name)
        {
            // Overload.
        }

        public PACKET_CREATE_TAMER(ErrorCodes errCode,  int unknown, ushort unknown2, byte tamerModel, string tamerName, ushort digimonModel, string digimonName)
            : base(PacketType.PACKET_CREATE_TAMER)
        {
            ;
            // IP Block? // 3 - 16 -> IP
            if (errCode == ErrorCodes.OK)
                Write(2); // Ok
            else
                Write(3); // Err?

            Write((int)errCode);

            Write(unknown); // Unknown 4 Bytes
            Write(unknown2); // Unknown 2 Bytes
            Write((short)0);
            Write(tamerModel); // Tamer Model -> Byte
            Write(tamerName, 21); // Tamer Name -> String[20] + 0 terminator
            Write(digimonModel); // Digimon Model -> Ushort? 
            Write(digimonName, 21); // Digimon Name -> String[20] + 0 terminator
            Write(0); // Unknown 4 Bytes
        }
    }
}
