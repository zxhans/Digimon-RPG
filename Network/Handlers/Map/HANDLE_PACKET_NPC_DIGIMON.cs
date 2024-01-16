using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client clicou em alguma opção de algum NPC Digimon ("D" Revive, Heal, Warehouse, etc)
    [PacketHandler(Type = PacketType.PACKET_NPC_DIGIMON, Connection = ConnectionType.Map)]
    public class HANDLE_PACKET_NPC_DIGIMON : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            byte[] trash = packet.ReadBytes(6); // Preenchimento

            // Operação realizada
            int op = packet.ReadInt();

            int unk = packet.ReadInt();
            long unk2 = packet.ReadLong();

            // ID do Digimon a ser processado
            int ID = packet.ReadInt();

            // Todos os dados do Digimon vem em seguida. Contudo, apenas o ID já é suficiente para o processo.
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            // Processando solicitação
            for(int i = 0; i < 5; i++)
            {
                Digimon d = sender.Tamer.Digimon[i];
                if (d != null && d.Id == ID)
                {
                    switch (op)
                    {
                        // Heal
                        case 4:
                            if (d.Health < d.MaxHealth || d.MaxVP < d.VP || d.MaxEV < d.EV)
                            {
                                int hp = d.MaxHealth - d.Health;
                                int vp = d.MaxVP - d.VP;
                                int evp = d.MaxEV - d.EV;
                                int preco = (hp + vp + evp) * 3;
                                if (sender.Tamer.Bits >= preco)
                                {
                                    d.AddHP(hp);
                                    d.AddVP(vp);
                                    d.AddEVP(evp);
                                    sender.Tamer.GainBit(-preco);

                                    // Respondendo o client
                                    sender.Connection.Send(new Packets.PACKET_NPC_DIGIMON(op, sender.Tamer.Bits, d));
                                    sender.Connection.Send(new Packets.PACKET_DIGIMON_ATT(d));
                                }
                                else
                                    // Respondendo o client
                                    sender.Connection.Send(new Packets.PACKET_NPC_DIGIMON(op, 0, d));
                            }
                            break;
                        // Revive
                        case 5:
                            if (d.Health <= 0)
                            {
                                int preco = 2000;
                                if (sender.Tamer.Bits >= preco)
                                {
                                    d.AddHP(d.MaxHealth / 10);
                                    sender.Tamer.GainBit(-preco);

                                    // Respondendo o client
                                    sender.Connection.Send(new Packets.PACKET_NPC_DIGIMON(op, sender.Tamer.Bits, d));
                                }
                                else
                                    // Respondendo o client
                                    sender.Connection.Send(new Packets.PACKET_NPC_DIGIMON(op, 0, d));
                            }
                            break;
                        // Deletando Digimon
                        case 6:
                            if (i != 0)
                            {
                                int preco = 500;
                                if (d.Level >= 11) preco = 1000;
                                if (d.Level >= 21) preco = 2000;
                                if (d.Level >= 31) preco = 4000;
                                if (d.Level >= 41) preco = 8000;
                                if (sender.Tamer.Bits >= preco && sender.Autentico)
                                {
                                    sender.Autentico = false;

                                    d.Delete();
                                    sender.Tamer.GainBit(-preco);

                                    // Respondendo o client
                                    sender.Connection.Send(new Packets.PACKET_NPC_DIGIMON(op, sender.Tamer.Bits, d));

                                    d.Close();
                                    sender.Tamer.Digimon.Remove(d);
                                }
                                else
                                    // Respondendo o client
                                    sender.Connection.Send(new Packets.PACKET_NPC_DIGIMON(op, 0, d));
                            }
                            break;
                    }

                    return;
                }
            }
        }
    }
}
