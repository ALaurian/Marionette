using System.Net;

namespace Marionette.Orchestrator;

public partial class Process
{
    public void UploadFile(string pathOfFileToUpload, string userName = "MarionetteUploader", string password = "")
    {
        var fileName = System.IO.Path.GetFileName(pathOfFileToUpload);
        var pathToUploadTo = @"Vault\" + fileName;

        try
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(userName, password);
                var requestUri = new Uri($"ftp://{_orchestrator.Connection.DataSource}/{pathToUploadTo}");

                client.UploadFile(requestUri, WebRequestMethods.Ftp.UploadFile, pathOfFileToUpload);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            throw;
        }
    }


}