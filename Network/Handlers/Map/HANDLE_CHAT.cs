using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote que recebe mensagens digitadas no Normal Chat
    [PacketHandler(Type = PacketType.PACKET_CHAT, Connection = ConnectionType.Map)]
    public class HANDLE_CHAT : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // Nick do Tamer que emitiu a mensagem
            string nick = packet.ReadString(21);
            // Mensagem
            int tamanho = packet.Remaining; // Vamos ler tudo o que falta ser lido no pacote
            string text = packet.ReadString(tamanho);

            Console.WriteLine("{1} diz: {0}", text, nick);

            // Verificando se a mensagem foi um comando
            if (!Utils.Comandos.Comando(sender, text))
            {
                MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                if (map != null)
                {
                    foreach(Client c in map.Players)
                        if(c != null)
                        // A variável "tamanho" também especifica o tamanho do pacote a ser enviado
                        c.Connection.Send(new Packets.PACKET_CHAT(nick, text, tamanho));
                }
            }
        }
    }
}
