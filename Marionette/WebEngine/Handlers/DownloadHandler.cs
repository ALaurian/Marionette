using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    private async void DownloadHandler(object sender, IDownload download)
    {
        _logger.LogMessage("Download started: {0}.", download.SuggestedFilename);
        _fileDownloadSession.Add(await download.PathAsync());
        var waiter = download.PathAsync();
        _downloadedFiles.Add(download);
        _logger.LogMessage("Downloaded file: {0}.", download.SuggestedFilename);
        _fileDownloadSession.Remove(await download.PathAsync());
    }
}