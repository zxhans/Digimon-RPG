using Digimon_Project.Database;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

using System;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 11 Cliente está deletando um Tamer
    [PacketHandler(Type = PacketType.PACKET_DELETE_TAMER, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_DELETE_TAMER : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Removendo os bytes da frente
            byte[] output = new byte[0];
            byte[] unk2 = packet.ReadBytes(4);
            long unk = packet.ReadLong();
            byte[] unk3 = packet.ReadBytes(2);

            short tamerId = packet.ReadShort(); // Slot ID
            string passwordMd5 = packet.ReadString(40);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Console.WriteLine("Deletando TamerId: {0} pass: {1} unk: {2}", tamerId, passwordMd5, unk);

            foreach(Tamer t in sender.TamerList)
            {
                if (t != null && t.Id == tamerId)
                {
                    if (passwordMd5.Equals(sender.User.TradePassword))
                    {
                        Emulator.Enviroment.Database.Update("tamers", new QueryParameters() { { "is_deleted", true } }
                            , "WHERE account_id=@owner_id AND id=@tamer_id LIMIT 1"
                            , new QueryParameters() { { "@owner_id", sender.User.Id }
                            , { "tamer_id", t.Id } }, "deleted_at=CURRENT_TIMESTAMP()");
                        Console.WriteLine("Deleted Tamer[{0}] {1} with Digimon {2}."
                            , tamerId, t.Name, t.Digimon[0].Name);
                        // Grab row id from memory and flag tamer as deleted -> Allows rollbacks.
                        // Run database queries to delete the selected slot.
                        sender.TamerList.Remove(t); // delete from memory.
                        //sender.TamerList.Sort();
                                  // (new Packets.PACKET_4556((byte)tamerId, passwordMd5).GetBuffer().Concat(new Packets.PACKET_4812(sender.TamerList).GetBuffer())).ToArray() // Original
                        sender.Connection.Send(new Packets.LOGIN_TAMERLIST(sender.TamerList).GetBuffer()); // Save some bytes.
                    }
                    else
                    {
                        // Invalid Password response.
                        sender.Connection.Send(new Packets.PACKET_DELETE_TAMER(3, tamerId, passwordMd5));
                    }
                    return;
                }
            }

        }
    }
}
