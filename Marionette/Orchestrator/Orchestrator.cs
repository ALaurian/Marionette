using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator
{
    public partial class Orchestrator
    {
        public MySqlConnection Connection;

        public Orchestrator(string server, string databaseName, string UID, string password)
        {
            string connectionString =
                $"Server={server};Database={databaseName};Uid={UID};Pwd={password};Allow User Variables=true;";

            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }
    }
}