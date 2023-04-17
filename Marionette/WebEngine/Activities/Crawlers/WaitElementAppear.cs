using System.Reflection;
using Microsoft.Playwright;
using Polly;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    
    public IElementHandle WaitElementAppear(string selector, bool lockToLastPage = false, int delayBefore = 1)
    {
        Thread.Sleep(TimeSpan.FromSeconds(delayBefore));
        var retry = Policy.HandleResult<IElementHandle>(x => x == null)
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(1))
            .Execute(() => FindElement(selector, lockToLastPage));

        if (retry is null)
            Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} appeared.", selector);
        if (retry is not null)
            Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} did not appear.", selector);

        return retry;
    }
}