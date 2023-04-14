using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IPage SetPageSize(IPage page, int x, int y)
    {
        page.SetViewportSizeAsync(x, y).Wait();
        return page;
    }

    public IPage SetPageSize(int index, int x, int y)
    {
        return SetPageSize(GetPageByIndex(index), x, y);
    }
}