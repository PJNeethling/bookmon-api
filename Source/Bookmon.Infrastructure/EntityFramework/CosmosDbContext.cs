using Bookmon.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookmon.Infrastructure.EntityFramework;

public sealed class CosmosDbContext : DbContext
{
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options)
    {
    }

    public CosmosDbContext()
    { }

    public DbSet<BookDto> Books { get; set; }

    public DbSet<OrderDto> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookDto>()
            .ToContainer("Books")
            .HasPartitionKey(e => e.Id);

        modelBuilder.Entity<OrderDto>()
            .ToContainer("Orders")
            .HasPartitionKey(e => e.Id);

        base.OnModelCreating(modelBuilder);
    }

    public void ClearContainer<T>() where T : class
    {
        var allItems = Set<T>().ToList();
        Set<T>().RemoveRange(allItems);
        SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedDate") is not null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedDate").IsModified = false;
                entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}