namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public MarionetteWebBrowser CloseFirstPage()
    {
        if (_pages.Any())
        {
            var page = _pages.First();
            page.CloseAsync().Wait();
            _pages.Remove(page);
        }

        return this;
    }
}