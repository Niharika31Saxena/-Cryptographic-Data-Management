using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Models
{
    // EncryptedData.cs
    public class EncryptedData
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public byte[] InitializationVector { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
