using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Flanium;
using FlaUI.UIA3;
using Microsoft.Playwright;
using MoreLinq;
using Polly;

namespace Fight;

public class WebBrowser
{
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;
    private bool _force;
    private List<IDownload> downloadedFiles = new();
    private List<string> fileDownloadSession = new();

    public void EagerMode()
    {
        _force = true;
    }

    public enum BrowserType
    {
        Chrome,
        Firefox,
    }

    public IPage GetPage()
    {
        return _page;
    }

    public WebBrowser(BrowserType browserType = BrowserType.Chrome, bool headlessMode = false)
    {
        var playwright = Playwright.CreateAsync().Result;

        _browser = browserType switch
        {
            BrowserType.Chrome => playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {Headless = headlessMode}).Result,
            BrowserType.Firefox => playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions {Headless = headlessMode}).Result,
            _ => null
        };

        _context = _browser.NewContextAsync().Result;
        _page = _context.NewPageAsync().Result;
        _page.Download += downloadHandler;

        Console.WriteLine("WebBrowser was successfully started.");
    }

    private async void downloadHandler(object sender, IDownload download)
    {
        fileDownloadSession.Add(await download.PathAsync());
        var waiter = download.PathAsync();
        downloadedFiles.Add(download);
        fileDownloadSession.Remove(fileDownloadSession.First());
    }

    public List<IDownload> GetDownloadedFiles()
    {
        Thread.Sleep(3000);
        while (fileDownloadSession.Any())
        {
        }

        var downloadedFilesList = downloadedFiles;
        downloadedFiles = new List<IDownload>();
        return downloadedFilesList;
    }


    public void Navigate(string url)
    {
        _page.GotoAsync(url).Wait();
    }

    public void Close(string containedURL)
    {
        var pages = _context.Pages.Where(x => x.Url.Contains(containedURL));
        if (pages.Any())
            pages.ForEach(async x => await x.CloseAsync());
    }


    #region Actions

    public async Task<IElementHandle> Click(string selector, int retries = 15, int retryInterval = 1)
    {
        var element = await Policy.HandleResult<IElementHandle>(result => result == null)
            .WaitAndRetryAsync(retries, interval => TimeSpan.FromSeconds(retryInterval))
            .ExecuteAsync(async () =>
            {
                var element = await FindElement(selector);
                if (element != null)
                {
                    try
                    {
                        await element.ClickAsync(new ElementHandleClickOptions() {Force = _force});
                        await element.DisposeAsync();
                        return element;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }

                return element;
            });

        return element;
    }

    public async Task<string> GetText(IElementHandle elementHandle, int retries = 15, int retryInterval = 1)
    {
        var element = await Policy.HandleResult<string>(result => result == null)
            .WaitAndRetryAsync(retries, interval => TimeSpan.FromSeconds(retryInterval))
            .ExecuteAsync(async () =>
            {
                var text = false;
                var innerText = false;
                
                if (await elementHandle.TextContentAsync() != "")
                    text = true;

                if (await elementHandle.InnerTextAsync() != "")
                    innerText = true;

                if (text)
                    return await elementHandle.TextContentAsync();

                if (innerText)
                    return await elementHandle.InnerTextAsync();

                return "";
            });

        return element;
    }

    #endregion


    #region Searchers

    public async Task<IElementHandle> FindElement(string selector)
    {
        IElementHandle element = null;

        var Pages = _context.Pages.ToArray();

        foreach (var w in Pages)
        {
            //============================================================
            element = await w.QuerySelectorAsync(selector);
            if (element != null)
            {
                return element;
            }

            //============================================================
            var iframes = w.Frames.ToList();
            var index = 0;

            for (; index < iframes.Count; index++)
            {
                var frame = iframes[index];


                element = await frame.QuerySelectorAsync(selector);
                if (element is not null)
                {
                    return element;
                }

                var children = frame.ChildFrames;

                if (children.Count > 0 && iframes.Any(x => children.Any(y => y.Equals(x))) == false)
                {
                    iframes.InsertRange(index + 1, children);
                    index--;
                }
            }
        }

        return element;
    }

    public async Task<List<IElementHandle>> FindAllDescendants(IElementHandle element)
    {
        var descendants = (await element.QuerySelectorAllAsync("*")).ToList();
        return descendants;
    }

    public async Task<List<IElementHandle>> FindAllChildren(IElementHandle element)
    {
        var children = (await element.QuerySelectorAllAsync("xpath=child::*")).ToList();
        return children;
    }

    public async Task<IElementHandle> WaitElementAppear(string selector, int retries = 15,
        int retryInterval = 1, string FrameType = "iframe")
    {
        var element = await Policy.HandleResult<IElementHandle>(result => result == null)
            .WaitAndRetryAsync(retries, interval => TimeSpan.FromSeconds(retryInterval))
            .ExecuteAsync(async () =>
            {
                var element = await FindElement(selector);
                if (element != null)
                {
                    return element;
                }

                return null;
            });

        if (element != null) return element;

        return null;
    }

    #endregion
}