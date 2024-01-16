using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;


namespace Digimon_Project.Network.Packets
{
    // Updating tamer Bits
    public class PACKET_BIT_ATT : OutPacket
    {
        public PACKET_BIT_ATT(Tamer t)
            : base(PacketType.PACKET_BIT_ATT)
        {
            Write(new byte[6]);

            Write((double)t.Bits);

            //Write(new byte[8]);
            Write((int)t.Gold);//GOLD(DIREITA)
            Write((int)t.Coin);//COIN(ESQUERDA)

            //Write(Utils.StringHex.Hex2Binary("02 00 00 00"));//GOLD
            //Write(Utils.StringHex.Hex2Binary("01 00 00 00"));//COIN
        }
    }
}
