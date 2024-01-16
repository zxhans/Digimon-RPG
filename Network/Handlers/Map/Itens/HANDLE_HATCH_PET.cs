using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client está chocando um Pet Egg
    [PacketHandler(Type = PacketType.PACKET_HATCH_PET, Connection = ConnectionType.Map)]
    public class HANDLE_HATCH_PET : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6);

            int op = packet.ReadInt(); // Operação
            int id = packet.ReadInt(); // ID do item no banco
            int idx = packet.ReadInt(); // ID do item no codex

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Verificando se o item exite
            foreach(Item i in sender.Tamer.Items)
                if(i != null && i.Id == id && i.ItemEffect1 == 71)
                {
                    sender.Tamer.Pet = i.ItemEffect1Value;
                    sender.Tamer.PetHP = 5000;
                    sender.Tamer.SavePet();
                    i.Consumir();

                    sender.Connection.Send(new Packets.PACKET_HATCH_PET(id, idx));
                    sender.Connection.Send(new Packets.PACKET_PET_ATT(sender.Tamer));

                    sender.Tamer.AtualizarInventario();

                    return;
                }
        }
    }
}
