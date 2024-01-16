using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 14 CLient envia verificação de conexão. Aqui também acontece verificação de primeiro acesso.
    [PacketHandler(Type = PacketType.PACKET_CHECK_CONNECTION, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_CHECK_CONNECTION : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Apesar de não servir, todo o pacote tem que ser lido. Isso libera a memória do buffer, permitindo
            // o recebimento de novos pacotes.
            int unk = packet.ReadInt();
            string unk2 = packet.ReadString(24);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            sender.Connection.Send(new Packets.PACKET_CHECK_CONNECTION(sender.User.Username
                , sender.User.TradePassword == "�ű������Դϴ���й�ȣ Conf")); 
            // Verificando se é o primeiro acesso do usuário
            // a string em comparaçao com o TradePassword é um valor padrao que o client usa em caso
            // de primeiro acesso. Como o pacote enviado é o mesmo que o pacote de CHANGE_TRADE_PASSWORD,
            // temos que tratá-lo da mesma forma, mantendo o campo com este verificador como Default na tabela.
            // Dessa forma, o pacote CC 15 vai funcionar tanto para criaçao da senha, quanto para alteraçao.
        }
    }
}
