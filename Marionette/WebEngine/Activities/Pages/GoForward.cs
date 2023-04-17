using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void GoForward(IPage page)
    {
        page.GoForwardAsync().Wait();
    }
}