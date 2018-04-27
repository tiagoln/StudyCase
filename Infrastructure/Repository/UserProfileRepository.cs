using System;
using System.Linq;
using Core.Interfaces.RepositoryContracts;
using Core.Model;
using Infrastructure.Database;

namespace Infrastructure.Repository
{
    public class UserProfileRepository : Repository<UserProfile, Guid>, IUserProfileRepository
    {
        public UserProfileRepository(StudyContext dbContext) : base(dbContext)
        {
        }

        public UserProfile GetByUserId(Guid userId)
        {
            return DbContext.Set<UserProfile>()
                .SingleOrDefault(profile => profile.UserId.Equals(userId));
        }
    }
}