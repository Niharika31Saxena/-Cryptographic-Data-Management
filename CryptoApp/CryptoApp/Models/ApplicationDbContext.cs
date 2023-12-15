using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<EncryptedData> EncryptedData { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EncryptedData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Data).IsRequired();
            entity.Property(e => e.InitializationVector).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
        });

        // Additional configurations if needed

        base.OnModelCreating(modelBuilder);
    }
}

public class EncryptedData
{
    public int Id { get; set; }
    public string Data { get; set; }
    public string InitializationVector { get; set; }
    public DateTime Timestamp { get; set; }
}