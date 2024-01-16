using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido ao finalizar o login no map. Requer uma resposta do mesmo tipo vazia, PACKET_CCF0
    [PacketHandler(Type = PacketType.PACKET_CCF0, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_CCF0 : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            sender.Connection.Send(new Packets.PACKET_CCF0());

        }
    }
}
