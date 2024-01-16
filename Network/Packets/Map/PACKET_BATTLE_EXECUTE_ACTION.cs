using System;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Pacote enviado eventualmente pelo login do Map. 
    public class PACKET_BATTLE_EXECUTE_ACTION : OutPacket
    {
        public PACKET_BATTLE_EXECUTE_ACTION(Digimon[] atk, Digimon[] def, int team, int skill, int slash)
            : base(PacketType.PACKET_BATTLE_EXECUTE_ACTION)
        {
            Write(new byte[6]); // Preenchimento

            // Time que está executando a ação
            for(int i = 0; i < 5; i++)
            {
                if (atk[i] != null)
                {
                    Write(atk[i].BattleId); // ID em batalha
                    Write(atk[i].BattleSufix, 8); // Sufixo do ID em batalha
                    Write(atk[i].Health);
                    Write(atk[i].VP);
                    Write(atk[i].EV);
                    Write(new byte[8]);
                    Write(atk[i].atacando);
                    Write(atk[i].atacado);
                    Write(new byte[2]);

                    // Cards usados
                    if (atk[i].card1 != null)
                        Write(atk[i].card1.ItemId);
                    else Write(new byte [4]);
                    if (atk[i].card2 != null)
                        Write(atk[i].card2.ItemId);
                    else Write(new byte [4]);
                    if (atk[i].card3 != null)
                        Write(atk[i].card3.ItemId);
                    else Write(new byte [4]);
                    // Quantidades dos cards usados
                    if (atk[i].card1 != null)
                        Write(atk[i].card1.ItemQuant);
                    else Write(new byte[4]);
                    if (atk[i].card2 != null)
                        Write(atk[i].card2.ItemQuant);
                    else Write(new byte[4]);
                    if (atk[i].card3 != null)
                        Write(atk[i].card3.ItemQuant);
                    else Write(new byte[4]);

                    Write(new byte[48]);
                }
                else
                {
                    Write(new byte[112]);
                }
            }

            // Time que oposto
            for (int i = 0; i < 5; i++)
            {
                if (def[i] != null)
                {
                    Write(def[i].BattleId); // ID em batalha
                    Write(def[i].BattleSufix, 8); // Sufixo do ID em batalha
                    Write(def[i].Health);
                    Write(def[i].VP);
                    Write(def[i].EV);
                    Write(Utils.StringHex.Hex2Binary("F4 01 00 00"));
                    Write(new byte[4]);
                    Write(def[i].atacando);
                    Write(def[i].atacado);
                    Write(new byte[2]);

                    // Cards usados
                    if (def[i].card1 != null)
                        Write(def[i].card1.ItemId);
                    else Write(new byte[4]);
                    if (def[i].card2 != null)
                        Write(def[i].card2.ItemId);
                    else Write(new byte[4]);
                    if (def[i].card3 != null)
                        Write(def[i].card3.ItemId);
                    else Write(new byte[4]);
                    // Quantidades dos cards usados
                    if (def[i].card1 != null)
                        Write(def[i].card1.ItemQuant);
                    else Write(new byte[4]);
                    if (def[i].card2 != null)
                        Write(def[i].card2.ItemQuant);
                    else Write(new byte[4]);
                    if (def[i].card3 != null)
                        Write(def[i].card3.ItemQuant);
                    else Write(new byte[4]);

                    Write(new byte[48]);
                }
                else
                {
                    Write(new byte[112]);
                }
            }

            // Última linha
            Write(team); // 01 - Açao inimiga
                         // 02 - Açao Aliada
            Write(skill); // Skill ID
            Write(slash); // Card Slash? 0 - Não, 1 - Sim
        }

        // Exemplo de estrutura
        public PACKET_BATTLE_EXECUTE_ACTION(long id)
            : base(PacketType.PACKET_BATTLE_EXECUTE_ACTION)
        {
            // Cada Digimon possui 7 Linhas de informaçao
            // O 5 Primeiros sao do time que está executando a açao
            Write(new byte[6]);

            Write(id);
            Write(new byte[8]);
            Write(Utils.StringHex.Hex2Binary("64 00 00 00")); // HP
            Write(Utils.StringHex.Hex2Binary("64 00 00 00")); // VP
            Write(Utils.StringHex.Hex2Binary("64 00 00 00")); // EVP
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("01")); // Boolean - Atacou?
            Write(Utils.StringHex.Hex2Binary("00")); // Boolean - Foi Atacado?
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00"));
            Write(new byte[512]);
            Write((long)1);
            Write(new byte[8]);
            Write(Utils.StringHex.Hex2Binary("32 00 00 00 32 00 00 00 00 00 00 00 54 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00"));
            Write(Utils.StringHex.Hex2Binary("01")); // Boolean - Foi atacabo?
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00"));
            Write(new byte[64]);
            Write((long)2);
            Write(new byte[8]);
            Write(Utils.StringHex.Hex2Binary("32 00 00 00 32 00 00 00 00 00 00 00 54 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00")); // Boolean - Foi atacabo?
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00"));
            Write(new byte[400]);

            // Última linha
            Write(Utils.StringHex.Hex2Binary("02 00 00 00")); // 01 - Açao inimiga
                                                              // 02 - Açao Aliada
            Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Skill ID
            Write(Utils.StringHex.Hex2Binary("00 00 00 00")); // Card Slash? 0 - Não, 1 - Sim
        }
    }
}
