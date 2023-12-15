using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class EncryptionService
{
    private readonly string key; // Replace this with a secure way of storing and retrieving your encryption key

    public EncryptionService(string encryptionKey)
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentException("Encryption key cannot be null or empty.", nameof(encryptionKey));
        }

        if (encryptionKey.Length != 32) // AES-256 requires a 256-bit key
        {
            throw new ArgumentException("Encryption key must be 32 characters long.", nameof(encryptionKey));
        }

        this.key = encryptionKey;
    }

    public string Encrypt(string data)
    {
        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(data);
                }

                byte[] iv = aesAlg.IV;
                byte[] encryptedData = msEncrypt.ToArray();

                // Combine IV and encrypted data
                byte[] result = new byte[iv.Length + encryptedData.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encryptedData, 0, result, iv.Length, encryptedData.Length);

                return Convert.ToBase64String(result);
            }
        }
    }

    public string Decrypt(string encryptedData)
    {
        byte[] allBytes = Convert.FromBase64String(encryptedData);

        // Extract IV
        byte[] iv = new byte[16]; // AES block size is 128 bits (16 bytes)
        Buffer.BlockCopy(allBytes, 0, iv, 0, iv.Length);

        // Extract encrypted data
        byte[] encryptedBytes = new byte[allBytes.Length - iv.Length];
        Buffer.BlockCopy(allBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}