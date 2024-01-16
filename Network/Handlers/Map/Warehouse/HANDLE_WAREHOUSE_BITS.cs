using Digimon_Project.Enums;
using Digimon_Project.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client está executando transação de Bits na Warehouse
    [PacketHandler(Type = PacketType.PACKET_WAREHOUSE_BITS, Connection = ConnectionType.Map)]
    public class HANDLE_WAREHOUSE_BITS : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Lendo o pacote
            byte[] trash = packet.ReadBytes(6);

            // Tipo de transação
            byte type = packet.ReadByte(); // 1 - Depósito, 2 - Retirada

            byte[] trash2 = packet.ReadBytes(7);

            // Bits com o Tamer
            double bits = packet.ReadDouble();

            // Bits na Warehouse 
            double warebits = packet.ReadDouble();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            Debug.Print("Type: {0}, bits: {1}, warebits: {2}", type, bits, warebits);

            // Resultado da transação
            byte result = 0;
            // 0 -> Error, 1-> Deposito acima do limite, 2-> Quantia maior do que tem, 
            // 3-> Bits negativo, 4 -> Quantia final acima do limite da warehouse, 
            // 5->Depositado com Sucesso, 6->Quantia maior do que tem na warehouse, 
            // 7->Quantia final acima do limite, 8->Sucesso

            // Processando solicitação
            // Depósito
            if (type == 1)
            {
                // Primeiro vamos ver se o Tamer possui mesmo o valor a ser depositado
                if(sender.Tamer.Bits >= bits)
                {
                    // Temos bits suficientes, vamos executar o depósito
                    sender.Tamer.GainBit(-bits); // Retirando do Tamer
                    sender.User.GainBit(bits); // Depositando na Warehouse
                    result = 5;
                }
                else
                {
                    result = 2;
                }
            }
            // Retirada
            else if (type == 2)
            {
                // Primeiro vamos ver se o Usuário possui mesmo o valor a ser retirado
                if (sender.User.WareBits >= bits)
                {
                    // Temos bits suficientes, vamos executar o depósito
                    sender.Tamer.GainBit(bits); // Depositando no Tamer
                    sender.User.GainBit(-bits); // Retirando da Warehouse
                    result = 8;
                }
                else
                {
                    result = 6;
                }
            }

            // Respondendo o client
            sender.Connection.Send(new Packets.PACKET_WAREHOUSE_BITS(type, bits
                , sender.User.WareBits, result));

            sender.Connection.Send(new Packets.PACKET_BIT_ATT(sender.Tamer));
        }
    }
}
