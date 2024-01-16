using Digimon_Project.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Game.Data
{
    public class UserInformation
    {
        public readonly uint Id;
        public readonly string Username;
        public string Password { get; set; }
        public string TradePassword { get; set; }
        public int Autoridade { get; set; }
        public double WareBits { get; set; }
        public byte WareExp { get; set; }

        public UserInformation(uint id, string username, string password, string tradePassword, int autoridade
            , double warebits, byte wareexp)
        {
            Id = id;
            Username = username;
            Password = password;
            TradePassword = tradePassword;
            Autoridade = autoridade;
            WareBits = warebits;
            WareExp = wareexp;
        }

        // Função para adicionar e salvar os WareBits
        public void GainBit(double bits)
        {
            WareBits += bits;

            Emulator.Enviroment.Database.Update("users", new QueryParameters() { { "warebits", WareBits } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }

        // Função para alterar a expansão da Warehouse
        public void ItemExpand(byte expand)
        {
            WareExp = expand;

            Emulator.Enviroment.Database.Update("users", new QueryParameters() { { "wareexp", WareExp } }
            , "WHERE id=@id", new QueryParameters() { { "id", Id } });
        }
    }
}
