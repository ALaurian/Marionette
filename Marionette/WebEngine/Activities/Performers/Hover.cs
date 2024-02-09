using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle Hover(string selector, bool lockToLastPage = false, int waitTimer = 250)
    {
        var element = FindElement(selector, lockToLastPage);

        element.HoverAsync(new ElementHandleHoverOptions { Force = _force }).Wait();
        Thread.Sleep(waitTimer);
        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Hovered over element.");

        return element;
    }

    public IElementHandle Hover(IElementHandle element, int waitTimer = 250)
    {
        element.HoverAsync(new ElementHandleHoverOptions { Force = _force }).Wait();
        Thread.Sleep(waitTimer);
        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Hovered over element.");

        return element;
    }
}