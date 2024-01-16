using Digimon_Project.Database.Results;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Network.Handlers.Map
{
    // Client solicita atualização de informação de um de seus digimons
    [PacketHandler(Type = PacketType.PACKET_DIGIMON_EVO_RETURN, Connection = ConnectionType.Map)]
    public class PACKET_DIGIMON_EVO_RETURN : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int trash1 = packet.ReadInt();
            short trash2 = packet.ReadShort();

            int unk1 = packet.ReadInt();
            int digimonID = packet.ReadInt();
            int itemIDX = packet.ReadInt();

            byte b; // Lendo o restante do pacote (Apesar de conter informações, praticamente na mesma estrutura
            // inclusive, as informações são irrelevantes. Tudo o que precisamos é do ID para responder o Client)
            while (packet.Remaining > 0) b = packet.ReadByte();

            int Id = digimonID;

            if (sender.Tamer != null)
            {
                if (Id != 0)
                    foreach (Digimon d in sender.Tamer.Digimon)
                    {
                        if (d.Id == Id)
                        {
                            //SE EVO RETURN BRANCO / NORMAL
                            if (itemIDX == 898 || itemIDX == 903)
                            {
                                foreach(Item item in sender.Tamer.Items)
                                {
                                    if (item != null && item.ItemId == itemIDX && item.ItemQuant > 0)
                                    {
                                        DigimonBaseResult r = Emulator.Enviroment.Database.Select<DigimonBaseResult>(
                                        "dg.skill1, dg.skill2, dg.str, dg.dex, dg.con, dg.inte, dg.estage, dg.tipo, dg.model"
                                        + ", dg.evolution_line, dg.nome AS OriName"
                                        , "digimon AS dg"
                                        , "WHERE dg.id=@id"
                                        , new Database.QueryParameters() { { "id", d.DigimonId } });

                                        DigimonELResult eLResult = Emulator.Enviroment.Database.Select<DigimonELResult>(
                                            "i, r, c, u, m", "evolution_line", "WHERE id=@id",
                                            new Database.QueryParameters() { { "id", r.EL } });

                                        //0 - falha
                                        //1 - sucesso
                                        //2 - nao tme digimon
                                        byte result = 0;

                                        if (eLResult.Valid)
                                        {
                                            int RookieForm = eLResult.fases[1];
                                            int ChampForm = eLResult.fases[2];
                                            int UltimForm = eLResult.fases[3];
                                            int MegaForm = eLResult.fases[4];

                                            int changed = 0;

                                            if (RookieForm != d.RookieForm)
                                            {
                                                d.NewDigievolutionForce(2, RookieForm);
                                                changed = 1;
                                            }
                                            if (ChampForm != d.ChampForm)
                                            {
                                                d.NewDigievolution(3, ChampForm);
                                                changed = 1;
                                            }
                                            if (UltimForm != d.UltimForm)
                                            {
                                                d.NewDigievolution(4, UltimForm);
                                                changed = 1;
                                            }
                                            if (MegaForm != d.MegaForm)
                                            {
                                                d.NewDigievolution(5, MegaForm);
                                                changed = 1;
                                            }

                                            if (changed != 0)
                                            {
                                                result = 1;
                                            }
                                        }
                                        else
                                        {
                                            result = 0;
                                        }

                                        if (result == 0)
                                        {
                                            Utils.Comandos.Send(sender, "Digimon Return sem efeito");
                                        }
                                        else if (result == 1)
                                        {
                                            //SUCESSO!
                                            d.ResetStatus();

                                            // Descontando itens
                                            sender.Tamer.RemoveItem(item.ItemName, 1);
                                            sender.Tamer.AtualizarInventario();
                                            Utils.Comandos.Send(sender, "Digimon Return com sucesso!");

                                            // Respondendo o Client
                                            d.CarregarEvolutions();
                                            d.Digivolver(0);

                                            sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(d));
                                        }

                                        sender.Connection.Send(new Packets.PACKET_DIGIMON_EVO_RETURN(result, itemIDX, Id));
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Utils.Comandos.Send(sender, "Digimon Return em andamento");
                                sender.Connection.Send(new Packets.PACKET_DIGIMON_EVO_RETURN(0, itemIDX, Id));
                            }
                            /*
                            Utils.Comandos.Send(sender, "Clicou no :" + d.Name + "Lv. " + d.Level);
                            Utils.Comandos.Send(sender, "DigimonID " + d.DigimonId);
                            Utils.Comandos.Send(sender, "RookieForm " + d.RookieForm);
                            Utils.Comandos.Send(sender, "ChampionForm " + d.ChampForm);
                            Utils.Comandos.Send(sender, "UltForm " + d.UltimForm);
                            Utils.Comandos.Send(sender, "MegaForm " + d.MegaForm);
                            */
                            return;
                        }
                    }
            }


        }
    }
}
