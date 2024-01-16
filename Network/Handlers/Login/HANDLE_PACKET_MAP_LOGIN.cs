using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    [PacketHandler(Type = PacketType.PACKET_MAP_LOGIN, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_MAP_LOGIN : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();
            string username = packet.ReadString(21).Trim();
            string tamerName = packet.ReadString(20).Trim();
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Console.WriteLine("{0} Selected {1} to enter the server. Remaining: {2}", username, tamerName, packet.Remaining);

            sender.CarregarTamer(username, tamerName);
        }
    }
}
