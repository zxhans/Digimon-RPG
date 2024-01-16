using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 10 Client tentando criar um Tamer
    [PacketHandler(Type = PacketType.PACKET_CREATE_TAMER, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_CREATE_TAMER : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            Console.WriteLine("Crete Tamer request...");
            byte[] output = new byte[0];

            // Lendo o pacote
            // Removendo 4 x 00 do pacote
            byte[] unk = packet.ReadBytes(4);

            // Lendo mais um short inútil no pacote
            short unk2 = packet.ReadShort();

            /**/
            long opCode = packet.ReadLong();
            if (opCode != 1)
            {
                Console.WriteLine("Something is wrong. Ignoring the packet. opCode: {0}", opCode);
                return; // Something is wrong. Ignoring the packet.
            }
            /**/

            // TODO: Check Model Numbers + Names.
            int unknown = packet.ReadInt();
            ushort unknown2 = packet.ReadUShort();


            byte tamerModel = packet.ReadByte();
            string tamerName = packet.ReadString(21).Trim();
            ushort digimonModel = packet.ReadUShort();
            string digimonName = packet.ReadString(21).Trim();

            int unknown3 = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            int DigimonId = 2;
            // ID dos Digimons
            switch (digimonModel)
            {
                case 10: // Gigimon
                    DigimonId = 3;
                    break;
                case 11: // Viximon
                    DigimonId = 4;
                    break;
                case 12: // Gummymon
                    DigimonId = 12;
                    break;
                case 24: // Chibimon
                    DigimonId = 23;
                    break;
                case 27: // Yaamon
                    DigimonId = 26;
                    break;
                case 31: // Hopmon
                    DigimonId = 30;
                    break;
                case 40: // Dorimon
                    DigimonId = 36;
                    break;
            }

            Console.WriteLine("Creation request for a Tamer -> Model: {0}, Name: {1}, Digimon -> Model: {2}, Name: {3}", tamerModel, tamerName, digimonModel, digimonName);

            Packets.PACKET_CREATE_TAMER.ErrorCodes errCode = Packets.PACKET_CREATE_TAMER.ErrorCodes.OK;

            VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                , "tamers", "WHERE name=@tamerName AND deleted_at is null LIMIT 1"
                , new QueryParameters() { { "@tamerName", tamerName } });
            if (!result.HasRows)
            {
                int openSlots = 4 - sender.TamerList.Count();
                if (openSlots > 0)
                {
                    int slot = sender.TamerList.Count();

                    int tamerId = Emulator.Enviroment.Database.Insert<int>("tamers"
                        , new QueryParameters() { { "account_id", sender.User.Id }, { "slot", slot + 1 }
                            , { "name", tamerName }, { "model_id", tamerModel }, { "map_id", 79 }, { "location_x", 60 }
                            , { "location_y", 58 } });
                    int digimonId = Emulator.Enviroment.Database.Insert<int>("digimons", new QueryParameters() {
                        { "tamer_id", tamerId }, { "digimon_id", DigimonId }, { "name", digimonName } });
                    if (tamerId > 0 && digimonId > 0)
                    {

                        Tamer tamer = new Tamer((byte)(slot + 1), tamerId) // fill w/ slot Number
                        {
                            Name = tamerName,
                            Model = tamerModel,
                            Level = 1,
                            MapId = 79,
                            Location = new Game.Data.Vector2(60, 58),
                            Digimon = new List<Digimon>()
                            {
                                new Digimon((byte)(slot + 1), digimonId)
                                {
                                    Name = digimonName,
                                    Model = digimonModel,
                                    Level = 1
                                }
                            }
                        };

                        sender.TamerList.Add(tamer); // Store tamer in memory.

                        output = new Packets.PACKET_CREATE_TAMER(errCode, unknown, unknown2, tamer).GetBuffer()
                            .Concat(new Packets.LOGIN_TAMERLIST(sender.TamerList).GetBuffer()).ToArray();

                    }
                    else
                    {
                        // Couldn't create the tamer.
                        errCode = Packets.PACKET_CREATE_TAMER.ErrorCodes.TamerNameInUse;
                    }
                }
                else
                {
                    // No open slots.
                    Console.WriteLine("No open slots found -> Ignoring the packet!!");
                    return;
                    //errCode = Packets.PACKET_4300.ErrorCodes.TamerNameInUse; // TEMP
                }
            }
            else
            {
                errCode = Packets.PACKET_CREATE_TAMER.ErrorCodes.TamerNameInUse;
            }

            if (errCode != Packets.PACKET_CREATE_TAMER.ErrorCodes.OK)
                output = new Packets.PACKET_CREATE_TAMER(errCode, unknown, unknown2, tamerModel
                , tamerName, digimonModel, digimonName).GetBuffer();

            sender.Connection.Send(output);

        }
    }
}
