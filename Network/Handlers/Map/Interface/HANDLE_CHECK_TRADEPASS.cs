using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido em solicitações que requerem confirmação do warehouse password
    [PacketHandler(Type = PacketType.PACKET_CHECK_TRADE_PASS, Connection = ConnectionType.Map)]
    public class HANDLE_CHECK_TRADEPASS : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6); // Preenchimento

            // Username - Apesar de recebermos, não vamos usar essa informação. Usaremos o username armazenado
            // internamente.
            string username = packet.ReadString(21);

            // Senha
            string pass = packet.ReadString(40);

            byte unk = packet.ReadByte();

            // Operação realizada
            byte op = packet.ReadByte();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Autenticando
            byte ok = 2;
            if (sender.User.TradePassword == pass)
            {
                sender.Autentico = true;
                ok = 1;
            }

            // Respondendo o client
            sender.Connection.Send(new Packets.PACKET_CHECK_TRADE_PASS(username, pass, ok, op));
            Utils.Comandos.Send(sender, "Operacao:: " + op);

            //SE OP = 0 ACESSO A WAREHOUSE DO D

            //SE OP = 1 ACESSO AO CASH WAREHOUSE
            // Processamos com o username interno, mas respondemos com o username recebido.
        }
    }
}
