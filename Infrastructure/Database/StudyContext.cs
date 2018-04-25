using Core.Model;
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
    }
}