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
    [PacketHandler(Type = PacketType.PACKET_FFCC, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_FFCC : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Este pacote não precisa ser respondido, apenas lido. 
            // (Todo pacote precisa ser lido, ainda que não use as informações lidas)
            int unknown = packet.ReadInt();
            short unk = packet.ReadShort();
            byte unk2 = packet.ReadByte();
            string caminho = packet.ReadString(128);
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

        }
    }
}
