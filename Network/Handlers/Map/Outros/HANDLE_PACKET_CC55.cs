using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido ao finalizar o login no map. Requer uma resposta do mesmo tipo vazia, PACKET_CCF0
    [PacketHandler(Type = PacketType.PACKET_CC55, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_CC55 : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(10);
            int id = packet.ReadInt();

            byte[] trash2 = packet.ReadBytes(packet.Remaining);

            sender.Connection.Send(new Packets.PACKET_CC55(id, trash2));

        }
    }
}
