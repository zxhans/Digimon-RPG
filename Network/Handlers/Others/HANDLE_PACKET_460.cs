using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;

namespace Digimon_Project.Network.Handlers.Others
{
    /// <summary>
    /// Map Login Req
    /// </summary>
    [PacketHandler(Type = PacketType.PACKET_460, Connection = ConnectionType.Other)]
    public class HANDLE_PACKET_460 : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            string tamerName = packet.ReadString(21);
            string username = packet.ReadString(22);

            Console.WriteLine("Login request for tamer {0} by username {1}.", tamerName, username);

            LoginResult result = Emulator.Enviroment.Database.Select<LoginResult>("id, username, password, trade_password", "users", "WHERE username=@username LIMIT 1", new QueryParameters() { { "username", username } });
            if (result.IsValid)
            {
                sender.User = result.User;
                TamerResult tamer = Emulator.Enviroment.Database.Select<TamerResult>("t.id AS tamer_id, t.slot AS tamer_slot, t.name AS tamer_name, t.map_id, t.model_id AS tamer_model, t.level AS tamer_level, t.xp AS tamer_xp, t.location_x, t.location_y, t.battles AS tamer_battles, t.wins AS tamer_wins, d.id AS digimon_id, d.slot AS digimon_slot, d.name AS digimon_name, d.model_id AS digimon_model, d.level AS digimon_level, d.xp AS digimon_xp, d.strength AS digimon_strength, d.dexterity AS digimon_dexterity, d.constitution AS digimon_constitution, d.intelligence AS digimon_intelligence", "tamers AS t", "JOIN digimons AS d ON d.tamer_id=t.id AND d.is_deleted=0 WHERE t.owner_id=@user_id AND t.name=@tamer_name AND t.is_deleted=0 LIMIT 1", new QueryParameters() { { "user_id", sender.User.Id }, { "tamer_name", tamerName } });
                if (tamer.IsValid && Emulator.Enviroment.MapConnections.Add(sender.User.Username, sender))
                {
                    sender.ConnectionType = ConnectionType.Map;
                    sender.Tamer = tamer.Tamer;
                    tamer.Tamer.Digimon.Calculate();
                    Console.WriteLine("Successfully loaded {0} with Tamer {1} with partner {2}.", sender.User.Username, sender.Tamer.Name, sender.Tamer.Digimon.Name);
                    // Keep connection open, we wait for the login server to give us the signal to move to the map server.
                }
                else
                {
                    sender.Connection.Disconnect();
                }
            }
            else
            {
                sender.Connection.Disconnect(); // Force Disconnect.
            }

        }
    }
}
