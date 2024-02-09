using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public string GetText(string selector, bool expectBlank = false, bool lockToLastPage = false)
    {
        var element = expectBlank switch
        {
            true => FindElement(selector, lockToLastPage),
            false => FindElement(selector, lockToLastPage)
        };

        if (element.TextContentAsync().Result != "")
        {
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got text '{element.TextContentAsync().Result}'.");
            return element.TextContentAsync().Result;
        }

        if (element.GetAttributeAsync("value").Result != "")
        {
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got text '{element.GetAttributeAsync("value").Result}'.");
            return element.GetAttributeAsync("value").Result;
        }

        if (element.InnerTextAsync().Result != "")
        {
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got text '{element.InnerTextAsync().Result}'.");
            return element.InnerTextAsync().Result;
        }

        return "";
    }

    public string GetText(IElementHandle element)
    {
        if (element.TextContentAsync().Result != "")
        {
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Got text '{element.TextContentAsync().Result}'.");
            return element.TextContentAsync().Result;
        }

        if (element.GetAttributeAsync("value").Result != "")
        {
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Got text '{element.GetAttributeAsync("value").Result}'.");
            return element.GetAttributeAsync("value").Result;
        }

        if (element.InnerTextAsync().Result != "")
        {
            _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Got text '{element.InnerTextAsync().Result}'.");
            return element.InnerTextAsync().Result;
        }

        return "";
    }
}