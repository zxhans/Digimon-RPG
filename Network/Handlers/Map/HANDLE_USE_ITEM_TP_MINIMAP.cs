using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido ao finalizar o login no map. Requer uma resposta do mesmo tipo vazia, PACKET_CCF0
    [PacketHandler(Type = PacketType.PACKET_USE_ITEM_TP_MINIMAP, Connection = ConnectionType.Map)]
    public class HANDLE_USE_ITEM_TP_MINIMAP : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Lixo
            int unk = packet.ReadInt();
            int unk2 = packet.ReadShort();

            // Posição para onde foi teleportado
            int x = packet.ReadInt();
            int y = packet.ReadInt();
            // Operação
            int op = packet.ReadInt();
            // ID do item no Banco
            int itemId = packet.ReadInt();
            // O pacote contem todas as informações do item. Contudo, apenas o ID já é suficiente neste processo
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando o item
            foreach(Item i in sender.Tamer.Items)
                if(i != null && i.Id == itemId)
                {
                    // Encontramos o item, agora vamos procurar o Digimon
                    if(i.ItemQuant > 0 && i.CheckEffect(90))
                    {
                        sender.Tamer.Location = new Game.Data.Vector2(x, y);
                        sender.Connection.Send(new Packets.PACKET_USE_ITEM_TP_MINIMAP(i, x, y, op));
                        sender.Connection.Send(new Packets.PACKET_CREATE_TAMER(sender.Tamer));
                        sender.Connection.Send(new Packets.PACKET_GAIN_ITEM_INFO(i));
                        MapZone map = Emulator.Enviroment.MapZone[sender.Tamer.MapId];
                        if (map != null)
                            map.SendProximidades(sender);
                        i.Consumir();
                        sender.Tamer.SaveLocation();
                        sender.Tamer.AtualizarInventario();
                    }
                    return;
                }
        }
    }
}
