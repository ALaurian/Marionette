using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void RightClick(bool mouseMove = false)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error right-clicking element: {ex.Message}");
                    Console.WriteLine($"Retrying right-click. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            try
            {
                ActiveElement.RightClick(mouseMove);
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

}