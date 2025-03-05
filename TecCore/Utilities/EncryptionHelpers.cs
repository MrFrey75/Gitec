using System.Security.Cryptography;
using System.Text;

namespace TecCore.Utilities;

public static class EncryptionHelpers
{
    public static string HashString(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            
            return Convert.ToHexString(hashBytes); // Converts to uppercase hex string
        }
    }
    
    public static bool VerifyHashedString(string PasswordHash, string PasswordClearText)
    {
        return PasswordHash == HashString(PasswordClearText);
    }
}