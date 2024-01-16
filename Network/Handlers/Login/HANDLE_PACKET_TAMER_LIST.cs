using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Login
{
    // CC 04 Client solicitou suas informações básicas (Lista de Tamers, Digimons, etc)
    [PacketHandler(Type = PacketType.PACKET_TAMER_LIST, Connection = ConnectionType.Login)]
    public class HANDLE_PACKET_TAMER_LIST : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            Console.WriteLine("Received login request.");

            //if (sender.ConnectionType == ConnectionType.World)
            //    return;

            int unk2 = packet.ReadInt();
            short unk3 = packet.ReadShort();
            long unk = packet.ReadLong();
            string username = packet.ReadString(21);
            string date = packet.ReadString(21);
            string token = packet.ReadString(60);
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Console.WriteLine("Login request for user {0} with date {1}.", username, date);

            // Procurando usuário no banco
            LoginResult result = Emulator.Enviroment.Database.Select<LoginResult>
                ("id, username, password, trade_password, autoridade, warebits, wareexp"
                , "users" , "WHERE username=@username and (secure_login = 1 or secure_token=@token)"
                +" and ban <= 0 LIMIT 1", 
                new QueryParameters() { { "username", username }, { "token", token } });
            if (result.IsValid)
            {
                // Expulsando quem já está logando nesta conta
                foreach (Client c in Emulator.Enviroment.LoginListener.Clients)
                    if (c != null && c.User != null && c.User.Id == result.User.Id)
                    {
                        c.Disconect();
                        break;
                    }
                foreach (Client c in Emulator.Enviroment.MapListener.Clients)
                    if (c != null && c.User != null && c.User.Id == result.User.Id)
                    {
                        c.Disconect();
                        break;
                    }
                // Guardando no objeto Client as informações de usuário
                sender.User = result.User;

                // Procurando no banco a lista de Tamers e Digimons
                TamersResult tamers = Emulator.Enviroment.Database.Select<TamersResult>
                    ("t.id AS tamer_id, t.slot AS tamer_slot, t.name AS tamer_name, t.model_id AS tamer_model"
                    +", t.level AS tamer_level, t.map_id, t.location_x, t.location_y, t.battles_total AS tamer_battles"
                    + ", t.battles_won AS tamer_wins"
                    /**
                    + ", t.sock AS sock, t.shoes AS shoes, t.pants AS pants"
                    + ", t.glove AS glove, t.tshirt AS tshirt, t.jacket AS jacket, t.hat AS hat"
                    /**/
                    
                    + ", t.rank, t.bits, t.medals, t.coins"
                    + ", d.id AS d_id, d.slot AS digimon_slot, d.name AS digimon_name"
                    + ", dg.model AS digimon_model, d.level AS digimon_level, dg.nome AS OriName"
                    , "tamers AS t, digimons AS d, digimon AS dg"
                    
                    , "WHERE d.tamer_id=t.id AND d.is_deleted=0 AND d.slot = 0 AND d.digimon_id = dg.id"
                    + " AND d.digistore = 0"
                    + " AND t.account_id=@user_id"
                    + " AND t.is_deleted=0 LIMIT 4", new QueryParameters() { { "user_id", sender.User.Id } });

                // Salvando a lista no objeto Client
                sender.TamerList = tamers.tamerList;
                Console.WriteLine("Login Request was successfull for {0}.", username);

                // Enviando pacote de resposta
                sender.Connection.Send(new Packets.PACKET_TAMER_LIST(sender));
                
                Emulator.Enviroment.Database.Update("users", new QueryParameters() { { "secure_login", 0 }
                , { "secure_token", token } }, "WHERE id=@user_id"
                , new QueryParameters() { { "user_id", sender.User.Id } });
            }
            else
            {
                Console.WriteLine("Login Request failed for {0}, the user does not exist, or invalid secure_token"
                    , username);
                // sender.Connection.Send(new Packets.PACKET_TAMER_LIST(0));
                sender.Connection.Disconnect(); // Unknown User?
            }           

        }
    }
}
