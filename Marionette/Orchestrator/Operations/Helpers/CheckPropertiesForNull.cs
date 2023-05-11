using System.Reflection;

namespace Marionette.Orchestrator;

public partial class Process
{
    private static bool CheckPropertiesForNull(object obj)
    {
        var objType = obj.GetType();
        var properties = objType.GetProperties();

        return properties.All(property => property.GetValue(obj) != null);
    }
}