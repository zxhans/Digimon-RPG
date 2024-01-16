using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Packets
{
    // Verificaçao de login, antes da tamer List
    public class PACKET_TAMER_LIST : OutPacket
    {
        // Retorno principal - produção
        public PACKET_TAMER_LIST(Client sender, long unk)
            : base(PacketType.PACKET_TAMER_LIST)
        {
            // 4 zeros de preenchimento (4 x 00)
            Write(new byte[4]);
            // Valor identificador de pacote
            Write(new byte[] {0x36, 0xFC});
            // Identificador do pacote 02
            Write(0x02);
            // 5 x 00
            Write(new byte[5]);
            // Nome do usuário
            Write(sender.User.Username, 25);
            // 317 x 00 - O preenchimento de zeros "inúteis" no final do pacote NAO é necessário.
            //Write(new byte[317]);
        }

        // Os pacotes abaixo são respostas para base de testes
        public PACKET_TAMER_LIST(int nulled)
            : base(PacketType.PACKET_TAMER_LIST)
        {
            // IP Block? // 3 - 16 -> IP
            //Write((short)0);///Unkow 2 bytes
            Write(3);
            Write(18);
            Write("kelb2015", 25);
            Write("", 21);
            Write(new byte[254]);
        }

        public PACKET_TAMER_LIST()
            : base(PacketType.PACKET_TAMER_LIST)
        {
            // OK
            //Write((short)0);///Unkow 2 bytes
            Write(2);
            Write(0);
            Write("kelb2015", 25);
            Write("", 21);
            Write(new byte[254]);

        }

        public PACKET_TAMER_LIST(byte derp)
            : base(PacketType.PACKET_TAMER_LIST)
        {
            // ERR
            //Write((short)0);///Unkow 2 bytes
            Write(3);
            Write(2001);
            Write("kelb2015", 25);
            Write("", 21);
            Write(new byte[254]);
        }

        //03 00 00 00 D1 07 00 00 6B 65 6C 62 32 30 31 35 00 00 00 00 00 00 00 00 00 00 00 00 00 32 30 31 35 2D 31 31 2D 31 33 20 30 38 3A 33 31 3A 31 31 00 00 C0 DF B8 F8 B5 C8 20 C0 CE C1 F5 C5 B0 B8 A6 20 BB E7 BF EB 20 C7 CF BF B4 BD C0 B4 CF B4 D9 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
    }
}
