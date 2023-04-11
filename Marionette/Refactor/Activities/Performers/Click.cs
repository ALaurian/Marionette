using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public bool Click(ElementHandleBoundingBoxResult elementBoundingBox, IPage page)
    {
        page.Mouse.ClickAsync(elementBoundingBox.X, elementBoundingBox.Y).Wait();
        
        Log.Information($"[{MethodBase.GetCurrentMethod().Name}][{elementBoundingBox.X + "." + elementBoundingBox.Y}] Clicked element.");
        return true;
    }

    public IElementHandle Click(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.ClickAsync(new ElementHandleClickOptions { Force = _force }).Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}][{selector}] Clicked element.");

        return element;
    }

    public IElementHandle Click(IElementHandle element)
    {
        element.ClickAsync(new ElementHandleClickOptions { Force = _force }).Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Clicked element.");

        return element;
    }
}