using System;
using System.Security.Cryptography;
using System.Text;

namespace ZipCo.Common.Domain
{
    public class Encryptor
    {
        public static string EncryptMD5(string input)
        {
            const string saltKey = "Z1pC0";
            string encrypted = string.Empty;

            using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
            {
                UTF8Encoding encoder = new UTF8Encoding();
                byte[] hashedData = md5Hasher.ComputeHash(encoder.GetBytes(string.Concat(saltKey, input)));
                encrypted = Convert.ToBase64String(hashedData);
            }

            return encrypted;
        }
    }
}