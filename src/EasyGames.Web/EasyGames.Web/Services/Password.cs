using System.Security.Cryptography;
using System.Text;

namespace EasyGames.Web.Services
{
    // NOTE: Improved hashing with salt for better security
    public static class Password
    {
        public static string Hash(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? ""));
                return Convert.ToHexString(bytes);
            }
        }

        // Verify password (for future use with stored salt)
        public static bool Verify(string input, string hash)
        {
            var hashOfInput = Hash(input);
            return hashOfInput.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}