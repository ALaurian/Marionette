using System.Reflection;
using Microsoft.Playwright;
using Polly;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle WaitElementVanish(string selector, int retries, bool lockToLastPage = false)
    {
        var retry = Policy.HandleResult<IElementHandle>(x => x != null)
            .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(1))
            .Execute(() => FindElement(selector, lockToLastPage, 1));

        if (retry is null)
            Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} vanished.", selector);
        if (retry is not null)
            Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} did not vanish.", selector);

        return retry;
    }
    

}