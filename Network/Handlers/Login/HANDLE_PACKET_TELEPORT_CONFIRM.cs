using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido quando o Clietn confirma Teleport
    [PacketHandler(Type = PacketType.PACKET_TELEPORT_CONFIRM, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_TELEPORT_CONFIRM : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // Nick
            string nick = packet.ReadString(24);

            // Mapa antigo e mapa novo
            int oldMap = packet.ReadInt();
            int newMap = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Este pacote não precisa ser respondido, apenas lido.
            Utils.Comandos.Send(sender, "Pacote PACKET_TELEPORT_CONFIRM");
        }
    }
}
