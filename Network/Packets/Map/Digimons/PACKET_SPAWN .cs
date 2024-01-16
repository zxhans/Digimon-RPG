using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_SPAWN : OutPacket
    {
        public PACKET_SPAWN(Spawn spawn)
            : base(PacketType.PACKET_SPAWN)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 0A 9E ")); // Fill
            Write((long)spawn.Id); // Identifier
            Write(spawn.GUID); // Remaining Identifier

            Write(spawn.Name, 22); // Name
            //Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            Write(spawn.Model); // Model ID
            Write((short)spawn.estage);
            Write((short)spawn.Level); // Level
            Write((byte)spawn.rank); // Rank
            byte despawn = 1;
            if (spawn.Bloqueado) despawn = 11;
            if (spawn.Tempo > 0 && spawn.Abatido) despawn = 8;
            Write(despawn);  // 1 - Spawn / 8 - Despawn / 11 - Battle block
            Write(Utils.StringHex.Hex2Binary("08 00")); // Direction the model points
                                                        // 1 - West
                                                        // 3 - Northwest
                                                        // 4 - Northeast
                                                        // 6 - East
                                                        // 7 - Southeast
                                                        // 8 - South
                                                        // 9 - South-west
            Write((short)spawn.Pos.X); // X
            Write((short)spawn.Pos.Y); // Y
            Write((short)spawn.lastPos.X);
            Write((short)spawn.lastPos.Y);

        }

        public PACKET_SPAWN(Spawn spawn, byte despawn)
            : base(PacketType.PACKET_SPAWN)
        {
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 0A 9E ")); // Fill
            Write((long)spawn.Id); // Identifier
            Write(spawn.GUID); // Remaining Identifier

            Write(spawn.Name, 22); // Name
            //Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            Write(spawn.Model); // Model ID
            Write((short)spawn.estage);
            Write((short)spawn.Level); // Level
            Write((byte)spawn.rank); // Rank
            Write(despawn); // 1 - Spawn / 8 - Despawn / 11 - Battle block
            Write(Utils.StringHex.Hex2Binary("08 00")); //Direction the model points
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

        public PACKET_SPAWN(Spawn spawn, short x, short y)
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
            Write(Utils.StringHex.Hex2Binary("01")); // Despawn
            Write(Utils.StringHex.Hex2Binary("08 00")); // Direção para onde o modelo aponta
                                                        // 1 - Oeste
                                                        // 3 - Noroeste
                                                        // 4 - Nordeste
                                                        // 6 - Leste
                                                        // 7 - Sudeste
                                                        // 8 - Sul
                                                        // 9 - Sudoeste
            Write(x); // X
            Write(y); // Y
            Write(x);
            Write(y);

        }
    }
}
