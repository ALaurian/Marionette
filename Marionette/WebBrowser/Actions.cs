using System.Reflection;
using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IElementHandle ScrollToElement(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);
        
        element.ScrollIntoViewIfNeededAsync().Wait();

        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}][{selector}] Scrolled to element.");
        return element;
    }
    
    public IElementHandle ScrollToElement(IElementHandle element)
    {
        element.ScrollIntoViewIfNeededAsync().Wait();
        
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Scrolled to element.");
        
        return element;
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

    public IElementHandle Click(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.ClickAsync(new ElementHandleClickOptions() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}][{selector}] Clicked element.");
        
        return element;
    }
    
    public IElementHandle Click(IElementHandle element)
    {
        element.ClickAsync(new ElementHandleClickOptions() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Clicked element.");
        
        return element;
    }

    public IElementHandle Hover(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.HoverAsync(new() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Hovered over element.");
        return element;
    }
    
    public IElementHandle Hover(IElementHandle element)
    {
        element.HoverAsync(new() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Hovered over element.");
        return element;
    }

    public IElementHandle DoubleClick(string selector, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);
        
        element.DblClickAsync(new() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Double clicked element.");

        return element;
    }
    
    public IElementHandle DoubleClick(IElementHandle element)
    {
        element.DblClickAsync(new() { Force = _force }).Wait();
        element.DisposeAsync().AsTask().Wait();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Double clicked element.");
        return element;
    }

    public IElementHandle SetText(string selector, string value, bool typeInto = false, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        element.FillAsync("", new() { Force = _force }).Wait();
        if (typeInto)
            element.TypeAsync(value).Wait();
        else
            element.FillAsync(value, new() { Force = _force }).Wait();

        element.DisposeAsync().AsTask().Wait();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Set text to '{value}'.");

        return element;
    }

    public IElementHandle SetText(IElementHandle element, string value, bool typeInto = false)
    {
        element.FillAsync("", new() { Force = _force }).Wait();
        if (typeInto)
            element.TypeAsync(value).Wait();
        else
            element.FillAsync(value, new() { Force = _force }).Wait();

        element.DisposeAsync().AsTask().Wait();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Set text to '{value}'.");

        return element;
    }
    

    public string GetAttribute(string selector, string attributeName, bool lockToLastPage = false)
    {
        var element = FindElement(selector, lockToLastPage);

        var attrValue = element.GetAttributeAsync(attributeName).Result;
        element.DisposeAsync().AsTask().Start();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got attribute '{attributeName}' with value '{attrValue}'.");
        return attrValue;
    }
    
    public string GetAttribute(IElementHandle element, string attributeName)
    {
        var attrValue = element.GetAttributeAsync(attributeName).Result;
        element.DisposeAsync().AsTask().Start();
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Got attribute '{attributeName}' with value '{attrValue}'.");
        return attrValue;
    }

    public string GetText(string selector, bool expectBlank = false, bool lockToLastPage = false)
    {
        IElementHandle element = expectBlank switch
        {
            true => FindElement(selector, retries: 60,lockToLastPage:lockToLastPage),
            false => FindElement(selector, lockToLastPage:lockToLastPage)
        };
        
        if (element.TextContentAsync().Result != "")
        {
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got text '{element.TextContentAsync().Result}'.");
            return element.TextContentAsync().Result;
        }

        if (element.GetAttributeAsync("value").Result != "")
        {
            
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got text '{element.GetAttributeAsync("value").Result}'.");
            return element.GetAttributeAsync("value").Result;
        }

        if (element.InnerTextAsync().Result != "")
        {
            
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] [{selector}] Got text '{element.InnerTextAsync().Result}'.");
            return element.InnerTextAsync().Result;
        }

        
        return "";
    }
    
    public string GetText(IElementHandle element)
    {
        if (element.TextContentAsync().Result != "")
        {
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] Got text '{element.TextContentAsync().Result}'.");
            return element.TextContentAsync().Result;
        }

        if (element.GetAttributeAsync("value").Result != "")
        {
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] Got text '{element.GetAttributeAsync("value").Result}'.");
            return element.GetAttributeAsync("value").Result;
        }

        if (element.InnerTextAsync().Result != "")
        {
            Serilog.Log.Information( $"[{MethodBase.GetCurrentMethod().Name}] Got text '{element.InnerTextAsync().Result}'.");
            return element.InnerTextAsync().Result;
        }

        return "";
    }
    
    public void SendKey(Key key, IPage page, bool activatePage = false)
    {
        if(activatePage)
            page.BringToFrontAsync().Wait();
        
        page.Keyboard.PressAsync(key.ToString()).Wait();
        
        Serilog.Log.Information($"[{MethodBase.GetCurrentMethod().Name}] Pressed key '{key}'.");
    }
}