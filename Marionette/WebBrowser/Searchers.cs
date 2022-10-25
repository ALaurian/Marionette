using Microsoft.Playwright;
using Polly;

namespace Marionette.WebBrowser;

public partial class PlayWebBrowser
{
    public IElementHandle FindElement(string selector, bool lockToLastPage = false, int retries = 120, double retryInterval = 0.125)
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
                    _page.WaitForLoadStateAsync(_pageWaitType, new PageWaitForLoadStateOptions() {Timeout = 60})
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
                        w.BringToFrontAsync().Wait();
                        if (DebugMode)
                            Highlight(element, DebugMode_Duration);
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
                            w.BringToFrontAsync().Wait();
                            if (DebugMode)
                                Highlight(element, DebugMode_Duration);
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
        return descendants;
    }

    public List<IElementHandle> FindAllChildren(string selector)
    {
        var children = FindElement(selector).QuerySelectorAllAsync("xpath=child::*").Result.ToList();
        return children;
    }
}