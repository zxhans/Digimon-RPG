using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote que envia o Inventário completo ao entrar no jogo
    // durante a tela de carregamento. Força uma atualização completa no client.
    public class PACKET_INVENTARIO : OutPacket
    {
        public PACKET_INVENTARIO(Tamer tamer)
            : base(PacketType.PACKET_INVENTARIO)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 96 A9")); // Preenchimento

            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();

            // Cards
            for (int i = 0; i < 24; i++)
                itemWrite.WriteCard(tamer.Cards[i], this);

            // Itens
            for (int i = 0; i < 24; i++)
                itemWrite.WriteItem(tamer.Items[i], this);
        }

        public PACKET_INVENTARIO(Tamer tamer, int nSlot)
            : base(PacketType.PACKET_INVENTARIO)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 96 A9")); // Preenchimento

            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();

            Write(nSlot);

            // Cards
            for (int i = 0; i < 24; i++)
                itemWrite.WriteCard(tamer.Cards[i], this);

            // Itens
            for (int i = 0; i < 24; i++)
                itemWrite.WriteItem(tamer.Items[i], this);
        }
    }
}
