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
    // Confirmação de posição do tamer
    [PacketHandler(Type = PacketType.PACKET_CONFIRM_LOCATION, Connection = ConnectionType.Login)]
    public class HANDLE_CONFIRMA_POS : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6);
            // Map Id
            int map = packet.ReadInt();
            
            string tamername = packet.ReadString(24);
            int x = packet.ReadInt();
            int y = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Assimilando client, caso não tenho
            if(sender.User == null)
            {
                foreach(Client c in Emulator.Enviroment.MapListener.Clients)
                {
                    if(c.Tamer != null && c.Tamer.Name == tamername)
                    {
                        sender.User = c.User;
                        sender.Tamer = c.Tamer;

                        // Mensagem de boas-vindas
                        // sender.Connection.Send(new Network.Packets.PACKET_TESTE());
                        return;
                    }
                }
            }

            if (sender.Tamer != null && (sender.Tamer.Location.X != x || sender.Tamer.Location.Y != y))
            {
                sender.Tamer.Location.X = x;
                sender.Tamer.Location.Y = y;
                sender.Tamer.SaveLocation(x, y);
            }
        }
    }
}
