namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public MarionetteWebBrowser ClosePage(int index)
    {
        var page = GetPageByIndex(index);
        page.Download -= DownloadHandler;
        page.Dialog -= DialogHandler;
        page.CloseAsync().Wait();
        _pages.RemoveAt(index);
        return this;
    }

    public MarionetteWebBrowser ClosePage(string url)
    {
        foreach (var page in _pages.Where(p => p.Url.Contains(url)).ToList())
        {
            page.Download -= DownloadHandler;
            page.Dialog -= DialogHandler;
            page.CloseAsync().Wait();
            _pages.Remove(page);
        }

        return this;
    }
}