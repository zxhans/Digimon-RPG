using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    // Pacote que recebe mensagens digitadas no Whisper Chat
    [PacketHandler(Type = PacketType.PACKET_CHAT_WHISPER, Connection = ConnectionType.Login)]
    public class HANDLE_CHAT_WHISPER : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // Nick do Tamer que emitiu a mensagem
            string nick = packet.ReadString(21);
            // Nick do remetente
            string remetente = packet.ReadString(21);
            // Mensagem
            string text = packet.ReadString(256);
            
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Console.WriteLine("{1} whisper to {2}: {0}", text, nick, remetente);

            // Verificando se a mensagem foi um comando
            bool ok = false;
            if (!Utils.Comandos.Comando(sender, text))
                foreach (Client c in Emulator.Enviroment.LoginListener.Clients)
                    if (c != null && c.Tamer != null && c.Tamer.Name == remetente)
                    {
                        c.Connection.Send(new Packets.PACKET_CHAT_WHISPER(nick, remetente, text));
                        ok = true;
                    }

            // Remetente não encontrado
            if(!ok)
            sender.Connection.Send(new Packets.PACKET_CHAT_WHISPER(Emulator.Enviroment.GMNick, nick
                , "Tamer " + remetente + " not found."));
        }
    }
}
