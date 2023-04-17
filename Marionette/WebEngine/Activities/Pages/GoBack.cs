using System.Text.Json;
using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void GoBack(IPage page)
    {
        page.GoBackAsync().Wait();
    }
}