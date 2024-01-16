using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido em solicitações que requerem confirmação do warehouse password
    [PacketHandler(Type = PacketType.PACKET_OPEN_SHOPWAREHOUSE, Connection = ConnectionType.Map)]
    public class HANDLE_OPEN_SHOWPWAREHOUSE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6); // Preenchimento
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            //ENVIA O PACOTE DOS ITENS E CARDS QUE ESTÃO NO CASH WAREHOUSE
            sender.Connection.Send(new Packets.PACKET_OPEN_SHOPWAREHOUSE(sender.Tamer));
            //Utils.Comandos.Send(sender, "HANDLE_OPEN_SHOWPWAREHOUSE");
        }
    }
}
