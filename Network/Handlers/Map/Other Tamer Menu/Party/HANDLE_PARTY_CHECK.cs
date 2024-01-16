using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    // Check Party (Mostra o ponto dos membros no minimapa)
    [PacketHandler(Type = PacketType.PACKET_PARTY_CHECK, Connection = ConnectionType.Login)]
    public class HANDLE_PARTY_CHECK : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(10);

            string nick = packet.ReadString(21);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Debug.Print("Solicitante: {0}", sender.Tamer.Name);

            foreach(Client c in Emulator.Enviroment.MapListener.Clients)
                if(c != null && c.Tamer.Name == nick)
                {
                    sender.Connection.Send(new Packets.PACKET_PARTY_CHECK(c.Tamer));
                    return;
                }
        }
    }
}
