using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public bool DismissDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            _logger.LogMessage("Dismissing dialog: {Message}", _dialog.Message);
            _dialog.DismissAsync().Wait();
            _dialog = null;
            return true;
        }

        return false;
    }
}