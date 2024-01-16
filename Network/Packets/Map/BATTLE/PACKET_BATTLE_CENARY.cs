using System;
using System.Collections.Generic;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Teste
    public class PACKET_BATTLE_CENARY : OutPacket
    {
        // Cenário de batalha para Tamer sozinho contra Spawn
        public PACKET_BATTLE_CENARY(Digimon[] spawn, Tamer tamer)
            : base(PacketType.PACKET_BATTLE_CENARY)
        {
            // Aura
            Item aura = tamer.Items[(int)EquipSlots.aura - 1];
            if (aura != null && !aura.CheckEffect(61))
                foreach (Digimon d in tamer.Digimon)
                    if (d != null)
                        d.BackDigivolve(false, false);



            Write(Utils.StringHex.Hex2Binary("00 00 00 00 B4 48")); // Preenchimento

            PACKET_DIGIMON_WRITER writer = new PACKET_DIGIMON_WRITER();

            // Inimigos
            for (int i = 0; i < 5; i++)
            {
                if(spawn[i] != null)
                {
                    writer.WriteDigimon(spawn[i], this);
                }
                else
                {
                    Write(new byte[520]);
                }
            }

            // Aliados
            writer.WriteDigimon(tamer.Digimon[0], this);

            // Restante do pacote
            for (int i = 0; i < 4; i++)
                Write(new byte[520]);

        }

        // Cenário de Batalha para PvP
        public PACKET_BATTLE_CENARY(Digimon[] EquipeB, Digimon[] EquipeA, List<Client> clients)
            : base(PacketType.PACKET_BATTLE_CENARY)
        {
            // Aura
            foreach (Client c in clients)
                if (c != null && c.Tamer != null)
                {
                    Tamer tamer = c.Tamer;
                    if (tamer.Pet != 0 && tamer.PetHP > 0)
                        break;
                    Item aura = tamer.Items[(int)EquipSlots.aura - 1];
                    if (aura != null && !aura.CheckEffect(61))
                        foreach (Digimon d in tamer.Digimon)
                            if (d != null)
                                d.BackDigivolve(false, false);
                }



            Write(Utils.StringHex.Hex2Binary("00 00 00 00 B4 48")); // Preenchimento

            PACKET_DIGIMON_WRITER writer = new PACKET_DIGIMON_WRITER();

            // Equipe B
            for (int i = 0; i < 5; i++)
            {
                if (EquipeB[i] != null)
                {
                    writer.WriteDigimon(EquipeB[i], this);
                }
                else
                {
                    Write(new byte[520]);
                }
            }

            // Equipe A
            for (int i = 0; i < 5; i++)
            {
                if (EquipeA[i] != null)
                {
                    writer.WriteDigimon(EquipeA[i], this);
                }
                else
                {
                    Write(new byte[520]);
                }
            }

        }
    }
}
