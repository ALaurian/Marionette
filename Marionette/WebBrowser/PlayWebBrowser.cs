using System.Diagnostics;
using Microsoft.Playwright;
using MoreLinq;

namespace Marionette.WebBrowser;

public partial class PlayWebBrowser
{
    private IBrowser _browser;
    private IBrowserContext _context { get; set; }
    private IPage _page { get; set; }
    private bool _force = false;
    private LoadState _pageWaitType = LoadState.DOMContentLoaded;
    private List<IDownload> downloadedFiles = new();
    private List<string> fileDownloadSession = new();
    private IDialog _dialog { get; set; }

    public void ForceActions(bool state) => _force = state;

    public void SetPageWaitType(LoadState PageWaitType) => _pageWaitType = PageWaitType;

    /// <summary>
    /// Opens a session which can be connected to remotely.
    /// </summary>
    /// <param name="browserType"></param>
    /// <param name="pageWaitType"></param>
    public PlayWebBrowser(int port = 8080, bool connectSession = false,
        string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
        BrowserType browserType = BrowserType.Chrome, LoadState pageWaitType = LoadState.DOMContentLoaded)
    {
        //start Chrome process with remote debugging enabled

        if (connectSession)
        {
            var playwright = Playwright.CreateAsync().Result;

            _browser = browserType switch
            {
                BrowserType.Chrome => playwright.Chromium.ConnectOverCDPAsync("http://localhost:" + port).Result,
                BrowserType.Firefox => throw new Exception("Firefox is not supported at this moment."),
                _ => null
            };

            _context = _browser.Contexts.First();
            _page = _context.NewPageAsync().Result;
            _pageWaitType = pageWaitType;
            _page.Download += downloadHandler;
            _page.Dialog += dialogHandler;
        }
        else
        {
            var process = new Process();
            var processStartInfo = new ProcessStartInfo
            {
                FileName = browserPath,
                Arguments = "--remote-debugging-port=" + port,
                UseShellExecute = false,
            };
            process.StartInfo = processStartInfo;
            process.Start();

            var playwright = Playwright.CreateAsync().Result;

            _browser = browserType switch
            {
                BrowserType.Chrome => playwright.Chromium.ConnectOverCDPAsync("http://localhost:" + port).Result,
                BrowserType.Firefox => throw new Exception("Firefox is not supported at this moment."),
                _ => null
            };

            _context = _browser.Contexts.First();
            _page = _context.Pages.First();
            _pageWaitType = pageWaitType;
            _page.Download += downloadHandler;
            _page.Dialog += dialogHandler;
        }


        Console.WriteLine("WebBrowser connected to port" + port + ". With " + browserType + " browser.");
    }

    public PlayWebBrowser(BrowserType browserType, LoadState pageWaitType = LoadState.DOMContentLoaded,
        bool headlessMode = false)
    {
        var playwright = Playwright.CreateAsync().Result;

        _browser = browserType switch
        {
            BrowserType.Chrome => playwright.Chromium
                .LaunchAsync(new BrowserTypeLaunchOptions { Headless = headlessMode }).Result,
            BrowserType.Firefox => playwright.Firefox
                .LaunchAsync(new BrowserTypeLaunchOptions { Headless = headlessMode }).Result,
            _ => null
        };

        _context = _browser.NewContextAsync().Result;
        _page = _context.NewPageAsync().Result;
        _pageWaitType = pageWaitType;
        _page.Download += downloadHandler;
        _page.Dialog += dialogHandler;

        Console.WriteLine("WebBrowser was successfully started.");
    }

    public IPage GetPage() => _page;

    public PlayWebBrowser Navigate(string url)
    {
        _page.GotoAsync(url).Wait();

        return this;
    }

    public PlayWebBrowser Close(string containedURL)
    {
        var pages = _context.Pages.Where(x => x.Url.Contains(containedURL));
        if (pages.Any())
            pages.ForEach(x => x.CloseAsync().Wait());

        return this;
    }

    public PlayWebBrowser CloseLastPage()
    {
        var pages = _context.Pages;
        if (pages.Any())
            pages.Last().CloseAsync().Wait();

        return this;
    }

    public PlayWebBrowser CloseFirstPage()
    {
        var pages = _context.Pages;
        if (pages.Any())
            pages.First().CloseAsync().Wait();

        return this;
    }
}