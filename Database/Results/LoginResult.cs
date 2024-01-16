using Digimon_Project.Game.Data;
using MySql.Data.MySqlClient;

namespace Digimon_Project.Database.Results
{
    public class LoginResult : ISelectResult
    {
        public bool IsValid { get; private set; }
        public UserInformation User { get; private set; }

        private uint id;
        private string username;
        private string password;
        private string tradePassword;
        private int Autoridade;
        private double WareBits;
        private int expand;

        public void OnExecuted(MySqlDataReader reader)
        {
            IsValid = reader.HasRows;
            if (IsValid)
            {
                reader.Read(); // Read first.
                id = reader.GetUInt32("id");
                username = reader.GetString("username");
                password = reader.GetString("password");
                tradePassword = reader.GetString("trade_password");
                Autoridade = reader.GetInt16("autoridade");
                WareBits = reader.GetInt64("warebits");
                expand = reader.GetInt16("wareexp");

                User = new UserInformation(id, username, password, tradePassword, Autoridade, WareBits, (byte)expand);
            }
        }
    }
}
