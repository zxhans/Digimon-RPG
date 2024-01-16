using MySql.Data.MySqlClient;

namespace Digimon_Project.Database.Results
{
    class VerificationResult : ISelectResult
    {
        public bool HasRows { get; private set; }
        public int Id { get; private set; }

        public void OnExecuted(MySqlDataReader reader)
        {
            HasRows = reader.HasRows;
            if (reader.HasRows)
            {
                reader.Read();
                Id = reader.GetInt32("id");
            }
        }
    }
}
