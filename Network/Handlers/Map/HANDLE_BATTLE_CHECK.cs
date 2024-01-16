using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido durante a batalha
    [PacketHandler(Type = PacketType.PACKET_BATTLE_CHECK, Connection = ConnectionType.Map)]
    public class PACKET_BATTLE_CHECK : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // As informações neste pacote são irrelevantes, visto que a resposta também é um pacote vazio.
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            //sender.Connection.Send(new Packets.PACKET_CREATE_TAMER(sender.Tamer));
            sender.Connection.Send(new Packets.PACKET_BATTLE_CHECK());

            // Este pacote inidica que está tudo ponto para batalha, ou que uma ação foi finalizada.
            // Neste caso, a barra amarela (TP) deve ser reiniciada.
            Batalha batalha = sender.Batalha;
            if(batalha != null)
            {
                batalha.Prosseguir();
            }
        }
    }
    /**/
}
