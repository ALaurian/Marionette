using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IPage ActivatePage(IPage page)
    {
        page.BringToFrontAsync().Wait();
        return page;
    }

    public IPage ActivatePage(int index)
    {
        return ActivatePage(GetPageByIndex(index));
    }
}