using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public List<Asset> GetAllAssets()
    {
        var assets = new List<Asset>();
        var sql = "SELECT * FROM Assets;";

        using (var command = new MySqlCommand(sql, Connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var asset = new Asset(reader[0].ToString(), reader[1].ToString(), this, true);
                    assets.Add(asset);
                }
            }
        }
        return assets;
    }
}