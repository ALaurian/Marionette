using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class PlayWebBrowser
{
    public IDialog GetDialog() => _dialog;

    public void AcceptDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            _dialog.AcceptAsync().Wait();
            _dialog = null;
        }
    }

    public void DismissDialogIf(List<string> dialogTexts)
    {
        if (_dialog != null && dialogTexts.Any(x => _dialog.Message.Contains(x)))
        {
            _dialog.DismissAsync().Wait();
            _dialog = null;
        }
    }

    public void AcceptDialog()
    {
        if (_dialog != null)
        {
            _dialog.AcceptAsync().Wait();
            _dialog = null;
        }
    }

    public void DismissDialog()
    {
        if (_dialog != null)
        {
            _dialog.DismissAsync().Wait();
            _dialog = null;
        }
    }
}