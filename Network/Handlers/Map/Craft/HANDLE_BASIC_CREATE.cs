using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Processo de criação de crests e digieggs básicos
    [PacketHandler(Type = PacketType.PACKET_BASIC_CREATE, Connection = ConnectionType.Map)]
    public class HANDLE_BASIC_CREATE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6);
            // ID da receita
            byte id_receita = packet.ReadByte();

            Debug.Print("Basic Craft ID recebido: {0}", id_receita);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Processando
            int inventario = 0; // Inventário onde o item será criado
            int x = 0; // Posição x onde o item foi criado
            int y = 0; // posição y onde o item foi criado

            // Primeiro, vamos pegar a receita no banco
            ReceitaCraftResult result = Emulator.Enviroment.Database.Select<ReceitaCraftResult>("titulo, receita"
                , "craft_receitas", "WHERE id=@id", new Database.QueryParameters() { { "id", id_receita } });
            if (result.HasRows)
            {
                // Temos a receita, agora vamos verificar se o client tem os itens necessários
                string[] itens = result.receita.Split('/');
                bool ok = true;
                foreach(string item in itens)
                {
                    string[] i = item.Split('-');
                    if(i.Length == 2)
                    {
                        string name = i[0];
                        int quant = int.Parse(i[1]);

                        if (sender.Tamer.ItemCount(name) < quant)
                        {
                            sender.Connection.Send(new Packets.PACKET_BASIC_CREATE(0, 0, 0, 0));
                            return;
                        }

                    }
                    else
                    {
                        // Deu ruim
                        id_receita = 0;
                        ok = false;
                    }
                }

                // Se chegamos até aqui, o tamer tem os itens necessários. Podemos prosseguir
                if (ok)
                {
                    // Descontando os itens
                    foreach (string item in itens)
                    {
                        string[] i = item.Split('-');
                        if (i.Length == 2)
                        {
                            string name = i[0];
                            int quant = int.Parse(i[1]);

                            if (sender.Tamer.ItemCount(name) >= quant)
                                sender.Tamer.RemoveItem(name, quant);

                        }
                    }

                    // Adicionando o novo item
                    Item novo = sender.Tamer.AddItem(result.titulo, 1, false);

                    // Obtendo as coordenadas do novo item criado
                    x = novo.GetColuna();
                    y = novo.GetLinha();

                    // Enviando atualização de inventário
                    sender.Tamer.AtualizarInventario();
                }
            }
            else
            {
                // Deu ruim
                id_receita = 0;
            }

            // Respondendo o client
            sender.Connection.Send(new Packets.PACKET_BASIC_CREATE(id_receita, inventario, x, y));
        }
    }
}
