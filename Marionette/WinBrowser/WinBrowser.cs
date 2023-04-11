using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using Polly;

namespace Marionette.WinBrowser;

public class WinBrowser
{
    private UIA3Automation _uia3Automation { get; set; }
    private AutomationElement _desktop { get; set; }

    public WinBrowser(bool debugMode = false)
    {
        _uia3Automation = new UIA3Automation();
        _desktop = _uia3Automation.GetDesktop();
    }

    public static class Searchers
    {
        public static AutomationElement GetFromHandle(int processId)
        {
            var automation = new UIA3Automation();
            var allProcesses = Process.GetProcesses();
            //loop through processes
            foreach (var item in allProcesses)
                if (item.Id == processId)
                    return automation.FromHandle(item.MainWindowHandle);

            return null;
        }
        
        public static AutomationElement FindElement(AutomationElement window, string xPath, int retries = 15,
            double retryInterval = 1)
        {

            var automation = new UIA3Automation();
            var desktop = automation.GetDesktop();

            var element = Policy.HandleResult<AutomationElement>(result => result == null)
                .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => window.FindFirstByXPath(xPath));

            return element;
        }

        public static List<AutomationElement> FindElements(AutomationElement window, string xPath, int retries = 15,
            double retryInterval = 1)
        {
            var elements = Policy.HandleResult<List<AutomationElement>>(result => result == null)
                .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => window.FindAllByXPath(xPath).ToList());

            return elements;
        }
    }

    public static class Actions
    {
        public static string DesktopScreenshot(string saveToPath)
        {
            var automation = new UIA3Automation();
            var desktop = automation.GetDesktop();
            desktop.CaptureToFile(saveToPath);
            return saveToPath;
        }

        public static string ElementScreenshot(AutomationElement element, string saveToPath)
        {
            element.CaptureToFile(saveToPath);
            return saveToPath;
        }

        public static string GetText(AutomationElement element, int retries = 15, double retryInterval = 1)
        {
            var retryGetText = Policy.HandleResult<string>(result => result == null)
                .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
            var retryGetTextResult = retryGetText.Execute(() =>
            {
                try
                {
                    return element.AsTextBox().Text;
                }
                catch (Exception e)
                {
                    return null;
                }
            });
            return retryGetTextResult;
        }

        public static bool SendText(AutomationElement element, string text, bool eventTrigger = false, int retries = 15,
            double retryInterval = 1)
        {
            if (element != null)
            {
                if (eventTrigger == false)
                {
                    var retryClick = Policy.HandleResult<bool>(result => result)
                        .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
                    var retryResult = retryClick.Execute(() =>
                    {
                        try
                        {
                            element.AsTextBox().Text = text;

                            while (element.AsTextBox().Text != text) Thread.Sleep(250);
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }

                        return false;
                    });

                    return retryResult;
                }
                else
                {
                    var retryClick = Policy.HandleResult<bool>(result => result)
                        .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
                    var retryResult = retryClick.Execute(() =>
                    {
                        try
                        {
                            element.AsTextBox().Enter(text);

                            while (element.AsTextBox().Text != text) Thread.Sleep(250);
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }

                        return false;
                    });

                    return retryResult;
                }
            }

            return false;
        }

        public static bool Click(AutomationElement element, bool invoke = false, int retries = 15,
            double retryInterval = 1)
        {
            if (element != null)
            {
                if (invoke == false)
                {
                    var retryClick = Policy.HandleResult<bool>(result => result)
                        .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
                    var retryResult = retryClick.Execute(() =>
                    {
                        try
                        {
                            element.Click();
                            Thread.Sleep(100);
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }

                        return false;
                    });

                    return retryResult;
                }
                else
                {
                    var retryClick = Policy.HandleResult<bool>(result => result)
                        .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
                    var retryResult = retryClick.Execute(() =>
                    {
                        try
                        {
                            element.AsButton().Invoke();
                            Thread.Sleep(100);
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }

                        return false;
                    });

                    return retryResult;
                }
            }

            return false;
        }

        public static bool DoubleClick(AutomationElement element, bool invoke = false, int retries = 15,
            double retryInterval = 1)
        {
            if (element != null)
            {
                if (invoke == false)
                {
                    var retryClick = Policy.HandleResult<bool>(result => result)
                        .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
                    var retryResult = retryClick.Execute(() =>
                    {
                        try
                        {
                            element.DoubleClick();
                            Thread.Sleep(100);
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }

                        return false;
                    });

                    return retryResult;
                }
                else
                {
                    var retryClick = Policy.HandleResult<bool>(result => result)
                        .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
                    var retryResult = retryClick.Execute(() =>
                    {
                        try
                        {
                            element.AsButton().Invoke();
                            element.AsButton().Invoke();
                            Thread.Sleep(100);
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }

                        return false;
                    });

                    return retryResult;
                }
            }

            return false;
        }

        public static bool RightClick(AutomationElement element, bool invoke = false, int retries = 15,
            double retryInterval = 1)
        {
            if (element != null)
            {
                if (invoke == false)
                {
                    var retryClick = Policy.HandleResult<bool>(result => result)
                        .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval));
                    var retryResult = retryClick.Execute(() =>
                    {
                        try
                        {
                            element.RightClick();
                            Thread.Sleep(100);
                        }

                        catch (Exception e)
                        {
                            // ignored
                        }

                        return false;
                    });

                    return retryResult;
                }
            }

            return false;
        }

        public static void SendKeys(VirtualKeyShort keyShort)
        {
            Keyboard.Press(keyShort);
            Keyboard.Release(keyShort);
        }

        public static void SendKeyCombination(VirtualKeyShort[] keyShorts)
        {
            var keyCombination = Keyboard.Pressing(keyShorts);
            keyCombination.Dispose();
        }
    }
}