using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    /// <summary>
    /// Map Login Req?
    /// </summary>
    [PacketHandler(Type = PacketType.PACKET_18892, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_18892 : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            uint unknown = packet.ReadUInt();

            Console.WriteLine("PACKET_18892 -> {0}", unknown);

        }
    }
}
