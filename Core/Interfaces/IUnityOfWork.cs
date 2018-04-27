using System;
using Core.Interfaces.RepositoryContracts;
using Core.Model;

namespace Core.Interfaces
{
    public interface IUnityOfWork : IDisposable
    {
        IRepository<User, Guid> UserRepository { get; }
        IUserProfileRepository UserProfileRepository { get; }
        IRepository<Order, int> OrdeRepository { get; }        
        
        void Save();
        void Commit();
        void Rollback();
    }
}