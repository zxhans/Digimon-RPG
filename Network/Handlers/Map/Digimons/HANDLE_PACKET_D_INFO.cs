using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Este pacote é recebedo quando o client pressiona D e quer visualizar o informativo de status de
    // uma fase específica de seu Digimon.
    [PacketHandler(Type = PacketType.PACKET_D_INFO, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_D_INFO : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt(); // Preenchimento
            short unk2 = packet.ReadShort();

            byte fase = packet.ReadByte(); // byte que informa a fase de evolução que o client solicitou

            string unk3 = packet.ReadString(7);

            int ID = packet.ReadInt(); // ID do Digimon

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Respondendo o Client
            foreach(Digimon d in sender.Tamer.Digimon)
            {
                if (d != null && d.Id == ID)
                {
                    sender.Connection.Send(new Packets.PACKET_D_INFO(fase, d));
                    return;
                }
            }
        }
    }
}
