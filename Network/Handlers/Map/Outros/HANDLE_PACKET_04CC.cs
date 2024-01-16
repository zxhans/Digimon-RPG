using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido eventualmente durante o jogo. Aparentemente, sem efeito relevante.
    [PacketHandler(Type = PacketType.PACKET_TAMER_LIST, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_04CC : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // (Todo pacote precisa ser lido, ainda que não use as informações lidas)
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            //sender.Connection.Send(new Packets.PACKET_CREATE_TAMER(sender.Tamer));
            sender.Connection.Send(new Packets.PACKET_CHECK_CONNECTION(sender.Tamer));
        }
    }
}
