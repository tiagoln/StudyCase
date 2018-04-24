using Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class StudyContext : DbContext
    {
        public StudyContext(DbContextOptions<StudyContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}