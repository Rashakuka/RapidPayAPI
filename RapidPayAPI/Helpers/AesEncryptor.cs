using System.Security.Cryptography;
using System.Text;

namespace RapidPayAPI.EncryptionLibrary
{
    public class AesEncryptor
    {
        public byte[] EncryptAES(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256; 
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.GenerateIV(); 

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(plainText);
                        csEncrypt.Write(data, 0, data.Length);
                    }

                    byte[] encryptedData = msEncrypt.ToArray();
                    byte[] result = new byte[aesAlg.IV.Length + encryptedData.Length];

                    Array.Copy(aesAlg.IV, result, aesAlg.IV.Length);

                    Array.Copy(encryptedData, 0, result, aesAlg.IV.Length, encryptedData.Length);

                    return result;
                }
            }
        }

        public string DecryptAES(byte[] combinedData, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256; 
                aesAlg.Key = Encoding.UTF8.GetBytes(key);

                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] encryptedData = new byte[combinedData.Length - iv.Length];

                Array.Copy(combinedData, iv, iv.Length);

                Array.Copy(combinedData, iv.Length, encryptedData, 0, encryptedData.Length);

                aesAlg.IV = iv; 

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}