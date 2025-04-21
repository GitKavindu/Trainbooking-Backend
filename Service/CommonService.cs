using System.Text;
using System.Security.Cryptography;
namespace Service;
public class CommonService
{
    public string GenerateSha256Hash(string input)
    {
        // Create a new instance of SHA256
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Convert the input string to a byte array and compute the hash
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert the byte array to a string (hexadecimal format)
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            // Return the hexadecimal string
            return builder.ToString();
        }
    }
}
