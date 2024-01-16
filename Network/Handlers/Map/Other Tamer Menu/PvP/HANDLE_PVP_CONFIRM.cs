using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    /**
    // Tamer confirmou a entrada na batalha PvP
    [PacketHandler(Type = PacketType.PACKET_TESTE, Connection = ConnectionType.Map)]
    public class HANDLE_PVP_CONFIRM : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // O Client responde este pacote com exatamente os mesmos dados enviados. Contudo, aqui só precisamos
            // da confirmação.

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            if (sender.Batalha != null)
                sender.Batalha.BattleReady(sender);
        }
    }
    /**/
}
