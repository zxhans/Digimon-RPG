using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote que recebe informação da movimentação do Tamer
    [PacketHandler(Type = PacketType.PACKET_20CC, Connection = ConnectionType.Map)]
    public class HANDLE_MOVEMENT : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            if (sender.Tamer == null)
                return;

            long unknown = packet.ReadLong();
            // Recebendo nova localização
            short location_x = packet.ReadShort();
            short location_y = packet.ReadShort();
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Atualizando a nova localização no objeto Tamer
            sender.Tamer.Location = new Vector2(location_x, location_y);

            // Enviando componentes presentes no mapa
            MapZone Map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
            if(Map != null)
            {
                Map.SendProximidades(sender);
                if (!Map.Teleportar(sender))
                    // Salvando a nova localização no banco
                    sender.Tamer.SaveLocation();
            }

            //Console.WriteLine("Moving {0} to {1}.", sender.Tamer.Name, sender.Tamer.Location);
        }
    }
}
