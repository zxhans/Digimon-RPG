using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client inseriu pontos em algum atributo
    [PacketHandler(Type = PacketType.PACKET_DIGIMON_INCR_ATRIBUTE, Connection = ConnectionType.Map)]
    public class HANDLE_DIGIMON_INCR_ATRIBUTE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // ID do Digimon
            int ID = packet.ReadInt();

            // Entre o ID e a próxima informação relevante, há o GUID e um grande bloco vazio
            string trash = packet.ReadString(516);

            // Fase onde foi passado e o atributo
            byte estage = packet.ReadByte();
            byte atribute = packet.ReadByte();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procuranto o Digimon no tamer
            foreach(Digimon d in sender.Tamer.Digimon)
            {
                if(d != null && d.Id == ID)
                {
                    d.AddPoint(estage, atribute);
                    sender.Connection.Send(new Packets.PACKET_DIGIMON_INCR_ATRIBUTE(d, estage, atribute));
                    //sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(d));
                    return;
                }
            }
        }
    }
}
