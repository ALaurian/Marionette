using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle GetFocusedElement(bool lockToLastPage = true)
    {
        var element = FindElement("*:focus", lockToLastPage);
        var boundingBox = element.BoundingBoxAsync().Result;

        Log.Warning(
            $"[{MethodBase.GetCurrentMethod().Name}] Found focused element at X: {boundingBox.X} Y: {boundingBox.Y}.");
        return element;
    }
}