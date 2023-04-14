using System.Runtime.InteropServices;
using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void DoubleRightClick(bool mouseMove = false)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error double-right-clicking element: {ex.Message}");
                    Console.WriteLine($"Retrying double-right-click. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            try
            {
                ActiveElement.RightDoubleClick(mouseMove);
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

}