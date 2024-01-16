using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;

namespace Digimon_Project.Network.Handlers.Others
{
    // Client solicitando conexão no Map
    // Vamos usar a informação aqui obtida para carregar as informações do client conectado ao Listener
    // da porta 7000, que até aqui não possui nenhuma informação.
    [PacketHandler(Type = PacketType.PACKET_MAP_CONNECT, Connection = ConnectionType.Other)]
    public class HANDLE_MAP_CONNECT : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();
            string tamerName = packet.ReadString(21);
            string username = packet.ReadString(20);
            short unk3 = packet.ReadShort();
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Console.WriteLine("Login request for tamer {0} by username {1}.", tamerName, username);

            // Recarregando os dados do Tamer
            sender.CarregarTamer(username, tamerName);

            // Enviando respostas
            sender.SendDigimons();
            sender.Connection.Send(new Packets.PACKET_MAP_TAMER_AND_DIGIMONS(sender.Tamer));
            sender.Connection.Send(new Packets.PACKET_INVENTARIO(sender.Tamer));

            // Adicionando o Tamer na MapZone
            Emulator.Enviroment.MapZone[sender.Tamer.MapId].Add(sender);
        }
    }
}
