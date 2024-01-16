using MySql.Data.MySqlClient;

namespace Digimon_Project.Database
{
    public interface ISelectResult
    {
        void OnExecuted(MySqlDataReader reader);
    }
}
