using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote que atualiza o Inventário DURANTE o jogo. Não tem tela de carregamento, e não serve
    // para o login.
    public class PACKET_INVENTARIO_ATT : OutPacket
    {
        public PACKET_INVENTARIO_ATT(Tamer tamer)
            : base(PacketType.PACKET_INVENTARIO_ATT)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 B7 C7")); // Preenchimento
            Write((double)tamer.Bits); // Bits

            PACKET_ITEM_WRITER itemWrite = new PACKET_ITEM_WRITER();

            // Cards
            for (int i = 0; i < 24; i++)
                itemWrite.WriteCard(tamer.Cards[i], this);

            // Itens
            for (int i = 0; i < 24; i++)
                itemWrite.WriteItem(tamer.Items[i], this);

        }
    }
}
