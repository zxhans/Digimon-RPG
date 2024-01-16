using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;

namespace Digimon_Project.Network.Handlers.Login
{
    [PacketHandler(Type = PacketType.PACKET_972, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_972 : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();
            sender.Connection.Send(new Packets.PACKET_972()); // Let's just respond OK for now.
        }
    }
}
