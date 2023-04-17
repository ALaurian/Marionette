namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void DeleteAllCookies()
    {
        _context.ClearCookiesAsync().Wait();
    }
}