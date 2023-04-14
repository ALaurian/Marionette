using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle WaitElementAppear(string selector, bool lockToLastPage = false, int delayBefore = 1)
    {
        Thread.Sleep(TimeSpan.FromSeconds(delayBefore));
        var element = FindElement(selector, lockToLastPage);
        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Element {selector} appeared.", selector);

        return element;
    }
}