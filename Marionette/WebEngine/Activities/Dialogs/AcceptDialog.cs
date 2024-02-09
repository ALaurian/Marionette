using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public bool AcceptDialog()
    {
        if (_dialog != null)
        {
            _logger.LogMessage("Accepting dialog: {Message}", _dialog.Message);
            _dialog.AcceptAsync().Wait();
            _dialog = null;
            return true;
        }

        return false;
    }
}