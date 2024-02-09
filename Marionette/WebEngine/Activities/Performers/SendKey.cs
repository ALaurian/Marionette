using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void SendKey(MarionetteKey key, IPage page, bool activatePage = false)
    {
        if (activatePage)
            page.BringToFrontAsync().Wait();

        page.Keyboard.PressAsync(key.ToString()).Wait();

        _logger.LogMessage($"[{MethodBase.GetCurrentMethod().Name}] Pressed key '{key}'.");
    }

    public void SendKeys(MarionetteKey[] keys, IPage page, bool activatePage = false)
    {
        if (activatePage)
            page.BringToFrontAsync().Wait();
        
        foreach (var key in keys)
        {
            page.Keyboard.PressAsync(key.ToString()).Wait();
        }
        
    }
}