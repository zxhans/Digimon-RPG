using Digimon_Project.Game;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Digimon_Project.Managers
{
    public class MapConnections
    {
        public readonly ConcurrentDictionary<string, Client> Clients = new ConcurrentDictionary<string, Client>();

        public bool Add(string accountName, Client c)
        {
            accountName = accountName.ToLower();
            if(!Clients.ContainsKey(accountName))
            {
                if ( Clients.TryAdd(accountName, c))
                {
                    Console.WriteLine("Successfully added {0} to the map clients.", c.User.Username);
                    return true;
                }
            }
            return false;
        }

        public void Remove(string accountName)
        {
            accountName = accountName.ToLower();
            Client c = null;
            if (Clients.ContainsKey(accountName))
            {
                Clients.TryRemove(accountName, out c);
                if (c != null)
                {
                    Console.WriteLine("Successfully removed {0} to the map clients.", c.User.Username);
                }
            }
        }

        public Client this[string accountName]
        {
            get
            {
                accountName = accountName.ToLower();
                if (Clients.ContainsKey(accountName))
                {
                    return Clients[accountName];
                }
                return null;
            }
        }
    }
}
