using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Tamer desafiou outro Tamer para batalha
    [PacketHandler(Type = PacketType.PACKET_PVP_DESAFIO, Connection = ConnectionType.Map)]
    public class HANDLE_PVP_DESAFIO : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6);

            // GUID do desafiante
            long GUID = packet.ReadLong();
            string GUIDSufix = packet.ReadString(8);

            // GUID do desafiado
            long GUID2 = packet.ReadLong();
            string GUIDSufix2 = packet.ReadString(8);

            int op = packet.ReadInt(); // Operação realizada

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            MapZone Map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];

            if(Map != null)
            switch (op)
            {
                // Desafiando
                case 0:
                    // Ambos os Tamers tem que estar no mesmo mapa, e não podem estar em batalha
                    if (sender.Batalha == null && sender.Trade == null)
                    {
                        foreach (Client c in Map.Players)
                            if (c != null && c.Tamer.GUID == GUID2)
                            {
                                c.Connection.Send(new Packets.PACKET_PVP_DESAFIO(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                                return;
                            }
                    }
                    break;
                // Desafio aceito
                case 1:
                    // Ambos os Tamers tem que estar no mesmo mapa, e não podem estar em batalha
                    if (sender.Batalha == null && sender.Trade == null)
                    {
                        foreach (Client c in Map.Players)
                            if (c != null && c.Batalha == null && c.Trade == null && c.Tamer.GUID == GUID)
                            {
                                c.Connection.Send(new Packets.PACKET_PVP_DESAFIO(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                                sender.Connection.Send(new Packets.PACKET_PVP_DESAFIO(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                                Batalha batalha = new Batalha(sender, c);
                                return;
                            }
                    }
                    break;
                // Desafio rejeitado
                case 2:
                    // Ambos os Tamers tem que estar no mesmo mapa
                    foreach (Client c in Map.Players)
                        if (c != null && c.Tamer.GUID == GUID)
                        {
                            c.Connection.Send(new Packets.PACKET_PVP_DESAFIO(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                            return;
                        }
                    break;
                // Desafio expirado
                case 3:
                    // Ambos os Tamers tem que estar no mesmo mapa
                    foreach (Client c in Map.Players)
                        if (c != null && c.Tamer.GUID == GUID2)
                        {
                            c.Connection.Send(new Packets.PACKET_PVP_DESAFIO(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                            return;
                        }
                    break;
            }
        }
    }
}
