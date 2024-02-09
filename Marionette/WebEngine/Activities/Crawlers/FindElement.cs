using System.Reflection;
using Microsoft.Playwright;
using Polly;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
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
                    _pages[_pages.IndexOf(w)]
                        .WaitForLoadStateAsync(PageWaitType, new PageWaitForLoadStateOptions { Timeout = 60 })
                        .Wait();
                    //============================================================
                    try
                    {
                        _expectEngine.Expect(w.Locator(selector).First).ToHaveCountAsync(1,
                            new LocatorAssertionsToHaveCountOptions() { Timeout = 25 }).Wait();
                        element = w.Locator(selector).First
                            .ElementHandleAsync(new LocatorElementHandleOptions() { Timeout = 25 }).Result;
                    }
                    catch (Exception e)
                    {
                        _logger.LogMessage($"Error finding element {selector}");
                    }


                    if (element != null)
                    {
                        //w.BringToFrontAsync().Wait();
                        if (DebugMode)
                            Highlight(element, DebugModeDuration);

                        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Found element: {selector}");
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
                            _expectEngine.Expect(frame.Locator(selector).First).ToHaveCountAsync(1,
                                new LocatorAssertionsToHaveCountOptions() { Timeout = 25 }).Wait();
                            element = frame.Locator(selector).First
                                .ElementHandleAsync(new LocatorElementHandleOptions() { Timeout = 25 }).Result;
                            //.QuerySelectorAsync(selector).Result;
                        }
                        catch (Exception e)
                        {
                            _logger.LogMessage($"Error finding element {selector}");
                        }

                        if (element is not null)
                        {
                            if (DebugMode)
                                Highlight(element, DebugModeDuration);

                            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Found element: {selector}");
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
}