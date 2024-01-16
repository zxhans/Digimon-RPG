using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client selecionou o Digimon que vai passar pelo processo de evolução
    [PacketHandler(Type = PacketType.PACKET_INSERT_DIGMON_EVO, Connection = ConnectionType.Map)]
    public class HANDLE_INSERT_DIGIMON_EVO : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt(); // Preenchimento
            short unk2 = packet.ReadShort();

            int ID = packet.ReadInt(); // ID do Digimon
            // O ID do Digimon selecionado é a única informação disponível neste pacote. Devemos retornar
            // os modelos da linha evolutiva. A política de segurança será feita posteriormente, na execução
            // do processo.

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Respondendo o Client
            foreach(Digimon d in sender.Tamer.Digimon)
            {
                if (d != null && d.Id == ID)
                {
                    // Respondendo com a linha evolutiva, é o próprio Client que faz a primeira validação
                    // de linha requerida para o card em uso.
                    sender.Connection.Send(new Packets.PACKET_INSERT_DIGMON_EVO(d));

                    return;
                }
            }
        }
    }
}
