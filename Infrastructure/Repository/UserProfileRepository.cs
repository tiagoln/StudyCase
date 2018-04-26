using System.Linq;
using Core.Interfaces.RepositoryContracts;
using Core.Model;
using Infrastructure.Database;

namespace Infrastructure.Repository
{
    public class UserProfileRepository : Repository<UserProfile, string>, IUserProfileRepository
    {
        public UserProfileRepository(StudyContext dbContext) : base(dbContext)
        {
        }

        public UserProfile GetByUserId(string userId)
        {
            return DbContext.Set<UserProfile>()
                .SingleOrDefault(profile => profile.UserId.Equals(userId));
        }
    }
}