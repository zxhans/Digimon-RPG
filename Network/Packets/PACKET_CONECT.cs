using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{
    // Pacote contendo a autorização de conexão para o client
    public class PACKET_CONECT : OutPacket
    {
        public PACKET_CONECT()
            : base(PacketType.PACKET_CONECT)
        {
            /**
                CC 03 00 00 42 00 00 00 00 00 00 00 C4 DE          
                0000  02 00 00 00 00 00 00 00 00 00 00 00 D3 F5 D7 A3   
                0010  F1 FD D0 FC E7 E9 CE FA 00 00 00 00 00 00 00 00 
                0020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
                0030  00 00 00 00    

                Esta é a resposta que deve ser retornada. Contudo, os 8 primeiros pares já fazem parte do protocolo
                de comunicação TCP. Portando, não é necessário enviá-los. CC 03 00 00 42 00 00 00
                Logo, a mensagem deve ser escrita a partir do 9º par.
            /**/

            Write(new byte[4]); // Escrevendo um array de Byte especificando apenas o tamanho sem iniciar o conteúdo
                                // Dessa forma, será escrito um bloco preenchido com quantidade de pares de zeros
                                // igual ao tamanho do array byte. Neste caso, será escrito: 00 00 00 00
            // Este pacote é padrão. Todos os valores aqui são apenas preenchimentos.
            Write(new byte[] { 0xC4, 0xDE, 0x02 });
            Write(new byte[11]);
            Write(new byte[] { 0xD3, 0xF5, 0xD7, 0xA3, 0xF1, 0xFD, 0xD0, 0xFC, 0xE7, 0xE9, 0xCE, 0xFA });
            Write(new byte[28]);
            /**/
        }
    }
}
