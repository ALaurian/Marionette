using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle ScrollToElement(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.ScrollIntoViewIfNeededAsync().Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}][{selector}] Scrolled to element.");

        return element;
    }

    public IElementHandle ScrollToElement(IElementHandle element)
    {
        element.ScrollIntoViewIfNeededAsync().Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Scrolled to element.");

        return element;
    }
}