using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public string GetAttribute(string selector, string attributeName, bool lockToLastPage = false)
    {
        var attrValue = "";

        var element = FindElement(selector, lockToLastPage);

        attrValue = element.GetAttributeAsync(attributeName).Result;

        Log.Information(
            $"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got attribute '{attributeName}' with value '{attrValue}'.");

        return attrValue;
    }

    public string GetAttribute(IElementHandle element, string attributeName)
    {
        var attrValue = element.GetAttributeAsync(attributeName).Result;

        Log.Information(
            $"[{MethodBase.GetCurrentMethod().Name}] Got attribute '{attributeName}' with value '{attrValue}'.");

        return attrValue;
    }
}