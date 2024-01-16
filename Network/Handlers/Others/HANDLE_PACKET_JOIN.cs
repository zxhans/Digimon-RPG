using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Others
{
    /// <summary>
    /// Map Login Req?
    /// </summary>
    [PacketHandler(Type = PacketType.PACKET_18892, Connection = ConnectionType.Other)]
    public class HANDLE_PACKET_JOIN : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Este pacote não precisa ser respondido, apenas lido. 
            // (Todo pacote precisa ser lido, ainda que não use as informações lidas)
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();


            //sender.Connection.Send(new Packets.PACKET_TESTE());
        }
    }
}
