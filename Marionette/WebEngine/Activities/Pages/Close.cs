using MoreLinq.Extensions;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public MarionetteWebBrowser Close(string containedURL)
    {
        var pages = _context.Pages.Where(x => x.Url.Contains(containedURL));
        if (pages.Any())
        {
            pages.ForEach(x => x.CloseAsync().Wait());
        }

        return this;
    }
}