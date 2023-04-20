namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public string GetDownloadedFilePath(int index)
    {
        return _downloadedFiles.ElementAt(index).SuggestedFilename;
    }
}