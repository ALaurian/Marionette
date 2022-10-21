using Microsoft.Playwright;
using Polly;

namespace Marionette.WebBrowser;

public partial class PlayWebBrowser
{
    public IElementHandle WaitElementVanish(string selector, int retries, int timeout)
    {
        var retry = Policy.HandleResult<IElementHandle>(x => x != null)
            .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(timeout))
            .Execute(() => FindElement(selector));
        return retry;
    }

    public void Highlight(IElementHandle element, int duration = 3, int borderWidth = 4)
    {
        var origStyle = element.EvaluateAsync<object>("o => o.style;", element).Result;
        element.EvaluateAsync(
            @"o => o.style.cssText = ""border-width: " + borderWidth + @"px; border-style: solid; border-color: red"";",
            element).Wait();
        Thread.Sleep(TimeSpan.FromSeconds(duration));
        element.EvaluateAsync("(element, origStyle) => element.style.cssText = origStyle;",
            new[] { element, origStyle }).Wait();
    }

    public IElementHandle WaitElementAppear(string selector)
    {
        return FindElement(selector) ?? null;
    }

    public IElementHandle Click(string selector)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        element.ClickAsync(new ElementHandleClickOptions() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        return element;
    }

    public IElementHandle Hover(string selector)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        element.HoverAsync(new() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        return element;
    }

    public IElementHandle DoubleClick(string selector)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        element.DblClickAsync(new() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        return element;
    }

    public IElementHandle SetText(string selector, string value, bool typeInto = false)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();


        element.FillAsync("", new() { Force = _force }).Wait();
        if (typeInto)
            element.TypeAsync(value).Wait();
        else
            element.FillAsync(value, new() { Force = _force }).Wait();

        element.DisposeAsync().AsTask().Wait();


        return element;
    }

    public string GetAttribute(string selector, string attributeName)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        var attrValue = element.GetAttributeAsync(attributeName).Result;
        element.DisposeAsync().AsTask().Start();
        return attrValue;
    }

    public string GetText(string selector)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        if (element.TextContentAsync().Result != "")
            return element.TextContentAsync().Result;
        if (element.GetAttributeAsync("value").Result != "")
            return element.GetAttributeAsync("value").Result;
        if (element.InnerTextAsync().Result != "")
            return element.InnerTextAsync().Result;

        return "";
    }
}