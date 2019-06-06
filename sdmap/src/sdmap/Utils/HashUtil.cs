using System;
using System.Text;

namespace sdmap.Utils
{
    internal static class HashUtil
    {
        public static string Base64SHA256(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var cipher = sha256.ComputeHash(bytes);
                return Convert
                    .ToBase64String(cipher)
                    .Replace("=", "")
                    .Replace("+", "_")
                    .Replace("/", "θ");
            }
        }
    }
}
