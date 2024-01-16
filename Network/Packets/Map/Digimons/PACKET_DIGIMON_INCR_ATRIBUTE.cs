using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Package that sends the client extra point information on some attribute 
    public class PACKET_DIGIMON_INCR_ATRIBUTE : OutPacket
    {
        public PACKET_DIGIMON_INCR_ATRIBUTE(Digimon d, byte fase, byte atribute)
            : base(PacketType.PACKET_DIGIMON_INCR_ATRIBUTE)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 C2 75")); // Fill

            PACKET_DIGIMON_WRITER writer = new PACKET_DIGIMON_WRITER();
            writer.WriteDigimon(d, fase, this);

            Write(fase);
            Write(atribute);
            Write(new byte[6]); // Fill
        }
    }
}
