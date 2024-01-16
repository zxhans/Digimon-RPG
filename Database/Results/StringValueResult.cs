using MySql.Data.MySqlClient;

namespace Digimon_Project.Database.Results
{
    class StringValueResult : ISelectResult
    {
        public bool HasRows { get; private set; }
        public string value { get; private set; }

        public void OnExecuted(MySqlDataReader reader)
        {
            HasRows = reader.HasRows;
            if (reader.HasRows)
            {
                reader.Read();
                value = reader.GetString("value");
            }
        }
    }
}
