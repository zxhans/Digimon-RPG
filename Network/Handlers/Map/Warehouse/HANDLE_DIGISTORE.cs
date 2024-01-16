using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Processo do Digistorage
    [PacketHandler(Type = PacketType.PACKET_DIGISTORE, Connection = ConnectionType.Map)]
    public class HANDLE_DIGISTORE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            // Lendo o pacote
            byte[] trash = packet.ReadBytes(6);

            // Pagina atual
            short pagina = packet.ReadShort();

            // Tipo de transação
            short type = packet.ReadByte();

            byte trash2 = packet.ReadByte();

            // Digimon depositado
            int deposito = packet.ReadInt();

            // Digimon Retirado
            int retirado = packet.ReadInt();

            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Processando
            double custo = 0;
            byte result = 0;
            // 0:error, 1: sucesso abertura, 2: falha abertura, 3: sucesso deposito, 4: falha deposito, 
            // 5: falha bits deposito, 6: retirada com sucesso, 
            // 7: falha retirada, 8: retirada falha bits, 9: troca com sucesso, 10: troca falha, 11: troca falha bits
            Tamer tamer = sender.Tamer;
            Digimon dResult = null; // Digimon enviado no resultado

            // Abrindo o storage
            if (type == 1)
            {
                if (sender.Tamer.Digistore > 0)
                    result = 1;
                else
                    result = 2;

                /**/
                if (result == 1)
                    sender.Connection.Send(new Packets.PACKET_DIGISTORE_LIST(sender.Tamer, pagina));
                /**/
            }

            // Depositando Digimon
            else if(type == 2)
            {
                // Temos espaço no Digistore? Temos bits suficientes pra transação?
                if(tamer.Digistore > tamer.Digistorage.Count)
                {
                    // Executando
                    foreach (Digimon d in tamer.Digimon)
                    {
                        if (d.Id == deposito)
                        {
                            // Temos Bits suficientes?
                            custo = GetCusto(d.Level);
                            if (tamer.Bits >= custo)
                            {
                                tamer.Digimon.Remove(d);
                                tamer.Digistorage.Add(d);
                                tamer.GainBit(-custo);
                                result = 3;
                                dResult = d;
                                Debug.Print("{0} depositado. Digistore count: {1}", d.OriName
                                    , tamer.Digistorage.Count);

                                tamer.SortDigimonSlots();
                                sender.Connection.Send(new Packets.PACKET_DIGISTORE_LIST(sender.Tamer, pagina));
                                break;
                            }
                            else
                            {
                                result = 5;
                            }
                        }
                    }
                }
                else
                {
                    result = 4;
                }
            }

            // Retirada
            else if (type == 3)
            {
                // Temos espaço na Party? Temos bits suficientes pra transação?
                if (tamer.Digimon.Count < 5)
                {
                    // Executando
                    foreach (Digimon d in tamer.Digistorage)
                    {
                        if (d.Id == retirado)
                        {
                            // Temos Bits suficientes?
                            custo = GetCusto(d.Level);
                            if (tamer.Bits >= custo)
                            {
                                tamer.Digimon.Add(d);
                                tamer.Digistorage.Remove(d);
                                d.CarregarEvolutions();
                                d.Tamer = tamer;
                                if (d.recTimer == null)
                                    d.SetRecTimer();
                                tamer.GainBit(-custo);
                                result = 6;
                                dResult = d;

                                tamer.SortDigimonSlots();
                                sender.Connection.Send(new Packets.PACKET_DIGISTORE_LIST(sender.Tamer, pagina));
                                break;
                            }
                            else
                            {
                                result = 8;
                            }
                        }
                    }
                }
                else
                {
                    result = 7;
                }
            }

            // Troca
            else if (type == 4)
            {
                // Executando
                Digimon retirar = null;
                Digimon depositar = null;
                // Procurando Digimon a retirar do Digistore
                foreach (Digimon d in tamer.Digistorage)
                {
                    if (d.Id == retirado)
                    {
                        retirar = d;
                        d.CarregarEvolutions();
                        d.Tamer = tamer;
                        if (d.recTimer == null)
                            d.SetRecTimer();
                        break;
                    }
                }
                // Procurando Digimon a depositar da Party
                foreach (Digimon d in tamer.Digimon)
                {
                    if (d.Id == deposito)
                    {
                        depositar = d;
                        break;
                    }
                }
                
                // Encontramos os dois Digimons?
                if (depositar != null && retirar != null)
                {
                    // Temos Bits suficientes?
                    custo = GetCusto(depositar.Level);
                    custo += GetCusto(retirar.Level);
                    if (tamer.Bits >= custo)
                    {
                        // Efetivando a troca
                        // Trocando os slots
                        int slot = depositar.Slot;
                        depositar.Slot = retirar.Slot;
                        retirar.Slot = slot;
                        // Trocando o campo digistore
                        depositar.Digistore = 1;
                        retirar.Digistore = 0;
                        // Trocando as listas locais
                        tamer.Digimon.Remove(depositar);
                        tamer.Digistorage.Remove(retirar);
                        tamer.Digimon.Add(retirar);
                        tamer.Digistorage.Add(depositar);
                        // Salvando alterações
                        retirar.SaveSlot();
                        depositar.SaveSlot();

                        // Custos e resultados
                        tamer.GainBit(-custo);
                        result = 9;
                        dResult = retirar;

                        tamer.SortDigimonSlots();
                        sender.Connection.Send(new Packets.PACKET_DIGISTORE_LIST(sender.Tamer, pagina));
                    }
                    else
                    {
                        result = 11;
                    }
                }
                else
                {
                    // Algo deu errado
                    result = 10;
                }
            }

            sender.Connection.Send(new Packets.PACKET_DIGISTORE(pagina, type, deposito, retirado, custo
                , sender.Tamer.Bits, result, dResult));

            // Debug
            Debug.Print("Digistore - Pagina: {0}, Type: {1}, deposito: {2}, retirado: {3}"
                , pagina, type, deposito, retirado);
        }

        private double GetCusto(int lvl)
        {
            if (lvl >= 81)
                return 70000;
            if (lvl >= 71)
                return 60000;
            if (lvl >= 61)
                return 50000;
            if (lvl >= 51)
                return 40000;
            if (lvl >= 41)
                return 30000;
            if (lvl >= 31)
                return 20000;
            if (lvl >= 21)
                return 10000;
            if (lvl >= 11)
                return 5000;

            return 1000;
        }
    }
}
