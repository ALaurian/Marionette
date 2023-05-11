using System.Security;
using System.Security.Cryptography;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public SecureString RetrieveSecureString(string name)
    {
        var command = Connection.CreateCommand();
        command.CommandText = "SELECT Value FROM assets WHERE Name = ?Name";
        command.Parameters.AddWithValue("?Name", name);

        MySqlDataReader reader = null;
        try
        {
            reader = command.ExecuteReader();

            if (reader.Read())
            {
                var encryptedData = (byte[])reader.GetValue(0);

                // Decrypt the data using ProtectedData
                var data = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);

                // Convert byte array to SecureString
                var secureString = new SecureString();
                for (var i = 0; i < data.Length; i += 2)
                {
                    var c = BitConverter.ToChar(data, i);
                    secureString.AppendChar(c);
                }

                secureString.MakeReadOnly();
                return secureString;
            }
            else
            {
                throw new Exception("No entry found with the specified Name.");
            }
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
    }
}