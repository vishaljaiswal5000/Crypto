using System;
using System.Security.Cryptography;
using System.Text;

namespace Crypto
{
    class Program
    {

        static string key { get; set; } = "SomeKey";

        static void Main(string[] args)
        {

            var Cipher = Encrypt("admin1*");
            var UnCipher = Decrypt(Cipher);
            Console.WriteLine("Cipher text: "+ Cipher);
            Console.WriteLine("UnCipher text: " + UnCipher);

        }


        public static string Encrypt(string text)
        {
            {
                byte[] data = UTF8Encoding.UTF8.GetBytes(text);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() 
                    { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateEncryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        return Convert.ToBase64String(results, 0, results.Length);
                    }
                }
            }
        }

        public static string Decrypt(string value)
        {

            byte[] data = Convert.FromBase64String(value); // decrypt the incrypted text
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() 
                { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }

        }
    }
}