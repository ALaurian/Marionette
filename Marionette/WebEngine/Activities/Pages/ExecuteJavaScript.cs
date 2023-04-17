using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public string ExecuteJavaScript(IPage page, string script)
    {
        // Evaluate the JavaScript
        var result = page.EvaluateAsync<string>(script).Result;
        return result;
    }
}