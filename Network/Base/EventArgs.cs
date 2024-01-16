using System;

namespace Digimon_Project.Network
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public readonly InPacket[] Packets;
        public PacketReceivedEventArgs(InPacket[] packets)
        {
            this.Packets = packets;
        }
    }
}
