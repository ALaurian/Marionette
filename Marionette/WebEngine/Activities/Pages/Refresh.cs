using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IPage Refresh(IPage page)
    {
        page.ReloadAsync().Wait();
        return page;
    }

    public IPage Refresh(int index)
    {
        return Refresh(GetPageByIndex(index));
    }
}