using System;
using System.ComponentModel;
using Digimon_Project.Enums;

namespace Digimon_Project.Network
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NPCAttribute : Attribute
    {
        [DefaultValue(NPCMap.NPC_TOY_TOWN_PATAMON)]
        public NPCMap Type { get; set; }
    }
}
