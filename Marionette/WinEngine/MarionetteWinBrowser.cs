using FlaUI.Core.AutomationElements;
using FlaUI.Core.Identifiers;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    private UIA3Automation _automation;

    private Window ActiveWindow;
    private AutomationElement ActiveElement;
    private List<AutomationElement> ActiveElements;

    private List<AutomationElement> _desktop;

    public int retryCount = 3;
    public TimeSpan RetryTimeSpan = TimeSpan.FromSeconds(1);

    public MarionetteWinBrowser()
    {
        _automation = new UIA3Automation();
        _desktop = _automation.GetDesktop().FindAllByXPath("*").ToList();

    }
}
