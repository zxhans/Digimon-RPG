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
    [PacketHandler(Type = PacketType.PACKET_18CC, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_18CC : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            string macAddress1 = packet.ReadString(20);
            string macAddress2 = packet.ReadString(20);
            string macAddress3 = packet.ReadString(20);

            Console.WriteLine("Received MAC Addresses: {0}", macAddress1);
            Console.WriteLine("Received MAC Addresses: {0}", macAddress2);
            Console.WriteLine("Received MAC Addresses: {0}", macAddress3);
        }
    }
}
