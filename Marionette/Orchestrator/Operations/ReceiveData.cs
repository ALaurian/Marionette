using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public DataTable ReceiveData(string tableName)
    {
        string table = tableName;
        string sql = $"SELECT * FROM {table}";

        ReturnAdapter(sql, out var adapter);
        Fill(adapter, out var dataTable);

        return dataTable;
    }
}