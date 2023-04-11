namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public MarionetteWebBrowser CloseLastPage()
    {
        if (_pages.Any())
        {
            var page = _pages.Last();
            page.CloseAsync().Wait();
            _pages.Remove(page);
        }

        return this;
    }
}