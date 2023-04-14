using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle SetText(string selector, string value, bool typeInto = false, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.FillAsync("", new ElementHandleFillOptions { Force = _force }).Wait();

        if (typeInto)
            element.TypeAsync(value).Wait();
        else
            element.FillAsync(value, new ElementHandleFillOptions { Force = _force }).Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Set text to '{value}'.");

        return element;
    }

    public IElementHandle SetText(IElementHandle element, string value, bool typeInto = false)
    {
        element.FillAsync("", new ElementHandleFillOptions { Force = _force }).Wait();
        if (typeInto)
            element.TypeAsync(value).Wait();
        else
            element.FillAsync(value, new ElementHandleFillOptions { Force = _force }).Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Set text to '{value}'.");

        return element;
    }
}