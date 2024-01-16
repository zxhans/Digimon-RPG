using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 03 Pedido de conexão: o usuário selecionou o servidor
    [PacketHandler(Type = PacketType.PACKET_CONECT, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_CONECT : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Console.WriteLine("Client trying connect...");
            // Enviando autorização de conexão para o Client
            sender.Connection.Send(new Packets.PACKET_CONECT());
        }
    }
}
