namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    private void RefreshDesktop()
    {
        _desktop = _automation.GetDesktop().FindAllByXPath("*").ToList();
    }
}