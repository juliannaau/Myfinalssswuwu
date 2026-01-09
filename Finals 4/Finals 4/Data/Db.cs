using MySql.Data.MySqlClient;

namespace Finals_4.Data
{
    public class Db
    {
        private readonly string _connectionString = "server=localhost;database=attendance;user=root;password=Finals;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
