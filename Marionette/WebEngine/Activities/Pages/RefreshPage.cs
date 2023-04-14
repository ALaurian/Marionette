using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IPage RefreshPage(IPage page)
    {
        page.ReloadAsync().Wait();
        return page;
    }

    public IPage RefreshPage(int index)
    {
        return RefreshPage(GetPageByIndex(index));
    }
}