using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IPage NewPage()
    {
        var newPage = _browser.NewPageAsync().Result;
        newPage.Download += DownloadHandler;
        newPage.Dialog += DialogHandler;
        _pages.Add(newPage);
        return newPage;
    }
}