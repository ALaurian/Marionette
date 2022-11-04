using Microsoft.Playwright;
using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    private async void DownloadHandler(object sender, IDownload download)
    {
        Log.Information("Download started: {0}.", download.SuggestedFilename);
        fileDownloadSession.Add(await download.PathAsync());
        var waiter = download.PathAsync();
        downloadedFiles.Add(download);
        Log.Information("Downloaded file: {0}.", download.SuggestedFilename);
        fileDownloadSession.Remove(fileDownloadSession.First());
    }

    private void DialogHandler(object sender, IDialog dialog)
    {
        _dialog = dialog;
    }

    public List<IDownload> GetDownloadedFiles(string elementToVanish)
    {
        while (!fileDownloadSession.Any())
        {

        }

        while (fileDownloadSession.Any())
        {

        }

        var downloadedFilesList = downloadedFiles;
        downloadedFiles = new List<IDownload>();
        return downloadedFilesList; 
    }
    public List<IDownload> GetDownloadedFiles()
    {
        while (!fileDownloadSession.Any())
        {

        }

        while (fileDownloadSession.Any())
        {

        }

        var downloadedFilesList = downloadedFiles;
        downloadedFiles = new List<IDownload>();
        return downloadedFilesList;
    }

    public void SetFilesLocation(string path, IDownload download)
    {
        if (path.Last().ToString() != @"\")
        {
            path += @"\";
        }

        download.SaveAsAsync(path + download.SuggestedFilename).Wait();
    }
}