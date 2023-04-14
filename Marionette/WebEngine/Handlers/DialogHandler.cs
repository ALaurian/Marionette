using Microsoft.Playwright;

namespace Marionette.WebBrowser;

public partial class MarionetteWebBrowser
{
    private void DialogHandler(object sender, IDialog dialog)
    {
        _dialog = dialog;
    }
}