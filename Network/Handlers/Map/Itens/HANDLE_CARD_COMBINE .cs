using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Digimon_Project.Network.Handlers.Map
{
    // Pacote recebido ao finalizar o login no map. Requer uma resposta do mesmo tipo vazia, PACKET_CCF0
    [PacketHandler(Type = PacketType.PACKET_CARD_COMBINE, Connection = ConnectionType.Map)]
    public class PACKET_CARD_COMBINE : Handler<Client>
    {
        public override void Handle(Client sender, InPacket packet)
        {

            // Lixo
            int unk = packet.ReadInt();
            int unk2 = packet.ReadShort();

            /*
            RECV -> 0xAACC  [170] LENGTH: 230
            18 00 00 00 98 98 preenchimento
            07 03 00 00 -- op?
            91 01 00 00 -- bless id
            03 00 00 00 
            27 02 00 00 
            75 06 00 00 -- id do card 1
            1A 03 00 00 01 00
            00 00 01 00 00 00 50 C3 00 00 A8 61 00 00 00 00
            00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00
            00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            00 00 00 00 00 00 00 00 00 00 0F 27 50 C3 9C 08
            00 00 00 00 00 00 03 00 00 00 28 02 00 00 
            75 06 00 00 -- id do card 2
            1A 03 00 00 01 00 00 00 01 00 00 00 50 C3
            00 00 A8 61 00 00 00 00 00 00 01 00 00 00 00 00
            00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            00 00 0F 27 50 C3 9D 08 00 00 00 00 00 00    
            */

            byte[] trash1 = packet.ReadBytes(4);

            int blessItemIDX = packet.ReadInt();

            byte[] trash2 = packet.ReadBytes(8);

            int card1ItemIDX = packet.ReadInt();

            byte[] trash3 = packet.ReadBytes(100);

            int card2ItemIDX = packet.ReadInt();

            byte mapID = packet.ReadByte();

            // O pacote contem todas as informações do item. Contudo, apenas o ID já é suficiente neste processo
            byte b; // Lendo o restante do pacote
            while (packet.Remaining > 0) b = packet.ReadByte();

            /*
            Utils.Comandos.Send(sender, "Bless " + blessItemIDX);
            Utils.Comandos.Send(sender, "C1 " + card1ItemIDX);
            Utils.Comandos.Send(sender, "C2 " + card2ItemIDX);
            */

            string c1Name = "0";
            string c2Name = "0";

            foreach (Item item in sender.Tamer.Items)
            {
                if (item != null && item.ItemId == card1ItemIDX && item.ItemQuant > 0)
                {
                    c1Name = item.ItemName;
                    c2Name = item.ItemName;
                    //Utils.Comandos.Send(sender, " " + item.ItemName);
                    break;
                }
            }

            string firstCardString = Regex.Match(c1Name, @"\d+").Value;
            string secondCardString = Regex.Match(c2Name, @"\d+").Value;

            int firstCardType = Int32.Parse(firstCardString);
            int secondCardType = Int32.Parse(secondCardString);


            int[] rate = {
                0,  //0
                0,  //1
                45, //2
                25, //3
                15, //4
                5,  //5
            };
            int[] price = {
                0,  //0
                0,  //1
                1000000, //2
                500000, //3
                100000, //4
                50000, //5
            };

            byte result = 1;
            byte bless = 0;


            if (card1ItemIDX != card2ItemIDX)
            {
                result = 0;
                Utils.Comandos.Send(sender, "Use identical cards to combine!");
            }
            else
            {
                if (firstCardType == 2)
                {
                    if (blessItemIDX == 401 && sender.Tamer.ItemCount("Digi Bless") >= 1)
                    {
                        bless = 1;
                    }
                    else
                    {
                        Utils.Comandos.Send(sender, "Require x1 Digi Bless");
                        result = 0;
                    }
                }

                if (sender.Tamer.Bits < price[firstCardType])
                {
                    result = 0;
                    Utils.Comandos.Send(sender, "Require " + price[firstCardType] + " Bits");
                }

                if (result == 1)
                {
                    if (bless == 1)
                    {
                        sender.Tamer.RemoveItem("Digi Bless", 1);
                    }
                    sender.Tamer.RemoveItem(c1Name, 1);
                    sender.Tamer.RemoveItem(c2Name, 1);

                    sender.Tamer.GainBit(-price[firstCardType]);

                    Random r = new Random();
                    if (r.Next(100) < rate[firstCardType])
                    {
                        string newCardItem = c1Name.Replace(firstCardType.ToString(), (firstCardType - 1).ToString());
                        Utils.Comandos.Send(sender, "Success! Received x1 " + newCardItem);
                        sender.Tamer.AddItem(newCardItem, 1, false);
                    }
                    else
                    {
                        result = 0;
                        Utils.Comandos.Send(sender, "Failed!");
                    }

                    sender.Tamer.AtualizarInventario();
                }
            }


            /*
            eCardUpgrade_error			= 0,		//¿¡·¯	
            eCardUpgrade_Success 		= 1,	//°­È­ ¼º°ø
            eCardUpgrade_Grade_Failes	= 2,	//Ä«µå µî±ÞÀÌ ÃÖ»ó
            eCardUpgrade_Kind_Failes	= 3,	//Ä«µåÀÇ Á¾·ù°¡ ´Ù¸§
            eCardUpgrade_MoreItem		= 4,	//Àç·á ¾ÆÀÌÅÛ ºÎÁ·
            eCardUpgrade_MoreBit		= 5,	//ºñÆ®¸ðÀÚ¶÷
            eCardUpgrade_Failes  		= 6,	//°­È­ ½ÇÆÐ
             */
            sender.Connection.Send(new Packets.PACKET_CARD_COMBINE(result));
        }
    }
}
