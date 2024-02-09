using System.Reflection;

namespace Marionette.Orchestrator.Operations.Helpers;

public static class DictionaryObjectExtensions
{
    public static Dictionary<string, string> ToPublicDictionary<T>(this T obj)
    {
        var dict = new Dictionary<string, string>();

        var publicProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var publicProperty in publicProperties)
        {
            var valueType = publicProperty.GetValue(obj).GetType();
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

    public static Dictionary<string, string> ToPrivateDictionary<T>(this T obj)
    {
        var dict = new Dictionary<string, string>();

        var privateFields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name.StartsWith("_prev"));
        var publicProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var privateProperty in privateFields)
        {
            if (privateProperty != null)
            {
                var value = privateProperty.GetValue(obj);
                if (value != null)
                {
                    var valueType = value.GetType();
                    
                    if (valueType.IsPrimitive || valueType.IsEnum || valueType == typeof(string))
                    {
                        dict.Add(privateProperty.Name, value.ToString());
                    }
                    else
                    {
                        dict.Add(privateProperty.Name, value.Serialize());
                    }
                }
            }
        }
        
        return dict;
    }
}