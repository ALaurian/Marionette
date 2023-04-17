using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void AddCookies(IEnumerable<Cookie> cookies)
    {
        _context.AddCookiesAsync(cookies).Wait();
    }
}