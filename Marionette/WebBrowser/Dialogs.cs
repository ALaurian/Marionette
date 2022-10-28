namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    public void AcceptDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            Serilog.Log.Information("Accepting dialog: {Message}", _dialog.Message);
            _dialog.AcceptAsync().Wait();
            _dialog = null;
        }
    }

    public void DismissDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            Serilog.Log.Information("Dismissing dialog: {Message}", _dialog.Message);
            _dialog.DismissAsync().Wait();
            _dialog = null;
        }
    }

    public void AcceptDialog()
    {
        if (_dialog != null)
        {
            Serilog.Log.Information("Accepting dialog: {Message}", _dialog.Message);
            _dialog.AcceptAsync().Wait();
            _dialog = null;
        }
    }

    public void DismissDialog()
    {
        if (_dialog != null)
        {
            Serilog.Log.Information("Dismissing dialog: {Message}", _dialog.Message);
            _dialog.DismissAsync().Wait();
            _dialog = null;
        }
    }
}