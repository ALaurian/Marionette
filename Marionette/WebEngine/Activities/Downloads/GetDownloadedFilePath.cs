using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public string GetDownloadedFilePath(int index)
    {
        return _downloadedFilesOut[index].PathAsync().Result;
    }
}