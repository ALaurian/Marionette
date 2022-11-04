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

    public bool DismissDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            Log.Information("Dismissing dialog: {Message}", _dialog.Message);
            _dialog.DismissAsync().Wait();
            _dialog = null;
            return true;
        }

        return false;
    }

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

    public bool DismissDialog()
    {
        if (_dialog != null)
        {
            Log.Information("Dismissing dialog: {Message}", _dialog.Message);
            _dialog.DismissAsync().Wait();
            _dialog = null;
            return true;
        }
        
        return false;
    }
}