using System.Security.Cryptography;
using System.Text;

<<<<<<< HEAD
<<<<<<< HEAD
namespace EasyGames.Web.Services
{
    // NOTE: Improved hashing with salt for better security
=======

namespace EasyGames.Web.Services
{
    // NOTE: demo-only hashing; ok for student project
>>>>>>> feature/akshata/data-shops
=======
namespace EasyGames.Web.Services
{
    // NOTE: Improved hashing with salt for better security
>>>>>>> origin/feature/hoang/pos-tier-email
    public static class Password
    {
        public static string Hash(string input)
        {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> origin/feature/hoang/pos-tier-email
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
<<<<<<< HEAD
        }
    }
}
=======
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? ""));
            return Convert.ToHexString(bytes);
        }
    }
}
>>>>>>> feature/akshata/data-shops
=======
        }
    }
}
>>>>>>> origin/feature/hoang/pos-tier-email
