using Serilog;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public bool AcceptDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            Log.Information("Accepting dialog: {Message}", _dialog.Message);
            _dialog.AcceptAsync().Wait();
            _dialog = null;
            return true;
        }

        return false;
    }
}