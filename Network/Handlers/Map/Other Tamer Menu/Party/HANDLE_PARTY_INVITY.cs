using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Tamer convidando outro para Party
    [PacketHandler(Type = PacketType.PACKET_PARTY_INVITY, Connection = ConnectionType.Map)]
    public class HANDLE_PARTY_INVITY : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6);

            // GUID do Líder
            long GUID = packet.ReadLong();
            string GUIDSufix = packet.ReadString(8);

            // GUID do convidado
            long GUID2 = packet.ReadLong();
            string GUIDSufix2 = packet.ReadString(8);

            int op = packet.ReadInt(); // Operação realizada

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            MapZone Map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];

            if(Map != null)
            switch (op)
            {
                // Convidando
                case 0:
                    // Ambos os Tamers tem que estar no mesmo mapa, e o Tamer que fez o convite não pode estar em party,
                    // ou tem que ser o líder da party
                    if (sender.Tamer.Party == null || sender.Tamer.Party.Lider == sender.Tamer)
                    {
                        foreach (Client c in Map.Players)
                            // O convidade não pode estar em Party
                            if (c != null && c.Tamer.GUID == GUID2 && c.Tamer.Party == null)
                            {
                                c.Connection.Send(new Packets.PACKET_PARTY_INVITY(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                                return;
                            }
                            else
                                Debug.Print("{0} party: {1}", c.Tamer.Name, c.Tamer.Party == null);
                        }
                    break;
                // Convite aceito
                case 1:
                    // Ambos os Tamers tem que estar no mesmo mapa
                    // O Tamer que aceitou o convite não pode estar em Party
                    if (sender.Tamer.Party == null)
                    {
                        foreach (Client c in Map.Players)
                            // O Tamer que fez o convite deve ser o Líder da Party, ou estar criando uma nova Party
                            if (c != null && c.Tamer.GUID == GUID && (c.Tamer.Party == null || c.Tamer.Party.Lider == c.Tamer))
                            {
                                c.Connection.Send(new Packets.PACKET_PARTY_INVITY(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                                sender.Connection.Send(new Packets.PACKET_PARTY_INVITY(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                                Party party = c.Tamer.Party;
                                if(party == null)
                                    party = new Party(c.Tamer);
                                party.AddTamer(sender.Tamer);
                                party.Enviar();
                                c.Tamer.SendSelf();
                                return;
                            }
                    }
                    break;
                // Convite rejeitado
                case 2:
                    // Ambos os Tamers tem que estar no mesmo mapa
                    foreach (Client c in Map.Players)
                        if (c != null && c.Tamer.GUID == GUID)
                        {
                            c.Connection.Send(new Packets.PACKET_PARTY_INVITY(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                            return;
                        }
                    break;
                // Convite expirado
                case 3:
                    // Ambos os Tamers tem que estar no mesmo mapa
                    foreach (Client c in Map.Players)
                        if (c != null && c.Tamer.GUID == GUID2)
                        {
                            c.Connection.Send(new Packets.PACKET_PARTY_INVITY(GUID, GUIDSufix, GUID2, GUIDSufix2, op));
                            return;
                        }
                    break;
                // Líder passando a liderança
                case 11:
                    // O solicitante deve ser o líder da party
                    if(sender.Tamer.Party != null && sender.Tamer.Party.Lider == sender.Tamer)
                        sender.Tamer.Party.NewLider(GUID2);
                    break;
            }
        }
    }
}
