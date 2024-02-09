namespace Marionette.Logger;

internal class MarionetteLogger
{
    public bool _enabledWriteConsole = false;
    public bool _enabledWriteToFile = false;
    public string currentTime = "";
    public void LogMessage(string message, string optional="")
    {
            WriteMessage(message, optional);
            WriteToFile(message, optional);
    }
    
    private void WriteMessage(string message, string optional= "")
    {
        if (_enabledWriteConsole)
        {
            Console.WriteLine(message + " " + optional);
        }
    }

    private void WriteToFile(string message, string optional = "")
    {
        if (_enabledWriteToFile)
        {
            if (currentTime != "")
            {
                File.AppendAllText("log.txt", Environment.NewLine);
                File.AppendAllText("log.txt", "------" + currentTime + "------" + Environment.NewLine);
                currentTime = "";
            }
            File.AppendAllText("log.txt", message + " " + optional + Environment.NewLine);
        }
    }
}