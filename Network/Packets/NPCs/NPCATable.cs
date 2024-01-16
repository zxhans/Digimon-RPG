using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Digimon_Project.Enums;
using Digimon_Project.Game;

namespace Digimon_Project.Network
{
    // Classe que mapeia os Handles e Packets por ID
    public class NPCATable
    {
        private Dictionary<NPCMap, NPC> _npcMaps = new Dictionary<NPCMap, NPC>();

        public NPCATable()
        {
            // Load packets trough reflection because it's cool.
            var assembly = Assembly.GetEntryAssembly();
            var types = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(NPCAttribute), false).Length > 0);
            foreach (var type in types)
            {
                NPCAttribute attr = type.GetCustomAttributes(typeof(NPCAttribute), false).FirstOrDefault() as NPCAttribute;
                if (attr != null)
                {
                    if (!_npcMaps.ContainsKey(attr.Type))
                    {
                        Console.WriteLine("NPC Added: [{0}]: {1}", attr.Type, type.Name);
                        _npcMaps.Add(attr.Type, (NPC)Activator.CreateInstance(type));
                    }
                    else
                    {
                        Console.WriteLine("Duplicate Entry found for {0} -> {1}", attr.Type, type);
                    }
                }
            }
        }

        public NPC Get(NPCMap npcId)
        {
            NPC result = null;
            if (_npcMaps.ContainsKey(npcId))
            {
                return _npcMaps[npcId];
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format("NPCTable loaded: {0} NPCs.", _npcMaps.Count);
        }

        #region Singleton
        private static NPCATable _instance = null;
        public static NPCATable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NPCATable();
                }
                return _instance;
            }
        }
        #endregion  
    }
}
