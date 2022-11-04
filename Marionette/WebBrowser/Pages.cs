using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public IPage RefreshPage(IPage page)
    {
        page.ReloadAsync().Wait();
        return page;
    }

    public IPage RefreshPage(int index)
    {
        try
        {
            Pages[index].ReloadAsync().Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return Pages[index];
    }

    public IPage SetPageSize(IPage page, int x, int y)
    {
        page.SetViewportSizeAsync(x, y).Wait();
        return page;
    }

    public IPage SetPageSize(int index, int x, int y)
    {
        try
        {
            Pages[index].SetViewportSizeAsync(x, y).Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return Pages[index];
    }

    public IPage ActivatePage(IPage page)
    {
        page.BringToFrontAsync().Wait();
        return page;
    }

    public IPage ActivatePage(int index)
    {
        try
        {
            Pages[index].BringToFrontAsync().Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Pages[index];
    }

    public IPage NewPage()
    {
        var newPage = _context.NewPageAsync().Result;
        newPage.Download += DownloadHandler;
        newPage.Dialog += DialogHandler;
        return newPage;
    }

    public MarionetteWebBrowser ClosePage(string url)
    {
        foreach (var x in Pages)
        {
            if (x.Url != url && !x.Url.Contains(url)) continue;
            x.Download -= DownloadHandler;
            x.Dialog -= DialogHandler;
            x.CloseAsync().Wait();
            Pages.Remove(x);
        }

        return this;
    }

    public MarionetteWebBrowser ClosePage(int index)
    {
        try
        {
            Pages[index].Download -= DownloadHandler;
            Pages[index].Dialog -= DialogHandler;
            Pages[index].CloseAsync().Wait();
            Pages.RemoveAt(index);
        }
        catch (Exception e)
        {
            return null;
        }

        return this;
    }

    public MarionetteWebBrowser CloseLastPage()
    {
        var pages = _context.Pages;
        if (pages.Any())
            pages.Last().CloseAsync().Wait();

        return this;
    }

    public MarionetteWebBrowser CloseFirstPage()
    {
        var pages = _context.Pages;
        if (pages.Any())
            pages.First().CloseAsync().Wait();

        return this;
    }
}