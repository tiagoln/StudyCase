using Core.Model;

namespace Core.Interfaces.RepositoryContracts
{
    public interface IUserProfileRepository : IRepository<UserProfile, string>
    {
        UserProfile GetByUserId(string userId);
    }
}