using Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class StudyContext : IdentityDbContext<User>
    {
        public StudyContext(DbContextOptions<StudyContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add your customizations after calling base.OnModelCreating(modelBuilder);

            // ==== Custom Identy table names ====
            modelBuilder.Entity<User>().ToTable("IdentityUser");
            modelBuilder.Entity<IdentityRole>().ToTable("IdentityRole");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("IdentityUserClaim");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("IdentityUserRole");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("IdentityUserLogin");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("IdentityRoleClaim");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("IdentityUserToken");
            //====================================
        }
    }
}