using ABMProductos.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ABMProductos.Data;

public class ABMProductosContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ABMProductosContext(DbContextOptions<ABMProductosContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Mouse inalámbrico", Price = 3999.99m });

        base.OnModelCreating(modelBuilder);
    }
}