using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Diagnostics;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 15 Client solicita criaçao/alteraçao da Warehousepassword
    [PacketHandler(Type = PacketType.PACKET_NEW_TRADE_PASS, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_CHANGE_TRADE_PASS : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Removendo bytes inúteis da frente
            byte[] unk = packet.ReadBytes(7);

            // Username
            string unsername = packet.ReadString(21);

            // Senha atual
            string atualPass = packet.ReadString(40);

            // Nova Senha
            string newPass = packet.ReadString(40);
            // Confirmação da nova Senha
            string ConfNewPass = packet.ReadString(40);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // A senha atual confere, podemos mudar/criar a nova
            if (atualPass == sender.User.TradePassword)
            {
                // Alterando no banco
                Emulator.Enviroment.Database.Update("users"
                    , new Database.QueryParameters() { { "trade_password", newPass } }, "where id = @id"
                    , new Database.QueryParameters() { { "@id", sender.User.Id } }
                    , "");

                // Atualizando o objeto Client
                sender.User.TradePassword = newPass;

                // Enviando pacote para o client
                sender.Connection.Send(new Packets.PACKET_NEW_TRADE_PASS(sender, atualPass, newPass));
            }
            else
            {
                // A senha atual está diferente da senha recebida

            }
        }
    }
}
