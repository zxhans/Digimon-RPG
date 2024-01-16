using System;
using System.Collections.Generic;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    public class LOGIN_TAMERLIST : OutPacket
    {
        private Random rand = new Random(); // Used to 'randomize' the map offset.

        // CC 12 Pacote com a Tamer list do client (Tela de seleção, após o login)
        // Essa função é chamada enviando a lista Tamer dentro do objeto Client
        public LOGIN_TAMERLIST(List<Tamer> tamersList)
            : base(PacketType.LOGIN_TAMERLIST)
        {
            // Cabeçalho
            // 4 x 00
            Write(new byte[4]);
            // Preenchendo com valor padrão
            Write(new byte[] { 0xC4, 0xBE });

            // Percorrendo a lista de tamers (só tem 4 espaços)
            for (byte i = 0; i < 4; i++)
            {
                // Se o índice da lista não for nulo, então chamamos a função abaixo para escrever as informações
                // do Tamer
                if (tamersList.Count > i && tamersList[i] != null)
                {
                    WriteTamer(tamersList[i]);
                }else
                {
                    // Se não há Tamer, devemos preencher o espaço vazio com 00
                    Write((new byte[108])); // Write empty slot.
                }
            }
            /**/


        }

        // Função que escreve as informações do Tamer no pacote
        private void WriteTamer(Tamer tamer)
        {

            Write(new byte[4]); // Byte separador
            Write(tamer.Id); // Original usa o ID. Vamos usar o Slot para que não seja possível
                                      // deletar um tamer que não seja do usuário atual.
            Write((byte)tamer.Model); // Tamer model Id
            Write(tamer.Name, 21); // Name
            Write((ushort)tamer.Level); // Level - UShort
            Write((short)tamer.Location.X); // Location X
            Write((short)tamer.Location.Y); // Location Y
            Write(tamer.Sock);//Sock
            Write(tamer.Shoes);//Shoes
            Write(tamer.Pants);//Pants
            Write(tamer.Glove);//Glove
            Write(tamer.Tshirt);//T-Shirt
            Write(tamer.Jacket);//Jacket
            Write(tamer.Hat);//Hat
            Write(new byte[4]); // Unknown 4 bytes
            Write((short)tamer.Digimon[0].Model); // Digimon Model Id
            Write((ushort)tamer.Digimon[0].Level); // Digimon Level - UShort
            Write(tamer.Digimon[0].Name != "noname" ? 
                tamer.Digimon[0].Name : tamer.Digimon[0].OriName, 20); // Digimon name
            Write(0); // ??
            Write(tamer.Battles); // Total Battles
            Write(tamer.Wins); // Wins
            Write((byte)(tamer.Slot)); // Slot
            Write((byte)tamer.MapId); // Map Id
            Write((ushort)0); // ?
            Write(tamer.Level + tamer.MapId); // Map + 1 + Offset.
        }
    }
}
