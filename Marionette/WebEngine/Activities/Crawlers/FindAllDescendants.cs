using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{    

    public List<IElementHandle> FindAllDescendants(string selector)
    {
        var descendants = FindElement(selector).QuerySelectorAllAsync("*").Result.ToList();

        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}]    Found {descendants.Count} descendants of {selector}");
        descendants.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}]  Found descendant at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return descendants;
    }

    public List<IElementHandle> FindAllDescendants(IElementHandle element)
    {
        var descendants = element.QuerySelectorAllAsync("*").Result.ToList();

        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}]    Found {descendants.Count} descendants of element");
        descendants.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}]  Found descendant at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return descendants;
    }
}