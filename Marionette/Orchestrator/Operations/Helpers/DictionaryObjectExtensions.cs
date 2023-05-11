using System.Reflection;

namespace Marionette.Orchestrator.Operations.Helpers;

public static class DictionaryObjectExtensions
{
    public static Dictionary<string, object> ToPublicDictionary<T>(this T obj)
    {
        var dict = new Dictionary<string, object>();

        var publicProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var publicProperty in publicProperties)
        {
            var valueType = publicProperty.GetType();
            if (valueType.IsPrimitive || valueType.IsEnum || valueType == typeof(string))
            {
                dict.Add(publicProperty.Name, publicProperty.GetValue(obj).ToString());
            }
            else
            {
                dict.Add(publicProperty.Name, publicProperty.GetValue(obj).Serialize());
            }
        }

        return dict;
    }

    public static Dictionary<string, object> ToPrivateDictionary<T>(this T obj)
    {
        var dict = new Dictionary<string, object>();

        var publicProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var publicProperty in publicProperties)
        {
            var privateProperty = obj.GetType().GetProperty($"_prev{publicProperty.Name}",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (privateProperty != null)
            {
                var value = privateProperty.GetValue(obj);
                if (value != null)
                {
                    var valueType = value.GetType();
                    if (valueType.IsPrimitive || valueType.IsEnum || valueType == typeof(string))
                    {
                        dict.Add(publicProperty.Name, value.ToString());
                    }
                    else
                    {
                        dict.Add(publicProperty.Name, value.Serialize());
                    }
                }
            }
        }

        return dict;
    }
}