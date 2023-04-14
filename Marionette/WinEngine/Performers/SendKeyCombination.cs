using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void SendKeyCombination(VirtualKeyShort[] keyShorts)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error sending key combination: {ex.Message}");
                    Console.WriteLine($"Retrying key combination sending. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            try
            {
                var keyCombination = Keyboard.Pressing(keyShorts);
                keyCombination.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

}