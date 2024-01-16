using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

namespace Digimon_Project.Network.Packets
{
    // Resposta de execução de procedimento de Bits na Warehouse
    public class PACKET_WAREHOUSE_BITS : OutPacket
    {
        public PACKET_WAREHOUSE_BITS(byte type, double bits, double warebits, byte result)
            : base(PacketType.PACKET_WAREHOUSE_BITS)
        {
            Write(new byte[6]);

            Write(type); // Tipo de transação: 1 - Depósito, 2 - Retirada

            Write(new byte[7]);

            Write(bits); // Bits processados
            Write(warebits); // Bits na Warehouse após a transação

            Write(result); // Resultado da transação
                           // 0 -> Error, 1-> Deposito acima do limite, 2-> Quantia maior do que tem, 
                           // 3-> Bits negativo, 4 -> Quantia final acima do limite da warehouse, 
                           // 5->Depositado com Sucesso, 6->Quantia maior do que tem na warehouse, 
                           // 7->Quantia final acima do limite, 8->Sucesso
            
            Write(new byte[7]);
        }
    }
}
