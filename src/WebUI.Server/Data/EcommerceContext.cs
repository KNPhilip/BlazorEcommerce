﻿using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Server.Data;

/// <summary>
/// A database context instance, representing a session with the database 
/// and can be used to query and save instances of the entities.
/// </summary>
/// <remarks>
/// EcommerceContext derives from DbContext, which is 
/// a combination of the UoW and Repository patterns.
/// </remarks>
/// <param name="options">DbContextOptions of the EcommerceContext to be passed
/// on to the base class.</param>
public sealed class EcommerceContext : IdentityDbContext<ApplicationUser>
{
    public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
    {
        
    }

    public EcommerceContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BlazorEcommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;");
    }

    #region Data Seeding
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasData(SeedData.SeedProducts());

        modelBuilder.Entity<Category>()
            .HasData(SeedData.SeedCategories());

        modelBuilder.Entity<ProductVariant>()
            .HasData(SeedData.SeedProductVariants());

        modelBuilder.Entity<ProductType>()
            .HasData(SeedData.SeedProductTypes());

        modelBuilder.Entity<ProductVariant>()
            .HasKey(p => new { p.ProductId, p.ProductTypeId });

        modelBuilder.Entity<CartItem>()
            .HasKey(ci => new { ci.UserId, ci.ProductId, ci.ProductTypeId });

        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.ProductId, oi.ProductTypeId });

        base.OnModelCreating(modelBuilder);
    }
    #endregion

    #region Properties
    // The name of the properties represents what will
    // be names of the tables in the actual database.

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Image> Images { get; set; } 
    #endregion
}
