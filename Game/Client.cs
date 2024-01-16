using Digimon_Project.Database;
using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using Digimon_Project.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Digimon_Project.Game
{
    // Objeto que além de guardar os dados básicos do usuário (Nick, ID, Lista de Tamers, Digimons...)
    // também é responsável por receber e enviar pacotes.
    public class Client : ISocketSession
    {
        public readonly ushort ConnectionSlot = 0;
        public readonly Connection Connection;
        public ConnectionType ConnectionType { get; set; }
        private int port;

        // Informações básicas do usuário
        public UserInformation User { get; set; }
        // Tamer, personagem
        public Tamer Tamer { get; set; }
        // Lista de Tamers
        public List<Tamer> TamerList { get; set; }

        // Controlador de Batalha
        public Batalha Batalha { get; set; }
        public byte Team = 2;

        // Variável que controla autenticidade em processo com warehouse password
        public bool Autentico = false;


        // Construtor da classe
        public Client(ushort connecitonSlot, Socket socket, ConnectionType connectionType, int port)
        {
            ConnectionType = ConnectionType.Unknown;
            TamerList = new List<Tamer>(); // 4 Empty slots.
            Tamer = null;
            User = null;
            this.port = port;

            Console.WriteLine("New {0} connection on slot {2} from {1} port: {3}"
                , connectionType, socket.AddressFamily, connecitonSlot, port);
            Connection = new Connection(socket, port, this);
            Connection.OnDisconnect += Connection_OnDisconnect;
            Connection.OnReceive += Connection_OnReceive;
            ConnectionType = connectionType;
            Connection.BeginReceive(); // Ready to receive.
        }

        // Função para resetar a Batalha
        public void StopBattle()
        {
            Batalha = null;
            Tamer.Slash1 = 0;
            Tamer.Slash2 = 0;
            Tamer.Slash3 = 0;

            if (Tamer.Digimon[0].Health <= 0)
            {
                Tamer.Digimon[0].Digivolver(0);
                Tamer.Digimon[0].Health = Tamer.Digimon[0].MaxHealth / 100;
                Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(Tamer.Digimon[0]));
            }
        }

        // Função que vai receber pacotes
        private void Connection_OnReceive(object sender, PacketReceivedEventArgs e)
        {
            // We received new packets.
            IHandler handler = null;

            // Pacote recebido
            foreach (InPacket p in e.Packets)
            {
                // Procurando um Handler que contenha o ID do pacote recebido
                // O Handler é uma classe que vai tratar os dados recebidos, e responder com um Packet
                handler = PacketTable.Instance.Get(this.ConnectionType, p.PacketId);
                if (handler != null)
                {
                    handler.Handle(this, p);
                }
                else
                {
                    if (Emulator.Enviroment.Teste)
                    {
                        Console.WriteLine("Received unknown Packet: [{1}] 0x{0} port: {2}"
                            , p.PacketId.ToString("X2"), this.ConnectionType, port);

                        Debug.Print("Received unknown Packet: [{1}] 0x{0} port: {2}"
                            , p.PacketId.ToString("X2"), this.ConnectionType, port);
                    }
                }
            }
        }

        private void Connection_OnDisconnect(object sender, EventArgs e)
        {
            Disconect();
        }

        public void Disconect()
        {
            // Disconnect called for this user, do clean-up?
            switch (ConnectionType)
            {
                case ConnectionType.Login:
                    Emulator.Enviroment.LoginListener.Free(ConnectionSlot);
                    Emulator.Enviroment.LoginListener.Clients.Remove(this);

                    Close();
                    break;
                case ConnectionType.Map:
                    Emulator.Enviroment.MapConnections.Remove(User.Username);
                    Emulator.Enviroment.MapListener.Free(ConnectionSlot);
                    Emulator.Enviroment.MapListener.Clients.Remove(this);

                    Close();
                    break;
                case ConnectionType.Other:
                    Emulator.Enviroment.MapListener.Free(ConnectionSlot);
                    break;
                default:
                    if (Emulator.Enviroment.Teste)
                    Console.WriteLine("Unknown ConnectionTyp: {0}, Emulator doesnt know what to do. Port: {1}"
                        , ConnectionType, port);
                    break;
            }
        }

        public void InitializeMapLogin()
        {
            if (ConnectionType != ConnectionType.Map)
                return; // Nope

            Console.WriteLine("We are ready to log-in into the map server!");

        }

        public void CarregarTamer()
        {
            CarregarTamer(User.Username, Tamer.Name);
        }

        public void CarregarTamer(string username, string tamerName)
        {
            LoginResult result = Emulator.Enviroment.Database.Select<LoginResult>("id, username, password, trade_password"
                + ", autoridade"
                , "users", "WHERE username=@username LIMIT 1", new QueryParameters() { { "username", username } });
            if (result.IsValid)
            {
                User = result.User;
                TamerResult tamer = Emulator.Enviroment.Database.Select<TamerResult>(
                    "t.id AS tamer_id, t.slot AS tamer_slot, t.name AS tamer_name, t.model_id AS tamer_model"
                    + ", t.level AS tamer_level, t.map_id, t.location_x, t.location_y, t.battles_total AS tamer_battles"
                    + ", t.battles_won AS tamer_wins"
                    + ", t.rank, t.bits, t.current_exp AS tamer_xp, t.authority"
                    , "tamers AS t"
                    , "WHERE t.account_id=@user_id "
                    + "AND t.name=@tamer_name AND t.is_deleted=0 LIMIT 1",
                    new QueryParameters() { { "user_id", User.Id }, { "tamer_name", tamerName } });
                if (tamer.IsValid 
                    //&& Emulator.Enviroment.MapConnections.Add(this.User.Username, this)
                    )
                {
                    ConnectionType = ConnectionType.Map;
                    Tamer = tamer.Tamer;
                    Tamer.Client = this;

                    // Carregando Inventário
                    ItemsResult items = Emulator.Enviroment.Database.Select<ItemsResult>(
                        "i.id AS item_id, i.slot AS item_slot, c.item_idx AS ItemId"
                        + ", i.quantity AS ItemQuant"
                        + ", c.item_tag AS ItemTag, c.item_type1 AS ItemType, c.item_use_on AS ItemUseOn"
                        + ", c.default_max_quantity AS ItemQuantMax, c.required_level AS ItemtamerLvl"
                        + ", c.effect_type_1 AS ItemEffect1, c.effect_value_1 AS ItemEffect1Value"
                        + ", c.effect_type_2 AS ItemEffect2, c.effect_value_2 AS ItemEffect2Value"
                        + ", c.effect_type_3 AS ItemEffect3, c.effect_value_3 AS ItemEffect3Value"
                        + ", c.effect_type_4 AS ItemEffect4, c.effect_value_4 AS ItemEffect4Value"
                        + ", c.custo"
                        + ", c.name AS ItemName"
                        , "tamer_inventory AS i"
                        , "JOIN item_codex AS c ON c.id = i.item_codex_id AND c.item_tab = 0"
                        + " WHERE i.tamer_id=@tamer_id"
                        , new QueryParameters() { { "tamer_id", tamer.Tamer.Id } }
                        );
                    Tamer.Items = items.itemList;

                    // Carregando Inventário de Cards
                    ItemsResult cards = Emulator.Enviroment.Database.Select<ItemsResult>(
                        "i.id AS item_id, i.slot AS item_slot, c.item_idx AS ItemId"
                        + ", i.quantity AS ItemQuant"
                        + ", c.item_tag AS ItemTag, c.item_type1 AS ItemType, c.item_use_on AS ItemUseOn"
                        + ", c.default_max_quantity AS ItemQuantMax, c.required_level AS ItemtamerLvl"
                        + ", c.effect_type_1 AS ItemEffect1, c.effect_value_1 AS ItemEffect1Value"
                        + ", c.effect_type_2 AS ItemEffect2, c.effect_value_2 AS ItemEffect2Value"
                        + ", c.effect_type_3 AS ItemEffect3, c.effect_value_3 AS ItemEffect3Value"
                        + ", c.effect_type_4 AS ItemEffect4, c.effect_value_4 AS ItemEffect4Value"
                        + ", c.custo"
                        + ", c.name AS ItemName"
                        , "tamer_inventory AS i"
                        , "JOIN item_codex AS c ON c.id = i.item_codex_id"
                        + "  AND c.item_tab = 1"
                        + " WHERE i.tamer_id=@tamer_id"
                        , new QueryParameters() { { "tamer_id", tamer.Tamer.Id } }
                        );
                    Tamer.Cards = cards.itemList;

                    // Carregando Quests
                    QuestsResult quests = Emulator.Enviroment.Database.Select<QuestsResult>(
                        "q.id AS quest_id, q.quest AS QuestName, q.andamento AS Andamento, q.tipo AS Tipo"
                        + ", q.objetivo AS Objetivo"
                        , "quests AS q"
                        , "WHERE q.tamer_id=@tamer_id"
                        , new QueryParameters() { { "tamer_id", tamer.Tamer.Id } }
                        );
                    Tamer.Quests = quests.questList;

                    // Carregando Digimons
                    DigimonsResult digimons = SelectDigimon();

                    Tamer.Digimon = digimons.digimonList;
                    // Calculando atributos dos Digimons
                    foreach (Digimon d in this.Tamer.Digimon)
                    {
                        if (d != null)
                        {
                            d.CarregarEvolutions();
                            d.Tamer = Tamer;

                            // Aproveitando a chamada para iniciar o temporizado de regeneração natural
                            if (d.recTimer == null)
                                d.SetRecTimer();
                        }
                    }

                    Tamer.Atualizar();
                    if (Emulator.Enviroment.Teste)
                        Console.WriteLine("Successfully loaded {0} with Tamer {1} with partner {2}.", User.Username
                        , Tamer.Name, Tamer.Digimon[0].Name);
                    // Keep connection open, we wait for the login server to give us the signal to move to the map server.
                }
                else
                {
                    Connection.Disconnect();
                }
            }
            else
            {
                Connection.Disconnect(); // Force Disconnect.
            }
        }

        // Função que envia para o Tamer as informações Individuais de Cada digimon. (CC B4)
        public void SendDigimons()
        {
            foreach(Digimon d in Tamer.Digimon)
            {
                if (d != null)
                {
                    Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(d));
                }
            }
        }

        // Função para montar o SELECT dos Digimon no banco
        public DigimonsResult SelectDigimon(string extra)
        {
            DigimonsResult digimons = Emulator.Enviroment.Database.Select<DigimonsResult>(
                    "d.id, d.slot AS digimon_slot, d.name AS digimon_name"
                    + ", d.level AS digimon_level, d.xp AS digimon_xp"
                    + ", d.strength AS digimon_strength, d.dexterity AS digimon_dexterity"
                    + ", d.constitution AS digimon_constitution, d.intelligence AS digimon_intelligence"
                    + ", d.c_strength AS digimon_Cstrength, d.c_dexterity AS digimon_Cdexterity"
                    + ", d.c_constitution AS digimon_Cconstitution, d.c_intelligence AS digimon_Cintelligence"
                    + ", d.u_strength AS digimon_Ustrength, d.u_dexterity AS digimon_Udexterity"
                    + ", d.u_constitution AS digimon_Uconstitution, d.u_intelligence AS digimon_Uintelligence"
                    + ", d.m_strength AS digimon_Mstrength, d.m_dexterity AS digimon_Mdexterity"
                    + ", d.m_constitution AS digimon_Mconstitution, d.m_intelligence AS digimon_Mintelligence"
                    + ", d.skill1lvl, d.skill2lvl"
                    + ", d.c_skill1lvl AS Cskill1lvl, d.c_skill2lvl AS Cskill2lvl"
                    + ", d.u_skill1lvl AS Uskill1lvl, d.u_skill2lvl AS Uskill2lvl"
                    + ", d.m_skill1lvl AS Mskill1lvl, d.m_skill2lvl AS Mskill2lvl"
                    + ", d.hp, d.vp, d.evp, d.digimon_id, d.rookie, d.champ, d.ultim, d.mega"
                    + ", d.battles, d.wins"
                    + ", dg.skill1, dg.skill2"
                    + ", dg.str, dg.dex, dg.con, dg.inte, dg.estage, dg.tipo"
                    + ", dg.model AS digimon_model, dg.nome as OriName"
                    , "digimons AS d, digimon AS dg"
                    , "WHERE d.tamer_id=@tamer_id "
                    + "AND d.digimon_id = dg.id "
                    + "AND d.is_deleted=0 "
                    + extra,
                    new QueryParameters() { { "tamer_id", Tamer.Id } });
            return digimons;
        }
        public DigimonsResult SelectDigimon()
        {
            return SelectDigimon("order by slot limit 5");
        }
        public DigimonsResult SelectDigimon(int id)
        {
            return SelectDigimon("AND d.id = '"+id.ToString()+"'");
        }

        // Liberando recursos
        private void Close()
        {
            try
            {
                if (Tamer != null)
                {
                    // Save tamer postion.
                    Tamer.SaveLocation();

                    if (Emulator.Enviroment.MapZone[Tamer.MapId] != null)
                        Emulator.Enviroment.MapZone[Tamer.MapId].Remove(this);

                    // Destruindo Digimons
                    if (Tamer.Digimon != null && Tamer.Digimon.Count > 0)
                        for (int i = 0; i < Tamer.Digimon.Count; i++)
                            if (Tamer.Digimon[i] != null)
                            {
                                Tamer.Digimon[i].recTimer.Close();
                                Tamer.Digimon[i].Tamer = null;
                                Tamer.Digimon[i].Dispose();
                                Tamer.Digimon[i] = null;
                            }
                    Tamer.Digimon.Clear();
                    Tamer.Client = null;
                    Tamer.Dispose();
                    Tamer = null;
                }

                if (TamerList != null)
                {
                    for (int i = 0; i < TamerList.Count; i++)
                    {
                        if (TamerList[i] != null)
                        {
                            if (TamerList[i].Digimon != null)
                            {
                                for (int j = 0; j < TamerList[i].Digimon.Count; j++)
                                {
                                    if (TamerList[i].Digimon[j] != null)
                                    {
                                        if (TamerList[i].Digimon[j].recTimer != null)
                                            TamerList[i].Digimon[j].recTimer.Close();
                                        TamerList[i].Digimon[j].Tamer = null;
                                        TamerList[i].Digimon[j].Dispose();
                                        TamerList[i].Digimon[j] = null;
                                    }
                                }
                                TamerList[i].Digimon.Clear();
                                TamerList[i].Client = null;
                                TamerList[i].Dispose();
                            }
                        }
                    }

                    TamerList.Clear();
                }
            }
            catch (Exception e)
            {

            }
        }

        // Destrutor
        ~Client()
        {
            Debug.Print("Client destruido.");
        }
    }
}
