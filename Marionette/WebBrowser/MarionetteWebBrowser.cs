using System.Diagnostics;
using Microsoft.Playwright;
using MoreLinq;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{

    private IBrowser _browser;
    private IBrowserContext _context { get; set; }
    public readonly List<IPage> Pages = new();
    private bool _force = false;
    private LoadState _pageWaitType = LoadState.DOMContentLoaded;
    private List<IDownload> downloadedFiles = new();
    private List<string> fileDownloadSession = new();
    private IDialog _dialog { get; set; }
    
    public bool DebugMode = false;
    public int DebugMode_Duration = 3;
    public bool LoggingEnabled = false;

    public void ForceActions(bool state) => _force = state;

    public void SetPageWaitType(LoadState PageWaitType) => _pageWaitType = PageWaitType;

    /// <summary>
    /// Opens a session which can be connected to remotely.
    /// </summary>
    /// <param name="browserType"></param>
    /// <param name="pageWaitType"></param>
    public MarionetteWebBrowser(bool connectSession, int port = 8080,
        string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
        BrowserType browserType = BrowserType.Chrome, LoadState pageWaitType = LoadState.DOMContentLoaded)
    {
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("MarionetteLog.txt", rollingInterval: RollingInterval.Minute)
            .CreateLogger();

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
            Pages.Add(_context.Pages.First());
            _pageWaitType = pageWaitType;
            Pages[0].Download += DownloadHandler;
            Pages[0].Dialog += DialogHandler;
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
            Pages.Add(_context.Pages.First());
            _pageWaitType = pageWaitType;
            Pages[0].Download += DownloadHandler;
            Pages[0].Dialog += DialogHandler;
        }


        Console.WriteLine("WebBrowser connected to port " + port + ". With " + browserType + " browser.");
    }

    public MarionetteWebBrowser(BrowserType browserType, LoadState pageWaitType = LoadState.DOMContentLoaded,
        bool headlessMode = false)
    {
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("MarionetteLog.txt", rollingInterval: RollingInterval.Minute)
            .CreateLogger();
        
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
        Pages.Add(_context.NewPageAsync().Result);
        _pageWaitType = pageWaitType;
        Pages[0].Download += DownloadHandler;
        Pages[0].Dialog += DialogHandler;

        Console.WriteLine("WebBrowser was successfully started.");
    }
    

    public MarionetteWebBrowser Navigate(string url, IPage page)
    {
        page.GotoAsync(url).Wait();

        return this;
    }

    public MarionetteWebBrowser Close(string containedURL)
    {
        var pages = _context.Pages.Where(x => x.Url.Contains(containedURL));
        if (pages.Any())
            pages.ForEach(x => x.CloseAsync().Wait());

        return this;
    }


}