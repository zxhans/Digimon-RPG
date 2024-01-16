using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace Digimon_Project.Game
{
    public class MapZone
    {
        public readonly int Id;
        public List<Spawn> spawn;
        private Timer aTimer;
        public List<ItemMap> Items = new List<ItemMap>();
        public List<Teleport> Teleports = new List<Teleport>();
        private int Distance = 20; // Distância mínima para mostrar componentes no mapa
        private int TPDistance = 2; // Distância mínima para Teleportar

        public readonly List<Client> Players = new List<Client>();

        public MapZone(int id)
        {
            Id = id;
        }

        public void Add(Client c)
        {
            if (!Players.Contains(c)) Players.Add(c);

            // Enviando Spawn para o Tamer que acabou de entrar na sala
            if (spawn == null)
                Spawn();

            if (Teleports.Count == 0)
                CarregarTeleports();

            SendProximidades(c);
        }

        public void Remove(Client c)
        {
            Players.Remove(c);
        }

        // Enviando tudo o que há nas proximidades para um Client
        public void SendProximidades(Client c)
        {
            sendSpawn(c);
            sendItem(c);
            SendTamer(c);
        }

        // Carregando spawn do mapa no banco
        public void Spawn()
        {
            SpawnResult spawnResult = Emulator.Enviroment.Database.Select<SpawnResult>("s.id, s.name"
                + ", s.lvl AS level, s.map_id, s.x AS location_x"
                + ", s.y AS location_y, s.rank, s.move, s.quant, s.mquant, s.digimon_id, s.item_drop"
                + ", d.tipo AS type, d.str, d.dex, d.con, d.inte, d.estage, d.skill1, d.skill2, d.model"
                , "spawn_digimons s, digimon d"
                , "WHERE s.map_id=@id AND s.digimon_id = d.id"
                , new Database.QueryParameters() { { "id", Id} });

            spawn = spawnResult.spawns;

            if (spawn.Count > 0) SetTimer();
        }

        // Carregando Teleports do mapa no banco
        public void CarregarTeleports()
        {
            TeleportsResult spawnResult = Emulator.Enviroment.Database.Select<TeleportsResult>("id, mapid"
                +", posx, posy, alvo, alvox, alvoy"
                , "teleports"
                , "WHERE mapid=@id AND alvo is not null"
                , new Database.QueryParameters() { { "id", Id } });

            Teleports = spawnResult.teleports;
        }

        // Startando temporizador, que vai processar o Spawn a cada 5 segundos
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(5000); // 5000 = 5 segundos
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += MoveSpawn;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        // Função que movimenta o spawn
        private void MoveSpawn(Object source, ElapsedEventArgs e)
        {
            foreach(Spawn s in spawn)
            {
                if (s.move == 1)
                {
                    Random random = new Random(DateTime.Now.Millisecond + (int)((s.lastPos.X + s.lastPos.Y) * 100));
                    if (random.Next(1, 100) >= 60)
                    {
                        s.lastPos.X = s.Pos.X;
                        s.lastPos.Y = s.Pos.Y;
                        int dist = 2;
                        s.Pos.X = random.Next((int)s.Location.X - dist, (int)s.Location.X + dist);
                        s.Pos.Y = random.Next((int)s.Location.Y - dist, (int)s.Location.Y + dist);
                        sendSpawn(s);
                    }
                }
            }
        }


        // Função que envia o pacote de spawn para os clients no mapa
        public void sendSpawn(Spawn s, Client c)
        {
            if (c.Tamer.Location.Distance(s.Location) <= Distance)
                c.Connection.Send(new Network.Packets.PACKET_SPAWN(s));
        }
        public void sendSpawn(Spawn s)
        {
            foreach(Client c in Players)
                if(c.Tamer.Location.Distance(s.Location) <= Distance)
                    sendSpawn(s, c);
        }
        public void sendSpawn(Client c)
        {
            foreach (Spawn s in spawn)
                sendSpawn(s, c);
        }

        // Função que envia o pacote de Itens no chão para os clients no mapa
        public void sendItem(ItemMap i, Client c)
        {
            if (c.Tamer.Location.Distance(i.Location) <= Distance)
            {
                if(i.Item.ItemTab == 0)
                    c.Connection.Send(new Network.Packets.PACKET_ITEM_DROP(i));
                else

                    c.Connection.Send(new Network.Packets.PACKET_CARD_DROP(i));
            }
        }
        public void sendItem(ItemMap i)
        {
            foreach (Client c in Players)
                if (c.Tamer.Location.Distance(i.Location) <= Distance)
                    sendItem(i, c);
        }
        public void sendItem(Client c)
        {
            foreach (ItemMap i in Items)
                sendItem(i, c);
        }
        public void removeItem(Vector2 pos)
        {
            foreach (Client c in Players)
                if (c.Tamer.Location.Distance(pos) <= Distance)
                    c.Connection.Send(new Network.Packets.PACKET_ITEM_DROP((short)pos.X
                                    , (short)pos.Y));
        }

        // Função que teleporta o Client, caso ele pise no teleport
        public bool Teleportar(Teleport t, Client c)
        {
            if (c.Tamer.Location.Distance(t.Location) <= TPDistance)
            {
                c.Tamer.Teleport(t);
                return true;
            }

            return false;
        }
        public bool Teleportar(Client c)
        {
            foreach (Teleport t in Teleports)
                if(Teleportar(t, c)) return true;

            return false;
        }

        // Função que envia Tamer no mapa para os outros tamers
        public void SendTamer(Tamer t, Client c)
        {
            if (c.Tamer != t && c.Tamer.Location.Distance(t.Location) <= Distance)
                c.Connection.Send(new Network.Packets.PACKET_CREATE_TAMER(t));
        }
        public void SendTamer(Client c)
        {
            foreach (Client t in Players)
                SendTamer(t.Tamer, c);
        }
        public void SendTamer(Tamer t)
        {
            foreach (Client c in Players)
                SendTamer(t, c);
        }
    }
}
