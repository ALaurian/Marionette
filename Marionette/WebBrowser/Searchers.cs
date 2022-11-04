using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Polly;
using Serilog;
using static System.Reflection.MethodBase;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{

    public PlaywrightTest ExpectEngine = new PageTest();
    private IElementHandle FindElement(string selector, bool lockToLastPage = false, int retries = 3)
    {
        var retryPolicy = Policy.HandleResult<IElementHandle>(result => result == null)
            .Retry(retries)
            .Execute(() =>
            {
                IElementHandle element = null;
                var pages = _context.Pages.Reverse().ToArray();

                if (lockToLastPage)
                    pages = new[] { pages.Last() };

                foreach (var w in pages)
                {
                    Pages[Pages.IndexOf(w)]
                        .WaitForLoadStateAsync(_pageWaitType, new PageWaitForLoadStateOptions { Timeout = 60 })
                        .Wait();
                    //============================================================
                    try
                    {
                        ExpectEngine.Expect(w.Locator(selector).First).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions() {Timeout = 25}).Wait();
                        element = w.Locator(selector).First.ElementHandleAsync(new LocatorElementHandleOptions() {Timeout = 25}).Result;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Error finding element {selector}");
                    }


                    if (element != null)
                    {
                        //w.BringToFrontAsync().Wait();
                        if (DebugMode)
                            Highlight(element, DebugMode_Duration);

                        Log.Warning($"[{GetCurrentMethod().Name}] Found element: {selector}");
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
                            ExpectEngine.Expect(frame.Locator(selector).First).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions() {Timeout = 25}).Wait();
                            element = frame.Locator(selector).First.ElementHandleAsync(new LocatorElementHandleOptions() {Timeout = 25}).Result; 
                            //.QuerySelectorAsync(selector).Result;
                        }
                        catch (Exception e)
                        {
                            Log.Error($"Error finding element {selector}");
                        }

                        if (element is not null)
                        {
                            if (DebugMode)
                                Highlight(element, DebugMode_Duration);

                            Log.Warning($"[{GetCurrentMethod().Name}] Found element: {selector}");
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

                return element;
            });

        return retryPolicy;
    }

    public List<IElementHandle> FindAllDescendants(string selector)
    {
        var descendants = FindElement(selector).QuerySelectorAllAsync("*").Result.ToList();

        Log.Information(
            $"[{GetCurrentMethod().Name}]    Found {descendants.Count} descendants of {selector}");
        descendants.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Log.Information(
                $"[{GetCurrentMethod().Name}]  Found descendant at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return descendants;
    }

    public List<IElementHandle> FindAllDescendants(IElementHandle element)
    {
        var descendants = element.QuerySelectorAllAsync("*").Result.ToList();

        Log.Information(
            $"[{GetCurrentMethod().Name}]    Found {descendants.Count} descendants of element");
        descendants.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Log.Information(
                $"[{GetCurrentMethod().Name}]  Found descendant at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return descendants;
    }

    public List<IElementHandle> FindAllChildren(string selector)
    {
        var children = FindElement(selector).QuerySelectorAllAsync("xpath=child::*").Result.ToList();

        Log.Information(
            $"[{GetCurrentMethod().Name}]    Found {children.Count} children of {selector}");
        children.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Log.Information(
                $"[{GetCurrentMethod().Name}]  Found child at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return children;
    }

    public List<IElementHandle> FindAllChildren(IElementHandle element)
    {
        var children = element.QuerySelectorAllAsync("xpath=child::*").Result.ToList();

        Log.Information(
            $"[{GetCurrentMethod().Name}]    Found {children.Count} children of element");
        children.ForEach(x =>
        {
            var boundingBox = x.BoundingBoxAsync().Result;
            Log.Information(
                $"[{GetCurrentMethod().Name}]  Found child at X: {boundingBox.X} Y: {boundingBox.Y}.");
        });
        return children;
    }

    public IElementHandle GetFocusedElement(bool lockToLastPage = true)
    {
        var element = FindElement("*:focus", lockToLastPage);
        var boundingBox = element.BoundingBoxAsync().Result;

        Log.Warning(
            $"[{GetCurrentMethod().Name}] Found focused element at X: {boundingBox.X} Y: {boundingBox.Y}.");
        return element;
    }
}