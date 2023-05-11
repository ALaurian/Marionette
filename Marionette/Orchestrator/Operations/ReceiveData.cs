using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public DataTable ReceiveData(string tableName)
    {
        var table = tableName;
        var sql = $"SELECT * FROM {table}";

        ReturnAdapter(sql, out var adapter);
        Fill(adapter, out var dataTable);

        adapter.Dispose();
        return dataTable;
    }
}