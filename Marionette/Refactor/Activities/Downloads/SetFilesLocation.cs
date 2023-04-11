using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void SetFilesLocation(string path, List<IDownload> download)
    {
        if (path.Last().ToString() != @"\")
        {
            path += @"\";
        }

        download.ForEach(x=> x.SaveAsAsync(path + x.SuggestedFilename).Wait());
    }
}