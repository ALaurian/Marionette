using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public MarionetteWebBrowser Navigate(string url, IPage page)
    {
        page.GotoAsync(url).Wait();

        return this;
    }
}