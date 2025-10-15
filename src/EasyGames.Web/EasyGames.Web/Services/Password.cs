using System.Security.Cryptography;
using System.Text;


namespace EasyGames.Web.Services
{
    // NOTE: demo-only hashing; ok for student project
    public static class Password
    {
        public static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? ""));
            return Convert.ToHexString(bytes);
        }
    }
}
