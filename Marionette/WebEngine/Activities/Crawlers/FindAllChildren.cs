using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{

    public List<IElementHandle> FindAllChildren(string selector)
    {
        var children = FindElement(selector).QuerySelectorAllAsync("xpath=child::*").Result.ToList();

        Log.Information(
            $"[{MethodBase.GetCurrentMethod().Name}]    Found {children.Count} children of {selector}");
        children.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Log.Information(
                $"[{MethodBase.GetCurrentMethod().Name}]  Found child at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return children;
    }

    public List<IElementHandle> FindAllChildren(IElementHandle element)
    {
        var children = element.QuerySelectorAllAsync("xpath=child::*").Result.ToList();

        Log.Information(
            $"[{MethodBase.GetCurrentMethod().Name}]    Found {children.Count} children of element");
        children.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Log.Information(
                $"[{MethodBase.GetCurrentMethod().Name}]  Found child at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return children;
    }
}