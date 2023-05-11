using Newtonsoft.Json;

namespace Marionette.Orchestrator.Operations.Helpers;

public static class DatabaseObjectHandler
{
    public static string Serialize<T>(this T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T Deserialize<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}