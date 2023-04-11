using System.Reflection;
using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void SendKey(Key key, IPage page, bool activatePage = false)
    {
        if (activatePage)
            page.BringToFrontAsync().Wait();

        page.Keyboard.PressAsync(key.ToString()).Wait();

        Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Pressed key '{key}'.");
    }
}