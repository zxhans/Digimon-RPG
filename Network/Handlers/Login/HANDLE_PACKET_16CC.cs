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
    [PacketHandler(Type = PacketType.PACKET_16CC, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_16CC : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            string username = packet.ReadString(21).Trim();
            string tamerName = packet.ReadString(20).Trim();

            Console.WriteLine("{0} Selected {1} to enter the server.‬", username, tamerName);
            //sender.ConnectionType = ConnectionType.Login7000;

            // Going to do an ugly hack. Cuz MySQL is slower on the other connection, we should queue this request up in the main thread.
            System.Threading.Thread.Sleep(500);

            if (username == sender.User.Username && sender.TamerList.Where(t => t != null && t.Name == tamerName).Count() > 0)
            {
                Client c = Emulator.Enviroment.MapConnections[sender.User.Username];
                if (c.Tamer != null && c.Tamer.Name == tamerName)
                {
                    // OK! -> Send the map server info!
                    c.InitializeMapLogin();
                }
            }
        }
    }
}
