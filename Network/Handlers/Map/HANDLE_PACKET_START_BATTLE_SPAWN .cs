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
    [PacketHandler(Type = PacketType.HANDLE_PACKET_START_BATTLE_SPAWN, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_START_BATTLE_SPAWN : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Lendo os bytes na frente
            int unk = packet.ReadInt();
            short unk2 = packet.ReadShort();
            int unk3 = packet.ReadInt();

            // Lendo o identificador do Spawn
            long spawn_id = packet.ReadLong();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando a instância do Spawn no MapZone
            int i = sender.Tamer.MapId;
            MapZone map = Emulator.Enviroment.MapZone[i];
            if(map != null)
            {
                foreach(Spawn s in map.spawn)
                {
                    if(s.Id == spawn_id)
                    {
                        // Respondendo o Client
                        sender.Connection.Send(new Packets.PACKET_BATTLE_START_SPAWN(spawn_id));
                        return;
                    }
                }
            }
        }
    }
}
