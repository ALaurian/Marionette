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
            .WaitAndRetry(1, retryAttempt => TimeSpan.FromMilliseconds(1000))
            .Execute(() => FindElement(selector, lockToLastPage));

        if (retry == null)
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} did not appear.", selector);
        if (retry != null)
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} did appeared.", selector);
        
        return retry;
    }
}