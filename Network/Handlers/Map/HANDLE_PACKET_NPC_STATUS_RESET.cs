using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client usou o Reset Status (NPC D)
    [PacketHandler(Type = PacketType.PACKET_NPC_RESET_STATUS, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_NPC_STATUS_RESET : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6); // Preenchimento

            // ID do Digimon
            int ID = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando o Digimon
            foreach(Digimon d in sender.Tamer.Digimon)
            {
                if (d != null && d.Id == ID)
                {
                    int preco = d.Level * 1000;
                    if(sender.Tamer.Bits >= preco)
                    {
                        d.ResetStatus();
                        sender.Tamer.GainBit(-preco);
                        // Respondendo o Client
                        sender.Connection.Send(new Packets.PACKET_NPC_RESET_STATUS(ID, sender.Tamer.Bits, 1));
                        sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(d));
                    }
                    else // Falha - Sem Bits
                        sender.Connection.Send(new Packets.PACKET_NPC_RESET_STATUS(ID, 0, 2));

                    return;
                }
            }
        }
    }
}
