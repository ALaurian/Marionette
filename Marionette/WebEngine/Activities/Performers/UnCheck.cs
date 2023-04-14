using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle UnCheck(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        if (element.IsCheckedAsync().Result)
        {
            element.UncheckAsync().Wait();
            Log.Information($"[{MethodBase.GetCurrentMethod().Name}][{selector}] Unchecked checkbox.");
        }
        else
        {
            Log.Information($"[{MethodBase.GetCurrentMethod().Name}][{selector}] Checkbox is already unchecked.");
        }

        return element;
    }
}