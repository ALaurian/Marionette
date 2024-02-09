using FlaUI.Core.AutomationElements;
using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public MarionetteWinBrowser SetActiveElement(AutomationElement element)
    {
        ActiveElement = element;
        return this;
    }
    public MarionetteWinBrowser SetActiveElement(string xPath)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error setting active element: {ex.Message}");
                    Console.WriteLine($"Retrying element activation. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            RefreshDesktop();
            ActiveElement = ActiveWindow.FindFirstByXPath(xPath);
        });

        return this;
    }

}