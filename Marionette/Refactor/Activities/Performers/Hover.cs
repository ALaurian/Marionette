using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle Hover(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.HoverAsync(new ElementHandleHoverOptions { Force = _force }).Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Hovered over element.");

        return element;
    }

    public IElementHandle Hover(IElementHandle element)
    {
        element.HoverAsync(new ElementHandleHoverOptions { Force = _force }).Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Hovered over element.");

        return element;
    }
}