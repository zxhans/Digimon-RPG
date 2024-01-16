using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Respondendo solicitação de alteração no Digimon, como o Nome
    public class PACKET_DIGIMON_NAME : OutPacket
    {
        public PACKET_DIGIMON_NAME(byte error, byte op, int ID, string name)
            : base(PacketType.PACKET_DIGIMON_NAME)
        {
            Write(new byte[6]);
            Write(error);
            Write(op);
            Write((short)0);
            Write(ID);
            Write(name, 20);
        }
    }
}
