using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public void SendKey(VirtualKeyShort keyShort)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error sending key: {ex.Message}");
                    Console.WriteLine($"Retrying key sending. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            try
            {
                Keyboard.Press(keyShort);
                Keyboard.Release(keyShort);
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

}