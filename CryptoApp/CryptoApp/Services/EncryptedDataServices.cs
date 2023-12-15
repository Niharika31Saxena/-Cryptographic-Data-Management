using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

public class EncryptedDataService
{
    private readonly ApplicationDbContext _context;

    public EncryptedDataService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Task 1: Insert encrypted data into the EncryptedData table.
    public void InsertEncryptedData(EncryptedData encryptedData)
    {
        _context.EncryptedData.Add(encryptedData);
        _context.SaveChanges();
    }

    // Task 2: Retrieve and decrypt data from the database based on the Id.
    public string RetrieveAndDecryptDataById(int id)
    {
        var encryptedData = _context.EncryptedData.Find(id);

        if (encryptedData == null)
        {
            throw new InvalidOperationException("Encrypted data not found.");
        }

        // Implement your decryption logic here using EncryptionService
        string decryptedData = EncryptionService.Decrypt(encryptedData.Data, encryptedData.InitializationVector);

        return decryptedData;
    }

    // Task 3: Update encrypted data.
    public void UpdateEncryptedData(int id, EncryptedData updatedData)
    {
        var existingData = _context.EncryptedData.Find(id);

        if (existingData == null)
        {
            throw new InvalidOperationException("Encrypted data not found.");
        }

        // Update properties as needed
        existingData.Data = updatedData.Data;
        existingData.InitializationVector = updatedData.InitializationVector;

        _context.SaveChanges();
    }

    // Task 4: Delete encrypted data.
    public void DeleteEncryptedData(int id)
    {
        var encryptedData = _context.EncryptedData.Find(id);

        if (encryptedData == null)
        {
            throw new InvalidOperationException("Encrypted data not found.");
        }

        _context.EncryptedData.Remove(encryptedData);
        _context.SaveChanges();
    }

    // Task 5: List all stored encrypted data.
    public IQueryable<EncryptedData> ListAllEncryptedData()
    {
        return _context.EncryptedData.AsQueryable();
    }
}

public class EncryptionService
{
    private readonly string encryptionKey; // Replace with a secure way of storing and retrieving your encryption key

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

        this.encryptionKey = encryptionKey;
    }

    public string Encrypt(string data, string initializationVector)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = Convert.FromBase64String(initializationVector);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(data);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public string Decrypt(string encryptedData, string initializationVector)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = Convert.FromBase64String(initializationVector);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedData)))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}

// Your existing ApplicationDbContext and EncryptedData model classes
public class ApplicationDbContext : DbContext
{
    // Your DbContext code
}

public class EncryptedData
{
    // Your EncryptedData model code
}