namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void ElementScreenshot(string saveToPath)
    {
        ActiveElement.CaptureToFile(saveToPath);
    }
}