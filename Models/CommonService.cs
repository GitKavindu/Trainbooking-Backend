using System.Text;
using System.Security.Cryptography;
using System.Globalization;
namespace Models;
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

    public string CleanMessage(Exception ex)
    {
        string[] parts = ex.Message.Split(new[] { ": " }, 2, StringSplitOptions.None);
        string cleanMessage = parts.Length > 1 ? parts[1] : ex.Message;

        return cleanMessage;
    }

    public DateTime CombineDateAndTime(string date, string time)
    {
        string combined = $"{date} {time}"; // "2025-06-29 06.00 PM"
        string format = "yyyy-MM-dd hh:mm tt";
        return DateTime.ParseExact(combined, format, CultureInfo.InvariantCulture);
    }
    
}
