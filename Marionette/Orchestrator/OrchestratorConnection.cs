using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator
{
    public class OrchestratorConnection
    {   
        
        public MySqlConnection Connection;
        public OrchestratorConnection(string databaseName, string UID, string password)
        {
            string connectionString = $"Server=localhost;Database={databaseName};Uid={UID};Pwd={password};";

            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        public DataTable ReceiveData(string tableName)
        {
            string table = tableName;
            string sql = $"SELECT * FROM {table}";
        
            using var command = new MySqlCommand(sql, Connection);
            using var adapter = new MySqlDataAdapter(command);
        
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
        
            return dataTable;
        }

        public void CloseConnection()
        {
            Connection.Close();
        }
    
    }
}