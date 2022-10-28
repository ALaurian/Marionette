using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void SetPageSize(IPage page, int x, int y)
    {
        page.SetViewportSizeAsync(x, y).Wait();
    }

    public void SetPageSize(int index, int x, int y)
    {
        Pages[index].SetViewportSizeAsync(x, y).Wait();
    }

    public bool ActivatePage(IPage page)
    {
        try
        {
            page.BringToFrontAsync().Wait();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public bool ActivatePage(int index)
    {
        try
        {
            Pages[index].BringToFrontAsync().Wait();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public IPage NewPage()
    {
        var newPage = _context.NewPageAsync().Result;
        newPage.Download += DownloadHandler;
        newPage.Dialog += DialogHandler;
        return newPage;
    }

    public bool ClosePage(string url)
    {
        foreach (var x in Pages)
        {
            if (x.Url == url || x.Url.Contains(url))
            {
                x.Download -= DownloadHandler;
                x.Dialog -= DialogHandler;
                x.CloseAsync().Wait();
                Pages.Remove(x);
                return true;
            }
        }

        return false;
    }

    public bool ClosePage(int index)
    {
        try
        {
            Pages[index].Download -= DownloadHandler;
            Pages[index].Dialog -= DialogHandler;
            Pages[index].CloseAsync().Wait();
            Pages.RemoveAt(index);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}