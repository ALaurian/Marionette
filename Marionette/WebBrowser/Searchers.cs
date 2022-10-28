using System.Reflection;
using Microsoft.Playwright;
using Polly;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    private IElementHandle FindElement(string selector, bool lockToLastPage = false, int retries = 120,
        double retryInterval = 0.125)
    {
        var element = Policy.HandleResult<IElementHandle>(result => result == null)
            .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
            .Execute(() =>
            {
                IElementHandle element = null;
                var pages = _context.Pages.Reverse().ToArray();

                if (lockToLastPage)
                    pages = new[] {pages.Last()};

                foreach (var w in pages)
                {
                    Pages[Pages.IndexOf(w)].WaitForLoadStateAsync(_pageWaitType, new PageWaitForLoadStateOptions() {Timeout = 60})
                        .Wait();
                    //============================================================
                    try
                    {
                        element = w.QuerySelectorAsync(selector).Result;
                    }
                    catch
                    {
                        // ignored
                    }

                    if (element != null)
                    {
                        //w.BringToFrontAsync().Wait();
                        if (DebugMode)
                            Highlight(element, DebugMode_Duration);
                        
                        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]    Found element: {selector}");
                        return element;
                    }

                    //============================================================
                    var iframes = w.Frames.ToList();
                    var index = 0;

                    for (; index < iframes.Count; index++)
                    {
                        var frame = iframes[index];

                        try
                        {
                            element = frame.QuerySelectorAsync(selector).Result;
                        }
                        catch
                        {
                            // ignored
                        }

                        if (element is not null)
                        {
                            //w.BringToFrontAsync().Wait();
                            if (DebugMode)
                                Highlight(element, DebugMode_Duration);
                            
                            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]    Found element: {selector}");
                            return element;
                        }

                        var children = frame.ChildFrames;

                        if (children.Count > 0 && iframes.Any(x => children.Any(y => y.Equals(x))) == false)
                        {
                            iframes.InsertRange(index + 1, children);
                            index--;
                        }
                    }
                }

                return null;
            });

        return element;
    }

    public List<IElementHandle> FindAllDescendants(string selector)
    {
        var descendants = FindElement(selector).QuerySelectorAllAsync("*").Result.ToList();
        
        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]    Found {descendants.Count} descendants of {selector}");
        descendants.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]  Found descendant at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return descendants;
    }
    
    public List<IElementHandle> FindAllDescendants(IElementHandle element)
    {
        var descendants = element.QuerySelectorAllAsync("*").Result.ToList();
        
        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]    Found {descendants.Count} descendants of element");
        descendants.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]  Found descendant at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return descendants;
    }

    public List<IElementHandle> FindAllChildren(string selector)
    {
        var children = FindElement(selector).QuerySelectorAllAsync("xpath=child::*").Result.ToList();
        
        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]    Found {children.Count} children of {selector}");
        children.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]  Found child at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return children;
    }
    
    public List<IElementHandle> FindAllChildren(IElementHandle element)
    {
        var children = element.QuerySelectorAllAsync("xpath=child::*").Result.ToList();
        
        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]    Found {children.Count} children of element");
        children.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]  Found child at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return children;
    }
    
    public IElementHandle GetFocusedElement(bool lockToLastPage = true)
    {
        var element = FindElement("*:focus", lockToLastPage);
        var boundingBox = element.BoundingBoxAsync().Result;
        
        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod()}]    Found focused element at X: {boundingBox.X} Y: {boundingBox.Y}.");
        return element;
    }
}