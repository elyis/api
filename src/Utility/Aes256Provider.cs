using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace api.src.Utility
{
    public class Aes256Provider
    {
        private static readonly string _key = "gUkXp2s5u8x/A?D(G+KbPeShVmYq3t6w";
        private static readonly string _IV = "HR$2pIjHR$2pIj12";

        private static Aes CreateAes()
        {
            var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_key);
            aesAlg.IV = Encoding.UTF8.GetBytes(_IV);
            return aesAlg;
        }

        public static string Decrypt(string input)
        {
            using var aesAlg = CreateAes();
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipherText = Convert.FromBase64String(input);

            using MemoryStream msDecrypt = new(cipherText);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);

            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        public static string Encrypt(string input)
        {
            using var aesAlg = CreateAes();
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using StreamWriter swEncrypt = new(csEncrypt);
            swEncrypt.Write(input);

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
    }

}