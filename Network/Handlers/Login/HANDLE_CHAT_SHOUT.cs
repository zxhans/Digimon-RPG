using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    // Pacote que recebe mensagens digitadas no Shout Chat
    [PacketHandler(Type = PacketType.PACKET_CHAT_SHOUT, Connection = ConnectionType.Login)]
    public class HANDLE_CHAT_SHOUT : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // Nick do Tamer que emitiu a mensagem
            string nick = packet.ReadString(21);
            // Level do Tamer
            short Lvl = packet.ReadShort();
            // Mensagem
            string text = packet.ReadString(256);

            short unk3 = packet.ReadShort();
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Console.WriteLine("{1} SHOUT: {0}", text, nick);
            //DISCORD WEBHOOK
            Utils.Http.Post("https://discord.com/api/webhooks/878784809252560926/_g0jnKWnrt1KsLtHZedorvHF1lwRN0_feDSisAzIbNIp8j68Drh3jUQCpwuS6jJypYnY",
                new System.Collections.Specialized.NameValueCollection()
            {
                    {
                        "username", nick
                    },
                    {
                        "content", text
                    }
            });

            // Verificando se a mensagem foi um comando
            if (!Utils.Comandos.Comando(sender, text) && Lvl >= (int)Constants.BraveTamerlvl)
                foreach(Client c in Emulator.Enviroment.LoginListener.Clients)
                    if (c != null)
                        c.Connection.Send(new Packets.PACKET_CHAT_SHOUT(nick, text, Lvl, unk3));
        }
    }
}
