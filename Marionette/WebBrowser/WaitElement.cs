using Microsoft.Playwright;
using Polly;
using Serilog;
using static System.Reflection.MethodBase;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle WaitElementVanish(string selector, int retries, bool lockToLastPage = false)
    {
        var retry = Policy.HandleResult<IElementHandle>(x => x != null)
            .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(1))
            .Execute(() => FindElement(selector, lockToLastPage,1));

        if (retry is null)
            Log.Information($"[{GetCurrentMethod().Name}] Element {selector} vanished.", selector);
        if (retry is not null)
            Log.Information($"[{GetCurrentMethod().Name}] Element {selector} did not vanish.", selector);

        return retry;
    }

    public IElementHandle WaitElementAppear(string selector, bool lockToLastPage = false, int delayBefore = 1)
    {
        Thread.Sleep(TimeSpan.FromSeconds(delayBefore));
        var element = FindElement(selector, lockToLastPage);
        Log.Information($"[{GetCurrentMethod().Name}] Element {selector} appeared.", selector);

        return element;
    }

    public IElementHandle WaitElementVisible(string selector, bool lockToLastPage = false, int retries = 15)
    {
        return Policy.HandleResult<IElementHandle>(result => result == null)
            .Retry(retries)
            .Execute(() =>
            {
                IElementHandle element = null;
                try
                {
                    element = FindElement(selector, lockToLastPage);
                    if (element.IsVisibleAsync().Result == false)
                        return null;
                    Log.Information($"[{GetCurrentMethod().Name}] Element {selector} appeared.", selector);
                }
                catch (Exception e)
                {
                    return null;
                }

                return element;
            });
    }
}