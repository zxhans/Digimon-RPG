using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client clicou em spawn para começar a batalhar
    [PacketHandler(Type = PacketType.PACKET_BATTLE_START, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_JOINED_BATTLE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Lendo os bytes na frente
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();

            // Lendo o identificador do Spawn
            long spawn_id = packet.ReadLong();
            // Lendo o restante do identificador
            long spawn_id_res = packet.ReadLong();

            // 4 Bytes
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Se já temos uma batalha criada, então é PvP. Devemos anunciar que estamos prontos
            if (sender.Batalha != null)
            {
                sender.Batalha.BattleReady(sender);
                return;
            }

            // Se chegamos até aqui, então estamos batalhando contra um spawn
            // Em caso de batalha contra Spawn, o Tamer não deve estar em party, ou ele deve ser o líder da Party
            if (sender.Tamer.Party == null || sender.Tamer.Party.Lider == sender.Tamer)
            {
                // Procurando a instância do Spawn no MapZone
                int i = sender.Tamer.MapId;
                MapZone map = Emulator.Enviroment.MapZone[i];
                if (map != null)
                {
                    foreach (Spawn s in map.spawn)
                    {
                        if (s.Id == spawn_id)
                        {
                            // Respondendo o Client
                            Batalha batalha = new Batalha(sender, s);
                            return;
                        }
                    }
                }
            }

            // Respondendo o Client
            //sender.SendDigimons();
            //sender.Connection.Send(new Packets.PACKET_BATTLE_CENARY(sender.Tamer));
            //sender.Connection.Send(new Packets.PACKET_CREATE_TAMER(sender.Tamer));
        }
    }
    /**/
}
