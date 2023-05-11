using System.Data;
using Marionette.Orchestrator.Enums;
using MySqlConnector;

namespace Marionette.Orchestrator
{
    public partial class Orchestrator
    {
        public MySqlConnection Connection;

        public Orchestrator(string server, string databaseName, string UID, string password)
        {
            var connectionString =
                $"Server={server};Database={databaseName};Uid={UID};Pwd={password};Allow User Variables=true;Pooling=true;Max Pool Size=100;";

            OpenConnection(connectionString);
        }
    }
}