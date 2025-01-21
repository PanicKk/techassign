using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WMS.Models.Entities;
using WMS.Models.Entities.Shared;

namespace WMS.Core.Data;

public class WMSDbContext : DbContext
{
    public WMSDbContext(DbContextOptions<WMSDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public virtual DbSet<Webhook> Webhooks { get; set; }
    public virtual DbSet<ActivityLog> Logs { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        SetBaseEntityData();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetBaseEntityData();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetBaseEntityData()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State is EntityState.Added or EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }
    }
}