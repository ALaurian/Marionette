using System.Data;
using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    
    public List<IDownload> GetDownloadedFiles(string elementToVanish, int retries, bool lockToLastPage)
    {
        WaitElementVanish(elementToVanish, retries, lockToLastPage);
        FileWaiter();
        _downloadedFilesOut = _downloadedFiles;
        _downloadedFiles = new List<IDownload>();
        return _downloadedFilesOut;
    }
    
    public List<IDownload> GetDownloadedFiles()
    {
        FileWaiter();
        _downloadedFilesOut = _downloadedFiles;
        _downloadedFiles = new List<IDownload>();
        return _downloadedFilesOut;
    }
    
    private void FileWaiter()
    {
        while (!_fileDownloadSession.Any()){}
        while (_fileDownloadSession.Any()){}
    }
}