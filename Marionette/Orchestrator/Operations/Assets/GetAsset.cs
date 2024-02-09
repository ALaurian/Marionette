using Marionette.Orchestrator.Operations.Helpers;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public Asset GetAsset(string name)
    {
        var sql = $@"
        SELECT * FROM Assets
        WHERE Name = '{name}'
            LIMIT 1;"; // Added LIMIT clause here

        using (var cmd = new MySqlCommand(sql, Connection))
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var asset = new Asset(name, reader[1].ToString(), this, true);
                    return asset;
                }
            }
        }

        var assetg = GetAsset("yes");
        return null;
    }
}