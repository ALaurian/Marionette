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
    private IBrowserContext _context { get; set; }
    private IPage _page { get; set; }
    private bool _force = false;
    private LoadState _pageWaitType = LoadState.DOMContentLoaded;
    private List<IDownload> downloadedFiles = new();
    private List<string> fileDownloadSession = new();
    private IDialog _dialog { get; set; }

    public void ForceActions(bool state) => _force = state;

    public void SetPageWaitType(LoadState PageWaitType) => _pageWaitType = PageWaitType;

    public WebBrowser(BrowserType browserType = BrowserType.Chrome, LoadState pageWaitType = LoadState.DOMContentLoaded, bool headlessMode = false)
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
        _pageWaitType = pageWaitType;
        _page.Download += downloadHandler;
        _page.Dialog += dialogHandler;

        Console.WriteLine("WebBrowser was successfully started.");
    }

    public enum BrowserType
    {
        Chrome,
        Firefox,
    }

    public IPage GetPage() => _page;


    #region Handlers
    private async void downloadHandler(object sender, IDownload download)
    {
        fileDownloadSession.Add(await download.PathAsync());
        var waiter = download.PathAsync();
        downloadedFiles.Add(download);
        fileDownloadSession.Remove(fileDownloadSession.First());
    }

    private void dialogHandler(object sender, IDialog dialog)
    {
        _dialog = dialog;
    }

    #endregion


    public List<IDownload> GetDownloadedFiles(string pathToSaveTo)
    {
        while (!fileDownloadSession.Any())
        {
        }

        while (fileDownloadSession.Any())
        {
        }

        var downloadedFilesList = downloadedFiles;
        downloadedFiles = new List<IDownload>();
        downloadedFiles.ForEach(x => x.SaveAsAsync(pathToSaveTo).Wait());
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


    #region Dialogs

    public IDialog GetDialog() => _dialog;

    public void AcceptDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            _dialog.AcceptAsync().Wait();
            _dialog = null;
        }
    }

    public void AcceptDialog()
    {
        if (_dialog != null)
        {
            _dialog.AcceptAsync().Wait();
            _dialog = null;
        }
    }

    public void DismissDialog()
    {
        if (_dialog != null)
        {
            _dialog.DismissAsync().Wait();
            _dialog = null;
        }
    }

    public void DismissDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            _dialog.DismissAsync().Wait();
            _dialog = null;
        }
    }

    #endregion

    #region Actions

    public IElementHandle WaitElementAppear(string selector)
    {
        return FindElement(selector) ?? null;
    }

    public IElementHandle Click(string selector)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        element.ClickAsync(new ElementHandleClickOptions() {Force = _force}).Wait();
        element.DisposeAsync().AsTask().Wait();
        return element;
    }

    public IElementHandle Hover(string selector)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        element.HoverAsync(new() {Force = _force}).Wait();
        element.DisposeAsync().AsTask().Wait();
        return element;
    }

    public IElementHandle DoubleClick(string selector)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();

        element.DblClickAsync(new() {Force = _force}).Wait();
        element.DisposeAsync().AsTask().Wait();
        return element;
    }

    public IElementHandle SetText(string selector, string value, bool typeInto = false)
    {
        var element = FindElement(selector);
        if (element is null)
            throw new NullReferenceException();


        element.FillAsync("", new() {Force = _force}).Wait();
        if (typeInto)
            element.TypeAsync(value).Wait();
        else
            element.FillAsync(value, new() {Force = _force}).Wait();

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

    #endregion


    #region Searchers

    public IElementHandle FindElement(string selector, int retries = 120, double retryInterval = 0.125)
    {
        var element = Policy.HandleResult<IElementHandle>(result => result == null)
            .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
            .Execute(() =>
            {
                IElementHandle element = null;
                var pages = _context.Pages.ToArray();

                foreach (var w in pages)
                {
                    _page.WaitForLoadStateAsync(_pageWaitType, new PageWaitForLoadStateOptions() {Timeout = 60}).Wait();
                    //============================================================
                    try
                    {
                        element = w.QuerySelectorAsync(selector).Result;
                    }
                    catch
                    {
                        // ignored
                    }

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

                        try
                        {
                            element = frame.QuerySelectorAsync(selector).Result;
                        }
                        catch
                        {
                            // ignored
                        }

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

                return null;
            });

        return element;
    }

    public List<IElementHandle> FindAllDescendants(string selector)
    {
        var descendants = FindElement(selector).QuerySelectorAllAsync("*").Result.ToList();
        return descendants;
    }

    public List<IElementHandle> FindAllChildren(string selector)
    {
        var children = FindElement(selector).QuerySelectorAllAsync("xpath=child::*").Result.ToList();
        return children;
    }

    #endregion
}