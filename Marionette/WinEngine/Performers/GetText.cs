using FlaUI.Core.AutomationElements;
using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public string GetText()
    {
        string text = string.Empty;

        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error getting element text: {ex.Message}");
                    Console.WriteLine($"Retrying text retrieval. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            try
            {
                text = ActiveElement.AsTextBox().Text;
            }
            catch (Exception ex)
            {
                throw;
            }
        });

        return text;
    }

}