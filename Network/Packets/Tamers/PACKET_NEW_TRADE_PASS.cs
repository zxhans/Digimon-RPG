using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Packets
{
    // CC 15 Pacote enviando confirmaçao da criaçao/alteraçao da Warehousepassword
    public class PACKET_NEW_TRADE_PASS : OutPacket
    {
        public PACKET_NEW_TRADE_PASS(Client sender, String antiga, String nova)
            : base(PacketType.PACKET_NEW_TRADE_PASS)
        {
            // 6 zeros de preenchimento (6 x 00)
            Write(new byte[6]);
            // Valor identificador de pacote
            Write(0x01);
            // Username
            Write(sender.User.Username, 21);
            // Senha antiga
            Write(antiga, 40);
            // Nova Senha
            Write(nova, 40);
            Write(nova, 40); // Confirmaçao

        }
    }
}
