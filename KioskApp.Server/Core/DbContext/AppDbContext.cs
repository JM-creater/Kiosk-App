using KioskApp.Model.Entities;
using KioskApp.Server.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KioskApp.Server.Core.DbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}
