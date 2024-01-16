using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Packets
{
    // CC 04 Verificaçao de login, antes da tamer List
    public class PACKET_TAMER_LIST : OutPacket
    {
        // Retorno principal - produção
        public PACKET_TAMER_LIST(Client sender)
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

    }
}
