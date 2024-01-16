using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 64 Recebido ao finalizar o Login
    [PacketHandler(Type = PacketType.PACKET_CCDE, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_CCDE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            int unk2 = packet.ReadInt();
            short unk3 = packet.ReadShort();

            // informação aparentemente revelante
            string tamername = packet.ReadString(21);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();
        }
    }
}
