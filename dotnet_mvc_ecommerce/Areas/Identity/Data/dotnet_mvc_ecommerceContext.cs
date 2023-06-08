using dotnet_mvc_ecommerce.Areas.Identity.Data;
using dotnet_mvc_ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnet_mvc_ecommerce.Data;

public class dotnet_mvc_ecommerceContext : IdentityDbContext<User>
{
    public dotnet_mvc_ecommerceContext(DbContextOptions<dotnet_mvc_ecommerceContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //order_product
        builder.Entity<Order_Product>().HasKey(am => new { am.OrderId, am.ProductId });

        builder.Entity<Order_Product>().HasOne(m => m.Product).WithMany(am => am.Order_Products).HasForeignKey(m => m.ProductId);
        builder.Entity<Order_Product>().HasOne(m => m.Order).WithMany(am => am.Order_Products).HasForeignKey(m => m.OrderId);


        // shoppingbasket_product
        builder.Entity<ShoppingBasket_Product>().HasKey(am => new { am.ShoppingBasketId, am.ProductId });

        builder.Entity<ShoppingBasket_Product>().HasOne(m => m.Product).WithMany(am => am.ShoppingBasket_Products).HasForeignKey(m => m.ProductId);
        builder.Entity<ShoppingBasket_Product>().HasOne(m => m.ShoppingBasket).WithMany(am => am.ShoppingBasket_Products).HasForeignKey(m => m.ShoppingBasketId);

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<CustomerDetail> CustomerDetail { get; set; }

    public DbSet<Order> Order { get; set; }

    public DbSet<Order_Product> Order_Product { get; set; }

    public DbSet<Product> Product { get; set; }

    public DbSet<ShoppingBasket> ShoppingBasket { get; set; }

    public DbSet<ShoppingBasket_Product> ShoppingBasket_Product { get; set; }

}
