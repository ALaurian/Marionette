using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Marionette.Logger;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using MoreLinq;
using NUnit.Framework;
using Serilog;

namespace Marionette.WebBrowser
{
    public partial class MarionetteWebBrowser
    {
        private readonly PlaywrightTest _expectEngine = new PageTest();
        private readonly IBrowser _browser;
        private readonly IBrowserContext _context;
        private readonly List<IPage> _pages = new();
        private List<IDownload> _downloadedFiles = new();
        private List<string> _fileDownloadSession = new();
        private IDialog _dialog;
        public bool _force;

        private MarionetteLogger _logger = new();

        public bool DebugMode { get; set; } = false;
        public int DebugModeDuration { get; set; } = 3;
        public LoadState PageWaitType { get; set; } = LoadState.DOMContentLoaded;
        
        public void enableWriteConsole()
        {
            _logger._enabledWriteConsole = true;
        }
        
        public void disableWriteConsole()
        {
            _logger._enabledWriteConsole = false;
        }
        
        public void enableWriteToFile()
        {
            //get the name of the executable running
            string fileName = Process.GetCurrentProcess().MainModule.FileName;
            //get the name of the process without the extension
            string processName = System.IO.Path.GetFileNameWithoutExtension(fileName);
            
            Console.WriteLine(processName + " executing...");
            
            _logger.currentTime = processName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            _logger._enabledWriteToFile = true;
        }
        
        public void disableWriteToFile()
        {
            _logger._enabledWriteToFile = false;
        }
        public MarionetteWebBrowser(bool connectSession, int port = 8080,
            string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
            BrowserType browserType = BrowserType.Chrome, LoadState pageWaitType = LoadState.DOMContentLoaded)
        {
            if (connectSession)
            {
                _browser = ConnectToExistingSession(port, browserType);
            }
            else
            {
                _browser = StartNewSession(port, browserType, browserPath);
            }

            _context = _browser.Contexts.First();
            _pages.Add(_context.Pages.First());
            PageWaitType = pageWaitType;
            _pages[0].Download += DownloadHandler;
            _pages[0].Dialog += DialogHandler;

        }

        public MarionetteWebBrowser(BrowserType browserType, LoadState pageWaitType = LoadState.DOMContentLoaded,
            bool headlessMode = false)
        {
            
            var playwright = Playwright.CreateAsync().Result;

            _browser = browserType switch
            {
                BrowserType.Chrome => playwright.Chromium
                    .LaunchAsync(new BrowserTypeLaunchOptions { Headless = headlessMode }).Result,
                BrowserType.Firefox => playwright.Firefox
                    .LaunchAsync(new BrowserTypeLaunchOptions { Headless = headlessMode }).Result,
                BrowserType.WebKit => playwright.Webkit
                    .LaunchAsync(new BrowserTypeLaunchOptions { Headless = headlessMode }).Result,
                _ => null
            };


            _context = _browser.NewContextAsync().Result;
            _pages.Add(_context.NewPageAsync().Result);
            PageWaitType = pageWaitType;
            _pages[0].Download += DownloadHandler;
            _pages[0].Dialog += DialogHandler;
            
        }

        private IBrowser ConnectToExistingSession(int port, BrowserType browserType)
        {
            
            var playwright = Playwright.CreateAsync().Result;

            return browserType switch
            {
                BrowserType.Chrome => playwright.Chromium.ConnectOverCDPAsync("http://localhost:" + port).Result,
                BrowserType.Firefox => throw new Exception("Firefox is not supported at this moment."),
                _ => null
            };
        }

        private IBrowser StartNewSession(int port, BrowserType browserType, string browserPath)
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

            return browserType switch
            {
                BrowserType.Chrome => playwright.Chromium.ConnectOverCDPAsync("http://localhost:" + port).Result,
                BrowserType.Firefox => throw new Exception("Firefox is not supported at this moment."),
                _ => null
            };
        }
        
        private List<IDownload> _downloadedFilesOut;
    }
}