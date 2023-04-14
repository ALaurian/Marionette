namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void DesktopScreenshot(string saveToPath)
    {
        _automation.GetDesktop().CaptureToFile(saveToPath);
    }
}