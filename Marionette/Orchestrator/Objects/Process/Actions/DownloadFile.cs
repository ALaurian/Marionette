using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Marionette.Orchestrator;

public partial class Process
{
    public void DownloadFile(string pathOfFileToDownload, string userName = "MarionetteDownloader",
        string password = "")
    {
        var fileName = System.IO.Path.GetFileName(pathOfFileToDownload);
        var pathToDownloadFrom = @"Vault\" + fileName;

        try
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(userName, password);
                var requestUri = new Uri($"ftp://{_orchestrator.Connection.DataSource}/{pathToDownloadFrom}");

                client.DownloadFile(requestUri, pathOfFileToDownload);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
            throw;
        }
    }
}