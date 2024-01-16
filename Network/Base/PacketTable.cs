using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Digimon_Project.Enums;

namespace Digimon_Project.Network
{
    // Classe que mapeia os Handles e Packets por ID
    public class PacketTable
    {
        private Dictionary<ConnectionType, Dictionary<PacketType, IHandler>> _packetHandlers = new Dictionary<ConnectionType, Dictionary<PacketType, IHandler>>();

        public PacketTable()
        {
            for (byte i = 0; i < (byte)ConnectionType.Count; i++)
            {
                _packetHandlers[(ConnectionType)i] = new Dictionary<PacketType, IHandler>();
            }

            // Load packets trough reflection because it's cool.
            var assembly = Assembly.GetEntryAssembly();
            var types = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(PacketHandlerAttribute), false).Length > 0);
            foreach (var type in types)
            {
                PacketHandlerAttribute attr = type.GetCustomAttributes(typeof(PacketHandlerAttribute), false).FirstOrDefault() as PacketHandlerAttribute;
                if (attr != null)
                {
                    if (!_packetHandlers[attr.Connection].ContainsKey(attr.Type))
                    {
                        Console.WriteLine("Added: [{2}][{0}]: {1}", attr.Type, type.Name, attr.Connection);
                        _packetHandlers[attr.Connection].Add(attr.Type, (IHandler)Activator.CreateInstance(type));
                    }
                    else
                    {
                        Console.WriteLine("Duplicate Entry found for {0} -> {1}", attr.Type, type);
                    }
                }
            }
        }

        public IHandler Get(ConnectionType connectionType, int index)
        {
            IHandler result = null;
            PacketType packetType = (PacketType)index;

            if (_packetHandlers[connectionType].ContainsKey(packetType))
            {
                return _packetHandlers[connectionType][(PacketType)index];
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format("PacketTable loaded: {0} packet handlers.", _packetHandlers.Count);
        }

        #region Singleton
        private static PacketTable _instance = null;
        public static PacketTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PacketTable();
                }
                return _instance;
            }
        }
        #endregion  
    }
}
