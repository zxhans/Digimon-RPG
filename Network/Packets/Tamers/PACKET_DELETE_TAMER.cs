using Digimon_Project.Enums;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Packets
{
    // CC 11  Pacote enviado ao Deletar Tamers
    public class PACKET_DELETE_TAMER : OutPacket
    {
        // Senha incorreta
        public PACKET_DELETE_TAMER(int slot, int id, string password)
            : base(PacketType.PACKET_DELETE_TAMER)
        {
            Write(new byte[6]);
            Write(Utils.StringHex.Hex2Binary("03 00 00 00 D0 07 00 00"));
            Write(password, 36);
            Write((ulong)0); // 8 empty bytes.
        }
    }
}
