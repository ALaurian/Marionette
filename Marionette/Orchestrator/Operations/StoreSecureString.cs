using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Orchestrator
{
    public void StoreSecureString(string name, SecureString value)
    {
        MySqlCommand command = Connection.CreateCommand();
        command.CommandText =
            "INSERT INTO assets (Name, Value) SELECT ?Name, ?Value WHERE NOT EXISTS (SELECT * FROM assets WHERE Name = ?Name)";
        byte[] encryptedData = null;

        // Convert SecureString to byte array
        IntPtr bstr = Marshal.SecureStringToBSTR(value);
        byte[] data = new byte[value.Length * 2];
        try
        {
            Marshal.Copy(bstr, data, 0, data.Length);
        }
        finally
        {
            Marshal.ZeroFreeBSTR(bstr);
            value.Dispose();
        }

        // Encrypt the data using ProtectedData
        encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

        MySqlParameter nameParam = new MySqlParameter("?Name", MySqlDbType.VarString);
        nameParam.Value = name;
        command.Parameters.Add(nameParam);

        MySqlParameter valueParam = new MySqlParameter("?Value", MySqlDbType.VarBinary);
        valueParam.Value = encryptedData;
        command.Parameters.Add(valueParam);

        try
        {
            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 0)
            {
                throw new Exception("Entry with the same Name already exists.");
            }
        }
        finally
        {
            // Do not close the connection here if you still need it for other operations
        }
    }
}