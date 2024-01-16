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
    [PacketHandler(Type = PacketType.HANDLE_PACKET_START_BATTLE, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_START_BATTLE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6);

            int op = packet.ReadInt(); // Operação realizada

            byte b;

            switch (op)
            {
                // Contra Spawn
                case 1:
                    // Lendo o identificador do Spawn
                    long spawn_id = packet.ReadLong();
                    long spawn_guid = packet.ReadLong();
                    // Lendo o restante do pacote
                    while (packet.Remaining > 0) b = packet.ReadByte();

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
                                if (!s.Bloqueado && !s.Abatido) // Estou bloqueado?
                                {

                                    OutPacket res = new Packets.PACKET_BATTLE_START(spawn_id);
                                    sender.Connection.Send(res);
                                    /**/
                                    // Se o Client está em Party, devemos enviar o mesmo pacote para os membros
                                    if (sender.Tamer.Party != null)
                                    {
                                        foreach (Tamer t in sender.Tamer.Party.Tamers)
                                            if (t != null)
                                            {
                                                res = new Packets.PACKET_BATTLE_START(spawn_id);
                                                t.Client.Connection.Send(res);
                                            }
                                    }
                                    if (sender.User.Autoridade >= 100)
                                    {
                                        Utils.Comandos.Send(sender, string.Format("[DEBUG] Battle contra o SpawnID: " + "{0}, GUID: {1}", spawn_id, spawn_guid));
                                    }

                                    // Se este spawn tem tempo para respawnar, então deve ser bloquado
                                    if (s.Tempo > 0)
                                    {
                                        s.Bloqueado = true;
                                        map.sendSpawn(s);
                                    }
                                }
                                /**/
                                return;
                            }
                        }
                    }
                    break;

                // Desafio PvP aceito
                case 2:
                    // GUID do desafiante
                    long GUID = packet.ReadLong();
                    string GUIDSufix = packet.ReadString(8);

                    // Lendo o restante do pacote
                    while (packet.Remaining > 0) b = packet.ReadByte();

                    MapZone Map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];

                    if (Map != null)
                        sender.Connection.Send(new Packets.PACKET_PVP_CONFIRM(GUID, GUIDSufix, 2, 2));
                    break;
            }
        }
    }
}
