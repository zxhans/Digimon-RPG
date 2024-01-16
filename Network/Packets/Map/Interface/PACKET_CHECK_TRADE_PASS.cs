using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Authenticating Tradepassword
    public class PACKET_CHECK_TRADE_PASS : OutPacket
    {
        public PACKET_CHECK_TRADE_PASS(string user, string pass, byte ok, byte op)
            : base(PacketType.PACKET_CHECK_TRADE_PASS)
        {
            Write(new byte[6]);
            Write(user, 21);
            Write(pass, 40);
            Write(ok);
            Write(op);
        }
    }
}
