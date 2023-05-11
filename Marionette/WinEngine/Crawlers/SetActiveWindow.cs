using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using MoreLinq.Extensions;
using Polly;

namespace Marionette.WinEngine;

public partial class MarionetteWinBrowser
{
    public Window SetActiveWindow(FrameworkType frameworkType, string Name = null, string ClassName = null, ControlType ControlType = ControlType.Window)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => RetryTimeSpan,
                (ex, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Error setting active window: {ex.Message}");
                    Console.WriteLine($"Retrying window activation. {retryCount} retries left.");
                });

        retryPolicy.Execute(() =>
        {
            RefreshDesktop();
            ActiveWindow = _desktop
                .First(x =>
                    (Name == null || x.Name == Name) &&
                    (ClassName == null || x.ClassName == ClassName) &&
                    (x.ControlType == ControlType) &&
                    (x.FrameworkType == frameworkType)).AsWindow();
        });

        return ActiveWindow;
    }


}