using System.Reflection;
using Microsoft.Playwright;
using Polly;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle WaitElementVanish(string selector, int retries, int timeout, bool lockToLastPage = false)
    {
        var retry = Policy.HandleResult<IElementHandle>(x => x != null)
            .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(timeout))
            .Execute(() => FindElement(selector, lockToLastPage));
        
        if(retry is null)
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] Element {selector} vanished.", selector);
        if(retry is not null)
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] Element {selector} did not vanish.", selector);

        return retry;
    }

    public IElementHandle WaitElementAppear(string selector, bool lockToLastPage = false, int retries = 120, double retryInterval = 0.125)
    {
        var element = FindElement(selector, lockToLastPage, retries, retryInterval);
        
        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] Element {selector} appeared.", selector);

        return element;
    }

    public IElementHandle WaitElementVisible(string selector, bool lockToLastPage = false, int retries = 120,
        double retryInterval = 0.125)
    {
        var element = FindElement(selector, lockToLastPage, retries, retryInterval);
        while (element.IsVisibleAsync().Result == false)
        {
            element = FindElement(selector, lockToLastPage, retries, retryInterval);
        }
        
        Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] Element {selector} is visible.", selector);
        return element.IsVisibleAsync().Result == true ? element : null;
    }
}