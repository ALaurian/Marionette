using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public List<BrowserContextCookiesResult> GetCookies()
    {
        return _context.CookiesAsync().Result.ToList();
    }
}