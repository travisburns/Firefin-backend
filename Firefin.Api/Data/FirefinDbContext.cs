using Firefin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Firefin.Api.Data;

public class FirefinDbContext : DbContext
{
    public FirefinDbContext(DbContextOptions<FirefinDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<BatchNote> BatchNotes => Set<BatchNote>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.HasIndex(p => p.Slug).IsUnique();
            e.Property(p => p.TargetPrice).HasPrecision(10, 2);
            e.HasMany(p => p.Recipes)
                .WithOne(r => r.Product!)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Recipe>(e =>
        {
            e.HasIndex(r => new { r.ProductId, r.Version }).IsUnique();
            e.HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe!)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasMany(r => r.Batches)
                .WithOne(b => b.Recipe!)
                .HasForeignKey(b => b.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RecipeIngredient>()
            .Property(i => i.Grams).HasPrecision(10, 2);

        modelBuilder.Entity<Batch>(e =>
        {
            e.HasIndex(b => new { b.RecipeId, b.BatchNumber }).IsUnique();
            e.HasMany(b => b.Notes)
                .WithOne(n => n.Batch!)
                .HasForeignKey(n => n.BatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasIndex(o => o.OrderNumber).IsUnique();
            e.Property(o => o.Subtotal).HasPrecision(10, 2);
            e.HasMany(o => o.Items)
                .WithOne(i => i.Order!)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.Property(i => i.UnitPrice).HasPrecision(10, 2);
            e.Property(i => i.LineTotal).HasPrecision(10, 2);
        });
    }
}
