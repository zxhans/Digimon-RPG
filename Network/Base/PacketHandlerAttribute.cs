using System;
using System.ComponentModel;
using Digimon_Project.Enums;

namespace Digimon_Project.Network
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketHandlerAttribute : Attribute
    {
        [DefaultValue(PacketType.Unknown)]
        public PacketType Type { get; set; }
        [DefaultValue(ConnectionType.Unknown)]
        public ConnectionType Connection { get; set; }
    }
}
