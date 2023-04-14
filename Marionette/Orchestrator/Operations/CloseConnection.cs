namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void CloseConnection()
    {
        Connection.Close();
    }
}