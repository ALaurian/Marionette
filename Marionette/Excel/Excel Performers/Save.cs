using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Polly;

namespace Marionette.Excel_Scope;

public partial class Excel
{
    public string Save()
    {
        try
        {
            workbook.Save();
            return workbook.Path;
        }
        catch (Exception e)
        {
            try
            {
                var automation = new UIA3Automation();
                var desktop = automation.GetDesktop();
                var window = Policy.HandleResult<Window>(result => result == null)
                    .WaitAndRetry(15, retryAttempt => TimeSpan.FromSeconds(1))
                    .Execute(() => desktop.FindFirstByXPath("*[contains(@Name,'" + workbook.Name + "')]").AsWindow());

                var element = Policy.HandleResult<AutomationElement>(result => result == null)
                    .WaitAndRetry(15, interval => TimeSpan.FromSeconds(1))
                    .Execute(() => window.FindFirstByXPath("//*[@Name='Save']"));
                element.AsButton().Invoke();

                return workbook.Path;
            }
            catch (Exception exception)
            {
                throw new Exception("File failed to save.");
            }
        }
    }
    
    
}