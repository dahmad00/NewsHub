using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tech360.Models;

namespace Tech360.Data
{
    public class Tech360Context : IdentityDbContext<ApplicationUser>
    {
        public Tech360Context(DbContextOptions<Tech360Context> options)
            : base(options)
        {
        }

        public DbSet<News> News { get; set; } // Add this line to include News in your DbContext
        public bool InitialFetchComplete { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // If you need to configure any specific settings for the News model, do it here
            // For example:
            // builder.Entity<News>().Property(n => n.Title).HasMaxLength(255);
        }
    }
}
