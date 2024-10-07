using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace PasswordMng_v0._01
{
    internal class Encryption
    {

        public string Decrypt(byte[] cypher, byte[] key, byte[] iv)
        {
            
            string plainText = String.Empty;
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(cypher))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                        {
                            plainText = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }

        public byte[] Encrypt(string str, byte[] key, byte[] iv)
        {

            byte[] cypher;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {

                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream, Encoding.UTF8))
                        {
                            streamWriter.Write(str);
                        }
                        cypher = memoryStream.ToArray();
                    }
                }
            }
            return cypher;
        }

        public byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[16]; // Ein zufälliges Salt sollte hier verwendet werden
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public byte[] GenerateRandomIV()
        {
            byte[] iv = new byte[16]; // Ein zufälliger IV sollte hier verwendet werden
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }

        public byte[] GenerateKeyFromPassword(string password, byte[] salt, int iterations, int keySize)
        {
            using (var argon2 = new Argon2d(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8; // Anzahl der Threads
                argon2.MemorySize = 65536; // Speichergröße in KB
                argon2.Iterations = iterations;

                return argon2.GetBytes(keySize);
            }
        }
    }
}
