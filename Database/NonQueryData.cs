using MySql.Data.MySqlClient;

namespace Digimon_Project.Database
{
    public struct NonQueryData
    {
        public QueryCallback Callback;
        public MySqlCommand Command;

        public NonQueryData(QueryCallback callback, MySqlCommand cmd)
        {
            Callback = callback;
            Command = cmd;
        }
    }
}
