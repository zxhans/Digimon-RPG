using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

namespace Digimon_Project.Network.Packets
{
    // Resposta aos processos no Digistore
    public class PACKET_DIGISTORE : OutPacket
    {
        public PACKET_DIGISTORE(short pagina, short type, int deposito, int retirada, double custo
            , double bits, byte result, Digimon d)
            : base(PacketType.PACKET_DIGISTORE)
        {
            Write(new byte[6]);

            // Página atual
            Write(pagina);

            // Tipo de operação
            Write(type);

            // Id dos Digimons depositado e retirad
            Write(deposito);
            Write(retirada);

            Write(new byte[4]);

            // Custo do procedimento
            Write(custo);

            // Bits com o Tamer após a transação
            Write(bits);

            // Resultado da transação
            Write(result);
            // 0:error, 1: sucesso abertura, 2: falha abertura, 3: sucesso deposito, 4: falha deposito, 
            // 5: falha bits deposito, 6: retirada com sucesso, 
            // 7: falha retirada, 8: retirada falha bits, 9: troca com sucesso, 10: troca falha, 11: troca falha bits

            Write(new byte[7]);

            // Digimon processado
            PACKET_DIGIMON_WRITER writer = new PACKET_DIGIMON_WRITER();
            if (d != null)
                writer.WriteDigimon(d, this);
            else
                Write(new byte[520]);
        }
    }
}
