using System.Security.Cryptography;
using System.Text;

namespace WinFormsAuthApp.Security
{
    public static class PasswordHasher
    {
        public static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        public static bool Verify(string plainText, string expectedHashHex)
        {
            var computed = ComputeSha256(plainText);
            return string.Equals(computed, expectedHashHex, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}


