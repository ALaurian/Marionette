using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using Polly;

namespace Marionette;

public class WinBrowser
{
    private UIA3Automation _uia3Automation { get; set; }
    private AutomationElement _desktop { get; set; }

    public WinBrowser(bool debugMode = false)
    {
        

        
        _uia3Automation = new UIA3Automation();
        _desktop = _uia3Automation.GetDesktop();

    }

    public Window GetWindow(int processID)
    {
        return Searchers.GetWindow(processID);
    }

    public Window GetWindow(string XPath, int retries = 15, double retryInterval = 1)
    {
        return Searchers.GetWindow(XPath, retries, retryInterval);
    }

    public List<Window> GetWindows(string XPath, int retries = 15, double retryInterval = 1)
    {
        return Searchers.GetWindows(XPath, retries, retryInterval);
    }

    public static class Searchers
    {
        public static Window GetWindow(int processId)
        {
            var automation = new UIA3Automation();
            var allProcesses = Process.GetProcesses();
            //loop through processes
            foreach (var item in allProcesses)
                if (item.Id == processId)
                    return automation.FromHandle(item.MainWindowHandle).AsWindow();

            return null;
        }
        public static Window GetWindow(string xPath, int retries = 15, double retryInterval = 1)
        {
            var automation = new UIA3Automation();
            var desktop = automation.GetDesktop();
            var window = Polly.Policy.HandleResult<Window>(result => result == null)
                .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => desktop.FindFirstByXPath(xPath).AsWindow());

            return window;
        }

        public static List<Window> GetWindows(string xPath, int retries = 15, double retryInterval = 1)
        {
            var automation = new UIA3Automation();
            var desktop = automation.GetDesktop();
            var window = Polly.Policy.HandleResult<List<Window>>(result => result.Count == 0)
                .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => desktop.FindAllByXPath(xPath).Cast<Window>().ToList());

            return window;
        }

        public static AutomationElement FindElement(Window window, string xPath, int retries = 15,
            double retryInterval = 1)
        {
            var element = Policy.HandleResult<AutomationElement>(result => result == null)
                .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => window.FindFirstByXPath(xPath));

            return element;
        }

        public static List<AutomationElement> FindElements(Window window, string xPath, int retries = 15,
            double retryInterval = 1)
        {
            var elements = Policy.HandleResult<List<AutomationElement>>(result => result == null)
                .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => window.FindAllByXPath(xPath).ToList());

            return elements;
        }

        public static AutomationElement FindElementInElement(AutomationElement element, string xPath,
            int retries = 15, double retryInterval = 1)
        {
            var welement = Policy.HandleResult<AutomationElement>(result => result == null)
                .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => element.FindFirstByXPath(xPath));

            return welement;
        }

        public static List<AutomationElement> FindElementsInElement(AutomationElement element, string xPath,
            int retries = 15, double retryInterval = 1)
        {
            var welements = Policy.HandleResult<List<AutomationElement>>(result => result == null)
                .WaitAndRetry(retries, interval => TimeSpan.FromSeconds(retryInterval))
                .Execute(() => element.FindAllByXPath(xPath).ToList());

            return welements;
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
                    var retryClick = Policy.HandleResult<bool>(result => result == true)
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
                    var retryClick = Policy.HandleResult<bool>(result => result == true)
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
                    var retryClick = Policy.HandleResult<bool>(result => result == true)
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
                    var retryClick = Policy.HandleResult<bool>(result => result == true)
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
                    var retryClick = Policy.HandleResult<bool>(result => result == true)
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
                    var retryClick = Policy.HandleResult<bool>(result => result == true)
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
                    var retryClick = Policy.HandleResult<bool>(result => result == true)
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
            FlaUI.Core.Input.Keyboard.Press(keyShort);
            FlaUI.Core.Input.Keyboard.Release(keyShort);
        }

        public static void SendKeyCombination(VirtualKeyShort[] keyShorts)
        {
            var keyCombination = FlaUI.Core.Input.Keyboard.Pressing(keyShorts);
            keyCombination.Dispose();
        }
    }
}