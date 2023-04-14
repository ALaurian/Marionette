using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void DoubleClick(bool mouseMove = false)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error double-clicking element: {ex.Message}");
                    Console.WriteLine($"Retrying double-click. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            try
            {
                ActiveElement.DoubleClick(mouseMove);
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

}