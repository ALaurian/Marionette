using System.Runtime.InteropServices;
using FlaUI.Core.AutomationElements;
using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void Click(bool simulateClick, bool mouseMove = false)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error clicking element: {ex.Message}");
                    Console.WriteLine($"Retrying click. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            try
            {
                if (simulateClick)
                {
                    ActiveElement.AsButton().Invoke();
                }
                else
                {
                    ActiveElement.Click(mouseMove);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

}