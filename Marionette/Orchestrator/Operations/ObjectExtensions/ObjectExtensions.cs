using System.Reflection;
using MySqlConnector;

namespace Marionette.Orchestrator.Operations.Helpers;

public static class ObjectExtensions
{
    public static void DeleteAsset(this Asset asset)
    {
        var privateFields = asset.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(x=> x.Name == "_orchestrator");
        var connection = (privateFields.GetValue(asset) as Orchestrator).Connection;
        
        var sql = $@"
        DELETE FROM Assets
        WHERE Name = '{asset.Name}';";

        using (var cmd = new MySqlCommand(sql, connection))
        {
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteProcess(this Process process)
    {
        var privateFields = process.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(x=> x.Name == "_orchestrator");
        var connection = (privateFields.GetValue(process) as Orchestrator).Connection;
        
        var sql = $@"
        DELETE FROM Processes
        WHERE Name = '{process.Name}'
            AND Version = '{process.Version}'
            AND Description = '{process.Description}'
            AND Path = '{process.Path}';";

        using (var cmd = new MySqlCommand(sql, connection))
        {
            cmd.ExecuteNonQuery();
        }
    }
}