using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public bool AcceptDialog()
    {
        if (_dialog != null)
        {
            Log.Information("Accepting dialog: {Message}", _dialog.Message);
            _dialog.AcceptAsync().Wait();
            _dialog = null;
            return true;
        }

        return false;
    }
}