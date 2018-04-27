using System;
using Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class StudyContext : IdentityDbContext<User, Role, Guid>
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
            modelBuilder.Entity<Role>().ToTable("IdentityRole");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("IdentityUserClaim");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("IdentityUserRole");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("IdentityUserLogin");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("IdentityRoleClaim");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("IdentityUserToken");
            //====================================
            
            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });
        }
    }
}