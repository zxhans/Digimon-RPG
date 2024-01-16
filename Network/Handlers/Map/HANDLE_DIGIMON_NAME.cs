using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client solicita alteração em seu Digimon, como o Nome
    [PacketHandler(Type = PacketType.PACKET_DIGIMON_NAME, Connection = ConnectionType.Map)]
    public class HANDLE_DIGIMON_NAME : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(7);

            // Operação executada
            byte op = packet.ReadByte();

            short unk = packet.ReadShort();

            // ID do Digimon
            int ID = packet.ReadInt();

            // Nome
            string name = packet.ReadString(20);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Processando
            byte error = 0;
            foreach(Digimon d in sender.Tamer.Digimon)
                if(d != null && d.Id == ID)
                    switch (op)
                    {
                        // Mudando o nome
                        case 2:
                            if (name == "")
                            {
                                if (d.Name == "noname")
                                    error = 1;
                                else
                                    error = 2;
                            }
                            else
                            {
                                if (d.Name == "noname")
                                {
                                    error = 3;
                                    d.ChangeName(name);
                                }
                                else
                                    error = 2;

                            }
                            break;
                    }

            sender.Connection.Send(new Packets.PACKET_DIGIMON_NAME(error, op, ID, name));

        }
    }
}
