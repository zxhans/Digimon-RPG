using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using Digimon_Project.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace Digimon_Project.Game
{
    // Classe que armazena itens no chão dos mapas
    public class Party
    {
        public Tamer Lider { get; set; }
        public List<Tamer> Tamers { get; set; }

        // Construtor
        public Party(Tamer tamer)
        {
            Lider = tamer;
            tamer.Party = this;
            Tamers = new List<Tamer>();
        }

        // Adicionando Tamers na Party
        public bool AddTamer(Tamer t)
        {
            if (Tamers.Count < 4)
            {
                Tamers.Add(t);
                t.Party = this;
                return true;
            }

            return false;
        }

        // Removendo Tamers da Party
        public void RemoveTamer(Tamer t)
        {
            // Enviando alteração para os membros
            OutPacket res = new Network.Packets.PACKET_PARTY_KICK(t);

            Lider.Client.Connection.Send(res);
            t.Client.Connection.Send(res);
            foreach (Tamer c in Tamers)
                if (c != null)
                    c.Client.Connection.Send(res);

            if (t != Lider)
            {
                Tamers.Remove(t);
            }
            else
            {
                Lider = null;
            }

            t.Party = null;

            Check();
        }
        public void RemoveTamer(string nick)
        {
            foreach (Tamer t in Tamers)
                if (t != null && t.Name == nick)
                {
                    RemoveTamer(t);
                    return;
                }

            if(Lider != null && Lider.Name == nick)
            {
                RemoveTamer(Lider);
                return;
            }
        }
        public void TamerExit(Tamer t)
        {
            // Enviando alteração para os membros
            OutPacket res = new Network.Packets.PACKET_PARTY_EXIT(t);

            Lider.Client.Connection.Send(res);
            t.Client.Connection.Send(res);
            foreach (Tamer c in Tamers)
                if (c != null)
                    c.Client.Connection.Send(res);

            if (t != Lider)
            {
                Tamers.Remove(t);
            }
            else
            {
                Lider = null;
            }

            t.Party = null;

            Check();
        }

        // Função que verifica se há um Tamer na party com o critério especificado
        public bool CheckTamer(int id)
        {
            if (Lider.Id == id) return true;

            foreach (Tamer t in Tamers)
                if (t != null && t.Id == id)
                    return true;

            return false;
        }

        // Se a party não tem mais membros além do Líder, deve ser desfeita
        private void Check()
        {
            if (Tamers.Count == 0 && Lider != null)
                Lider.Party = null;

            if (Lider == null)
                foreach (Tamer t in Tamers)
                    t.Party = null;
        }

        // Trocando o Líder
        public void NewLider(long GUID)
        {
            // Procurando o membro
            foreach(Tamer t in Tamers)
                if(t.GUID == GUID)
                {
                    // Efetuando a troca
                    Tamers.Add(Lider);
                    Tamers.Remove(t);
                    Lider = t;
                    // Enviando alteração para os membros
                    Network.OutPacket res = new Network.Packets.PACKET_PARTY_INVITY(t.GUID, t.Name, Lider.GUID, Lider.Name, 11);

                    Lider.Client.Connection.Send(res);
                    foreach (Tamer c in Tamers)
                        if (c != null)
                            c.Client.Connection.Send(res);

                    Lider.SendSelf();
                    t.SendSelf();
                    return;
                }
        }

        // Enviando a listagem para os membros
        public void Enviar()
        {
            Network.OutPacket res = new Network.Packets.PACKET_PARTY_LIST(this);
            Lider.Client.Connection.Send(res);

            foreach(Tamer t in Tamers)
                if(t != null)
                    t.Client.Connection.Send(res);
        }

        // Enviado pacote para os membros (Não o líder)
        public void SendPacket(OutPacket packet)
        {
            foreach (Tamer t in Tamers)
                if (t != null)
                    t.Client.Connection.Send(packet);
        }
    }
}
