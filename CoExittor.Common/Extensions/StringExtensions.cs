using System.Security.Cryptography;
using System.Text;

namespace CoExittor.Common.Extensions
{
    public static class StringExtensions
    {
        extension(string str)
        {
            public string ToSHA512Hex()
            {
                byte[] encodedUTF8str = Encoding.UTF8.GetBytes(str);
                byte[] sha512str = SHA512.HashData(encodedUTF8str);
                return Convert.ToHexStringLower(sha512str);
            }
        }
    }
}
