using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle DoubleClick(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.DblClickAsync(new ElementHandleDblClickOptions { Force = _force }).Wait();

        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Double clicked element.");

        return element;
    }

    public IElementHandle DoubleClick(IElementHandle element)
    {
        element.DblClickAsync(new ElementHandleDblClickOptions { Force = _force }).Wait();

        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Double clicked element.");

        return element;
    }
}