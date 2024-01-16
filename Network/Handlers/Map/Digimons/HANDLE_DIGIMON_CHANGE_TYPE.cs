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
    [PacketHandler(Type = PacketType.PACKET_DIGIMON_CHANGETYPE, Connection = ConnectionType.Map)]
    public class PACKET_DIGIMON_CHANGETYPE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {
            int trash1 = packet.ReadInt();
            short trash2 = packet.ReadShort();

            //db_id do digimon
            int dmSetId = packet.ReadInt();

            //id do digimon a ser setado.
            int dmNewType = packet.ReadInt();

            // Identificador do item - Se não for operação 1, este será o ID do item no banco
            int ID = packet.ReadInt();
            int ID2 = packet.ReadInt();
            int ID3 = packet.ReadInt();

            Utils.Comandos.Send(sender, "(digi_db)dmSetId " + dmSetId);
            Utils.Comandos.Send(sender, "dmNewType " + dmNewType);
            //Utils.Comandos.Send(sender, "n sei " + ID); //3 sempre
            Utils.Comandos.Send(sender, "(id na db)item ID2 " + ID2);
            Utils.Comandos.Send(sender, "item idx " + ID3);

            // Informação entre um item e outro
            string trash = packet.ReadString(96);

            byte b; // Lendo o restante do pacote (Apesar de conter informações, praticamente na mesma estrutura
            // inclusive, as informações são irrelevantes. Tudo o que precisamos é do ID para responder o Client)
            while (packet.Remaining > 0) b = packet.ReadByte();

            //faz a funcao
            if (dmNewType > 0)
            {
                //segunda parte
                //Utils.Comandos.Send(sender, "Digimon TFU > Em andamento...");
                if (sender.Tamer != null)
                {
                    //Utils.Comandos.Send(sender, "Digimon TFU > Tamer encontrado...");
                    if (dmSetId != 0)
                        foreach (Digimon d in sender.Tamer.Digimon)
                        {
                            //Utils.Comandos.Send(sender, "Digimon TFU > Procurando alvo...");
                            if (d.Id == dmSetId)
                            {
                                // Utils.Comandos.Send(sender, "Digimon TFU > Alvo encontrado...");
                                int resultado = 1;
                                if (d.Level < 41)
                                {
                                    Utils.Comandos.Send(sender, "Digimon TFU > Requer Digimon Lv. 41!");
                                    resultado = 0;
                                }
                                else if (ID3 == 22012)
                                {
                                    //LUCEMON
                                    d.NewDigievolutionForce(2, 783); //rooki
                                    d.NewDigievolutionForce(3, 821); //champion
                                    d.NewDigievolutionForce(4, 864); //ultimate
                                    d.NewDigievolutionForce(5, 719); //mega
                                    d.DigimonId = 783;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Lucemon obtido com sucesso!");
                                }
                                else if (ID3 == 22000)
                                {
                                    //AGUMON X
                                    d.NewDigievolutionForce(2, 772); //rooki
                                    d.NewDigievolutionForce(3, 803); //champion
                                    d.NewDigievolutionForce(4, 853); //ultimate
                                    d.NewDigievolutionForce(5, 678); //mega
                                    d.DigimonId = 772;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Agumon X obtido com sucesso!");
                                }
                                else if (ID3 == 22001)
                                {
                                    //GAOMON
                                    d.NewDigievolutionForce(2, 780); //rooki
                                    d.NewDigievolutionForce(3, 818); //champion
                                    d.NewDigievolutionForce(4, 861); //ultimate
                                    d.NewDigievolutionForce(5, 706); //mega
                                    d.DigimonId = 780;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Gaomon obtido com sucesso!");
                                }
                                else if (ID3 == 22002)
                                {
                                    //LALAMON
                                    d.NewDigievolutionForce(2, 781); //rooki
                                    d.NewDigievolutionForce(3, 819); //champion
                                    d.NewDigievolutionForce(4, 862); //ultimate
                                    d.NewDigievolutionForce(5, 707); //mega
                                    d.DigimonId = 781;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Lalamon obtido com sucesso!");
                                }
                                else if (ID3 == 22003)
                                {
                                    //FALCOMON
                                    d.NewDigievolutionForce(2, 782); //rooki
                                    d.NewDigievolutionForce(3, 820); //champion
                                    d.NewDigievolutionForce(4, 863); //ultimate
                                    d.NewDigievolutionForce(5, 710); //mega
                                    d.DigimonId = 782;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Falcomon obtido com sucesso!");
                                }
                                else if (ID3 == 926)
                                {
                                    //DORUMON
                                    d.NewDigievolutionForce(2, 760); //rooki
                                    d.NewDigievolutionForce(3, 826); //champion
                                    d.NewDigievolutionForce(4, 850); //ultimate
                                    d.NewDigievolutionForce(5, 703); //mega
                                    d.DigimonId = 760;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Dorumon obtido com sucesso!");
                                }
                                else if (ID3 == 587)
                                {
                                    //CATCH TFU
                                    int Revo = 50;
                                    int Cevo = 93;
                                    int Uevo = 120;
                                    int Mevo = 143;

                                    if (dmNewType == 64)
                                    {
                                        //AGUMON NORMAL
                                        Revo = 50;
                                        Cevo = 93;
                                        Uevo = 120;
                                        Mevo = 143;
                                    }
                                    else if (dmNewType == 74)
                                    {
                                        //GABUMON
                                        Revo = 60;
                                        Cevo = 86;
                                        Uevo = 121;
                                        Mevo = 623;
                                    }
                                    else if (dmNewType == 54)
                                    {
                                        //PATAMON
                                        Revo = 43;
                                        Cevo = 94;
                                        Uevo = 122;
                                        Mevo = 631;
                                    }
                                    else if (dmNewType == 65)
                                    {
                                        //PLOTMON
                                        Revo = 51;
                                        Cevo = 95;
                                        Uevo = 123;
                                        Mevo = 630;
                                    }
                                    else if (dmNewType == 68)
                                    {
                                        //TENTOMON
                                        Revo = 54;
                                        Cevo = 96;
                                        Uevo = 124;
                                        Mevo = 648;
                                    }
                                    else if (dmNewType == 63)
                                    {
                                        //BIYOMON
                                        Revo = 49;
                                        Cevo = 97;
                                        Uevo = 125;
                                        Mevo = 640;
                                    }

                                    //nova linha
                                    else if (dmNewType == 69)
                                    {
                                        //GOMAMON
                                        Revo = 55;
                                        Cevo = 98;
                                        Uevo = 126;
                                        Mevo = 665;
                                    }
                                    else if (dmNewType == 56)
                                    {
                                        //PALMON
                                        Revo = 8;
                                        Cevo = 9;
                                        Uevo = 10;
                                        Mevo = 11;
                                    }
                                    else if (dmNewType == 55)
                                    {
                                        //GOTSUMON (ANCIENT VOLCA LINE)
                                        Revo = 898;
                                        Cevo = 91;
                                        Uevo = 115;
                                        Mevo = 674;
                                    }
                                    else if (dmNewType == 71)
                                    {
                                        //HAGURUMON
                                        Revo = 57;
                                        Cevo = 82;
                                        Uevo = 147;
                                        Mevo = 664;
                                    }
                                    else if (dmNewType == 75)
                                    {
                                        //ELECMON
                                        Revo = 61;
                                        Cevo = 87;
                                        Uevo = 118;
                                        Mevo = 632;
                                    }
                                    else if (dmNewType == 76)
                                    {
                                        //HAWKMON
                                        Revo = 62;
                                        Cevo = 102;
                                        Uevo = 135;
                                        Mevo = 655;
                                    }
                                    d.NewDigievolutionForce(2, Revo); //rooki
                                    d.NewDigievolutionForce(3, Cevo); //champion
                                    d.NewDigievolutionForce(4, Uevo); //ultimate
                                    d.NewDigievolutionForce(5, Mevo); //mega
                                    d.DigimonId = Revo;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Realizado com sucesso!");
                                }
                                else if (ID3 == 910)
                                {
                                    //CATCH TFU 2
                                    int Revo = 63;
                                    int Cevo = 103;
                                    int Uevo = 136;
                                    int Mevo = 0;

                                    if (dmNewType == 77)
                                    {
                                        //ARMADILOMON
                                        Revo = 63;
                                        Cevo = 103;
                                        Uevo = 136;
                                        Mevo = 0;
                                    }
                                    else if (dmNewType == 78)
                                    {
                                        //LOPMON
                                        Revo = 64;
                                        Cevo = 104;
                                        Uevo = 138;
                                        Mevo = 945;
                                    }
                                    else if (dmNewType == 82)
                                    {
                                        //KOTEMON - CRUSADERMON
                                        Revo = 887;
                                        Cevo = 798;
                                        Uevo = 839;
                                        Mevo = 658;
                                    }
                                    else if (dmNewType == 83)
                                    {
                                        //CANDLEMON DYNAS
                                        Revo = 69;
                                        Cevo = 100;
                                        Uevo = 131;
                                        Mevo = 647;
                                    }
                                    else if (dmNewType == 85)
                                    {
                                        //WORMMON
                                        Revo = 70;
                                        Cevo = 796;
                                        Uevo = 620;
                                        Mevo = 654;
                                    }
                                    else if (dmNewType == 61)
                                    {
                                        //GOBLIMON
                                        Revo = 47;
                                        Cevo = 78;
                                        Uevo = 837;
                                        Mevo = 656;
                                    }

                                    //nova linha
                                    else if (dmNewType == 86)
                                    {
                                        //LABRAMON
                                        Revo = 71;
                                        Cevo = 797;
                                        Uevo = 838;
                                        Mevo = 657;
                                    }
                                    else if (dmNewType == 90)
                                    {
                                        //KERAMON
                                        Revo = 72;
                                        Cevo = 801;
                                        Uevo = 842;
                                        Mevo = 659;
                                    }
                                    else if (dmNewType == 66)
                                    {
                                        //BETAMON
                                        Revo = 52;
                                        Cevo = 88;
                                        Uevo = 119;
                                        Mevo = 628;
                                    }
                                    else if (dmNewType == 619)
                                    {
                                        //BEARMON
                                        Revo = 771;
                                        Cevo = 805;
                                        Uevo = 849;
                                        Mevo = 668;
                                    }
                                    else if (dmNewType == 59)
                                    {
                                        //MUSHROOMON
                                        Revo = 46;
                                        Cevo = 85;
                                        Uevo = 127;
                                        Mevo = 626;
                                    }
                                    d.NewDigievolutionForce(2, Revo); //rooki
                                    d.NewDigievolutionForce(3, Cevo); //champion
                                    d.NewDigievolutionForce(4, Uevo); //ultimate
                                    d.NewDigievolutionForce(5, Mevo); //mega
                                    d.DigimonId = Revo;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Realizado com sucesso!");
                                }
                                else if (ID3 == 22070)
                                {
                                    //CATCH TFU 3
                                    int Revo = 45;
                                    int Cevo = 951;
                                    int Uevo = 130;
                                    int Mevo = 625;

                                    if (dmNewType == 57)
                                    {
                                        //DEMI DEVIMON BOLTMON
                                        Revo = 777;
                                        Cevo = 812;
                                        Uevo = 854;
                                        Mevo = 684;
                                    }
                                    else if (dmNewType == 622)
                                    {
                                        //DRACOMON
                                        Revo = 773;
                                        Cevo = 808;
                                        Uevo = 845;
                                        Mevo = 679;
                                    }
                                    else if (dmNewType == 623)
                                    {
                                        //FANBEEMON
                                        Revo = 774;
                                        Cevo = 811;
                                        Uevo = 847;
                                        Mevo = 681;
                                    }
                                    else if (dmNewType == 624)
                                    {
                                        //DOKUNEMON
                                        Revo = 775;
                                        Cevo = 814;
                                        Uevo = 856;
                                        Mevo = 685;
                                    }

                                    //NOVA LINHA
                                    else if (dmNewType == 626)
                                    {
                                        //DEMI DEVI MYOTIS
                                        Revo = 45;
                                        Cevo = 951;
                                        Uevo = 130;
                                        Mevo = 625;
                                    }
                                    else if (dmNewType == 627)
                                    {
                                        //RYUDA
                                        Revo = 778;
                                        Cevo = 816;
                                        Uevo = 857;
                                        Mevo = 692;
                                    }
                                    else if (dmNewType == 628)
                                    {
                                        //TAPIR
                                        Revo = 779;
                                        Cevo = 817;
                                        Uevo = 860;
                                        Mevo = 696;
                                    }
                                    else if (dmNewType == 633)
                                    {
                                        //DRACMON
                                        Revo = 784;
                                        Cevo = 822;
                                        Uevo = 866;
                                        Mevo = 725;
                                    }

                                    d.NewDigievolutionForce(2, Revo); //rooki
                                    d.NewDigievolutionForce(3, Cevo); //champion
                                    d.NewDigievolutionForce(4, Uevo); //ultimate
                                    d.NewDigievolutionForce(5, Mevo); //mega
                                    d.DigimonId = Revo;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Realizado com sucesso!");
                                }
                                else if (ID3 == 909 || ID3 == 920)
                                {
                                    //STARTER TFU
                                    int Revo = 41;
                                    int Cevo = 74;
                                    int Uevo = 111;
                                    int Mevo = 621;

                                    if (dmNewType == 51)
                                    {
                                        //GUILMON
                                        Revo = 41;
                                        Cevo = 74;
                                        Uevo = 111;
                                        Mevo = 621;
                                    }
                                    else if (dmNewType == 52)
                                    {
                                        //RENAMON
                                        Revo = 2;
                                        Cevo = 5;
                                        Uevo = 6;
                                        Mevo = 7;
                                    }
                                    else if (dmNewType == 53)
                                    {
                                        //TERRIERMON
                                        Revo = 42;
                                        Cevo = 75;
                                        Uevo = 112;
                                        Mevo = 622;
                                    }

                                    //NOVA LINHA
                                    else if (dmNewType == 67)
                                    {
                                        //VEEMON
                                        Revo = 53;
                                        Cevo = 89;
                                        Uevo = 129;
                                        Mevo = 624;
                                    }
                                    else if (dmNewType == 62)
                                    {
                                        //IMPMON
                                        Revo = 48;
                                        Cevo = 83;
                                        Uevo = 137;
                                        Mevo = 638;
                                    }
                                    else if (dmNewType == 81)
                                    {
                                        //MONODRAMON
                                        Revo = 67;
                                        Cevo = 107;
                                        Uevo = 142;
                                        Mevo = 645;
                                    }

                                    d.NewDigievolutionForce(2, Revo); //rooki
                                    d.NewDigievolutionForce(3, Cevo); //champion
                                    d.NewDigievolutionForce(4, Uevo); //ultimate
                                    d.NewDigievolutionForce(5, Mevo); //mega
                                    d.DigimonId = Revo;
                                    Utils.Comandos.Send(sender, "Digimon TFU > Realizado com sucesso!");
                                }

                                /////////////////////////////////////////////////////////
                                ///BM ITEM
                                /////////////////////////////////////////////////////////   


                                else if (ID3 == 21004 || ID3 == 21008)
                                {
                                    //VI BM ITEM
                                    byte fase = 0;
                                    int alvo = 0;
                                    if (dmNewType == 284)
                                    {
                                        //LUCE SATAN MONDE
                                        fase = 5;
                                        alvo = 720;
                                    }
                                    else if (dmNewType == 203)
                                    {
                                        //DUKE CM
                                        fase = 5;
                                        alvo = 649;
                                    }
                                    else if (dmNewType == 193)
                                    {
                                        //BELZE BM
                                        fase = 5;
                                        alvo = 639;
                                    }
                                    else if (dmNewType == 275)
                                    {
                                        //DEXMON
                                        fase = 5;
                                        alvo = 713;
                                    }

                                    if (fase != 0)
                                    {
                                        d.NewDigievolutionForce(fase, alvo);
                                        Utils.Comandos.Send(sender, "BM Item > Sucesso!");
                                    }
                                }
                                else if (ID3 == 21005 || ID3 == 21009)
                                {
                                    //VA BM ITEM
                                    byte fase = 0;
                                    int alvo = 0;
                                    if (dmNewType == 267)
                                    {
                                        //SHINEGREYMON BM
                                        fase = 5;
                                        alvo = 705;
                                    }
                                    else if (dmNewType == 231)
                                    {
                                        //BLACK SAINT GARGOMON
                                        fase = 5;
                                        alvo = 670;
                                    }
                                    else if (dmNewType == 232)
                                    {
                                        //FUJIN
                                        fase = 5;
                                        alvo = 671;
                                    }
                                    else if (dmNewType == 198)
                                    {
                                        //IMPERIALDRAMON PALADIN MODE
                                        fase = 5;
                                        alvo = 644;
                                    }
                                    else if (dmNewType == 273)
                                    {
                                        //RAVEMON BM
                                        fase = 5;
                                        alvo = 711;
                                    }

                                    if (fase != 0)
                                    {
                                        d.NewDigievolutionForce(fase, alvo);
                                        Utils.Comandos.Send(sender, "BM Item > Sucesso!");
                                    }
                                }
                                else if (ID3 == 21006 || ID3 == 21010)
                                {
                                    //DA BM ITEM
                                    byte fase = 0;
                                    int alvo = 0;
                                    if (dmNewType == 219)
                                    {
                                        //KUZUHAMON
                                        fase = 5;
                                        alvo = 662;
                                    }
                                    else if (dmNewType == 270)
                                    {
                                        //MIRAGE GAOGAMON BM
                                        fase = 5;
                                        alvo = 708;
                                    }
                                    else if(dmNewType == 271)
                                    {
                                        //ROSEMON BM
                                        fase = 5;
                                        alvo = 709;
                                    }

                                    if (fase != 0)
                                    {
                                        d.NewDigievolutionForce(fase, alvo);
                                        Utils.Comandos.Send(sender, "BM Item > Sucesso!");
                                    }
                                }
                                else
                                {
                                    Utils.Comandos.Send(sender, "Invalid TFU/MEGA option!");
                                    resultado = 0;
                                }

                                if (resultado == 1)
                                {
                                    foreach (Item item in sender.Tamer.Items)
                                    {
                                        if (item != null && item.ItemId == ID3 && item.ItemQuant > 0)
                                        {
                                            // Descontando itens

                                            sender.Tamer.RemoveItem(item.ItemName, 1);
                                            sender.Tamer.AtualizarInventario();
                                            break;
                                        }
                                    }
                                    //sender.SendDigimons();
                                    d.CarregarEvolutions();
                                    d.Digivolver(0);
                                    sender.Connection.Send(new Packets.PACKET_DIGIMON_INDIVIDUAL(d));
                                    sender.Tamer.AtualizarInventario();

                                    break;
                                }
                            }
                        }
                }
            }
            else
            {
                //primeira parte em que seleciona o boneco a ser trocado
                Utils.Comandos.Send(sender, "Digimon TFU > Selecione o Digimon desejado!");
            }
            //RESPONDE O CLIENTE
            sender.Connection.Send(new Packets.PACKET_DIGIMON_CHANGETYPE(dmSetId, dmNewType));
        }
    }
}
