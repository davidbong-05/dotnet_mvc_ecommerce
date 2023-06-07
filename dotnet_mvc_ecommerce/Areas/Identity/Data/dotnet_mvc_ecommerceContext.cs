using dotnet_mvc_ecommerce.Areas.Identity.Data;
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
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
