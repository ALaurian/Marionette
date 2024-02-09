using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public bool DismissDialog()
    {
        if (_dialog != null)
        {
            _logger.LogMessage("Dismissing dialog: {Message}", _dialog.Message);
            _dialog.DismissAsync().Wait();
            _dialog = null;
            return true;
        }

        return false;
    }
}