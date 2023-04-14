using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public MarionetteWebBrowser Highlight(IElementHandle element, int duration = 3, int borderWidth = 4)
    {
        var origStyle = element.EvaluateAsync<object>("o => o.style;", element).Result;
        element.EvaluateAsync(
            @"o => o.style.cssText = ""border-width: " + borderWidth + @"px; border-style: solid; border-color: red"";",
            element).Wait();
        Thread.Sleep(TimeSpan.FromSeconds(duration));
        element.EvaluateAsync("(element, origStyle) => element.style.cssText = origStyle;",
            new[] { element, origStyle }).Wait();

        return this;
    }
}