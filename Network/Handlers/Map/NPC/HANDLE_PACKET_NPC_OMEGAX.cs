using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client clicou em alguma opção de algum NPC
    [PacketHandler(Type = PacketType.PACKET_NPC_OMEGAX, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_NPC_OMEGAX : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int unknown = packet.ReadInt();
            short unk = packet.ReadShort();
            // Lendo o ID do NPC
            int op = packet.ReadInt();

            byte[] trash1 = packet.ReadBytes(24);

            int itemidOne = packet.ReadInt();

            byte[] trash2 = packet.ReadBytes(24);

            int itemidTwo = packet.ReadInt();

            //            Utils.Comandos.Send(sender, "ID:" + itemidOne + "| ID: " + itemidTwo + "| OP:" + op);

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(0, 0));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(1, 1));

            //sender.Tamer.RemoveItem("AntiBody X", 1);
            //sender.Tamer.RemoveItem("Evo Card Omegamon 1", 1);

            /*
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(0, 0));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(0, 1));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(0, 2));

            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(1, 0));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(1, 1));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(1, 2));

            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(2, 0));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(2, 1));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX(2, 2));
            //sender.Connection.Send(new Packets.PACKET_USE_ITEM_BOX(1, 1));
            //sender.Connection.Send(new Packets.PACKET_CARD_COMBINE(1));
            //sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(1));

            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(0, 0));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(0, 1));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(0, 2));

            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(1, 0));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(1, 1));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(1, 2));

            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(2, 0));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(2, 1));
            sender.Connection.Send(new Packets.PACKET_NPC_OMEGAX_TRADE(2, 2));
            //sender.Tamer.AtualizarInventario();
            */
        }

    }
}
