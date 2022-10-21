using Microsoft.Playwright;
using WinRT;

namespace Marionette.WebBrowser;

public partial class PlayWebBrowser
{
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

    public void SetFileLocation(string path, IDownload download)
    {
        if (path.Last().ToString() != @"\")
        {
            path += @"\";
        }

        download.SaveAsAsync(path + download.SuggestedFilename).Wait();
    }
}