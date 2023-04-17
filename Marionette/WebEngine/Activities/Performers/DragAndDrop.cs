using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void DragAndDrop(string fromSelector, string toSelector, IPage page)
    {
        page.BringToFrontAsync().Wait();
        page.DragAndDropAsync(fromSelector, toSelector, new PageDragAndDropOptions());
    }
}