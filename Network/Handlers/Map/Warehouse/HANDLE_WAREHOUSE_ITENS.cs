using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Cliente solicita a listagem da Warehouse
    [PacketHandler(Type = PacketType.PACKET_WAREHOUSE_ITENS, Connection = ConnectionType.Map)]
    public class HANDLE_WAREHOUSE_ITENS : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // apesar do tamanho deste pacote, sua única função é solicitar a listagem.
            // Logo, não precisamos das informações contidas nele.

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Respondendo o client
            if (sender.Autentico) // Verificando se houve autenciação (Warehouse password)
            {
                sender.Connection.Send(new Packets.PACKET_WAREHOUSE_ITENS(sender));

                sender.Autentico = false;
            }
        }
    }
}
