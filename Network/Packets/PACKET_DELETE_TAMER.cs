using Digimon_Project.Enums;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Packets
{
    /// <summary>
    /// Reponse to a tamer deletion request.
    /// </summary>
    public class PACKET_DELETE_TAMER : OutPacket
    {
        public PACKET_DELETE_TAMER(byte slot, string password)
            : base(PacketType.PACKET_DELETE_TAMER)
        {
            Write((ulong)2);
            Write((uint)slot);
            Write(new byte[32]); // No need to send the hash back.
            Write((ulong)0); // 8 empty bytes.
        }
    }
}
