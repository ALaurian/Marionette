using System.Reflection;
using Microsoft.Playwright;
using Polly;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
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
                    _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} appeared.", selector);
                }
                catch (Exception e)
                {
                    return null;
                }

                return element;
            });
    }
}