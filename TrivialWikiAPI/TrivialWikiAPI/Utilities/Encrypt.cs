using System.Security.Cryptography;
using System.Text;

namespace TrivialWikiAPI.Utilities
{
    public static class Encrypt
    {
        public static string GetMD5(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                md5.ComputeHash(Encoding.ASCII.GetBytes(text));
                byte[] result = md5.Hash;
                StringBuilder str = new StringBuilder();
                for (int i = 1; i < result.Length; i++)
                {
                    str.Append(result[i].ToString());
                }
                return str.ToString();
            }
        }
    }
}