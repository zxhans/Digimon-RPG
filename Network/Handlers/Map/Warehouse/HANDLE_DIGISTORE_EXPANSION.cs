using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Processo de expansão do Digistorage
    [PacketHandler(Type = PacketType.PACKET_DIGISTORE_EXPANSION, Connection = ConnectionType.Map)]
    public class HANDLE_DIGISTORE_EXPANSION : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Não há informação neste pacote. Ele é apenas o gatilho para o procedimento.
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Processando
            // Resultado do processo
            byte result = 3;
            // [0: Sucesso, 1: Sem item, 2: Todos slots liberados, 3: Error]

            // Primeiro, vamos verificar se o tamer tem o item
            if(sender.Tamer.ItemCount("Storage Expansion") > 0)
            {
                // Temos o item. Agora vamos ver se já não chegamos a capacidade máxima do digistore (100)
                if(sender.Tamer.Digistore < 100)
                {
                    // Tudo certo, podemos executar a expansão
                    sender.Tamer.ExpandirDigistore();
                    sender.Tamer.RemoveItem("Storage Expansion", 1);
                    sender.Tamer.AtualizarInventario();

                    result = 0;
                }
                else
                {
                    result = 2;
                }
            }
            else
            {
                result = 1;
            }

            sender.Connection.Send(new Packets.PACKET_DIGISTORE_EXPANSION(result));
        }
    }
}
