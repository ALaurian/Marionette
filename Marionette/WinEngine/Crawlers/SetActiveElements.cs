using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public MarionetteWinBrowser SetActiveElements(string xPath)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error setting active elements: {ex.Message}");
                    Console.WriteLine($"Retrying elements activation. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            RefreshDesktop();
            ActiveElements = ActiveWindow.FindAllByXPath(xPath).ToList();
        });

        return this;
    }


}