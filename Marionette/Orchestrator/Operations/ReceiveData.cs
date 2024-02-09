using System.Data;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public DataTable ReceiveData(string tableName)
    {
        var table = tableName;
        var sql = $"SELECT * FROM {table}";

        var adapter = ReturnAdapter(sql);
        var dataTable = Fill(adapter);

        adapter.Dispose();
        return dataTable;
    }
}