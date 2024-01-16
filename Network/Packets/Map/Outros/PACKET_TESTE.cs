using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_TESTE : OutPacket
    {
        public PACKET_TESTE(): base()
        {
            // O pacote é mesmo vazio
            //Write(StringHex.Hex2Binary(PacketString.ReadOnly("pacote")));
        }

        public PACKET_TESTE(Spawn spawn, byte teste)
            : base(PacketType.PACKET_SPAWN)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 0A 9E ")); // Preenchimento
            Write((long)spawn.Id); // Identificador
            Write(spawn.GUID); // Restante do Identificador

            Write(spawn.Name, 22); // Nome
            //Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            Write(spawn.Model); // Model ID
            Write((short)spawn.estage);
            Write((short)spawn.Level); // Level
            Write((byte)spawn.rank); // Rank
            Write(teste); // Despawn?
            Write(StringHex.Hex2Binary("08 01")); // Direção para onde o modelo aponta
                                                        // 1 - Oeste
                                                        // 3 - Noroeste
                                                        // 4 - Nordeste
                                                        // 6 - Leste
                                                        // 7 - Sudeste
                                                        // 8 - Sul
                                                        // 9 - Sudoeste
            Write((short)spawn.Pos.X); // X
            Write((short)spawn.Pos.Y); // Y
            Write((short)spawn.lastPos.X);
            Write((short)spawn.lastPos.Y);

        }
    }
}
