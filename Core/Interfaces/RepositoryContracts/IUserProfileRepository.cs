using System;
using Core.Model;

namespace Core.Interfaces.RepositoryContracts
{
    public interface IUserProfileRepository : IRepository<UserProfile, Guid>
    {
        UserProfile GetByUserId(Guid userId);
    }
}