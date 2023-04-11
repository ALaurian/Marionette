using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IPage GetPageByIndex(int index)
    {
        if (index < 0 || index >= _pages.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _pages[index];
    }
}