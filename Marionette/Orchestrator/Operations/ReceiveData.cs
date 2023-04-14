using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
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
}