using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void SetFileLocation(string path, IDownload download)
    {
        if (path.Last().ToString() != @"\")
        {
            path += @"\";
        }

        download.SaveAsAsync(path + download.SuggestedFilename).Wait();
    }
}