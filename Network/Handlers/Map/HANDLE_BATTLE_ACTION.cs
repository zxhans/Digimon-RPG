using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido durante a batalha
    // Tamer executou uma ação
    [PacketHandler(Type = PacketType.PACKET_BATTLE_ACTION, Connection = ConnectionType.Map)]
    public class HANDLE_BATTLE_ACTION : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            //Lendo o pacote
            int Click = packet.ReadInt(); // Número de vezes que o client clicou a mesma ação, subtraído de 7
            short unk = packet.ReadShort();
            byte unk2 = packet.ReadByte();

            // ID da ação solicitada
            byte act = packet.ReadByte();

            short unk3 = packet.ReadShort();

            // ID do Digimon que solicitou a ação
            long id = packet.ReadLong();
            string sufix = packet.ReadString(8); // Restante do ID

            // ID do Alvo
            long alvo = packet.ReadLong();
            string alvo_sufix = packet.ReadString(8); // Restante do ID do alvo

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Procurando a instância da Batalha on o Client está envolvido
            Batalha batalha = sender.Batalha;
            if(batalha != null)
            {
                batalha.Action(act, id, sufix, alvo, alvo_sufix, sender);
            }
        }
    }
    /**/
}
