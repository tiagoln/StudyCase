using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Database
{
    public class StudyContextFactory : IDesignTimeDbContextFactory<StudyContext>
    {
        public StudyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudyContext>();
            optionsBuilder.UseSqlServer("Server = tcp:localhost,1433; Database = StudyCase; User Id = sa; Password = Admin#123; MultipleActiveResultSets=True");

            return new StudyContext(optionsBuilder.Options);
        }
    }
}