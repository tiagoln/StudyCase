using System;
using Core.Model;

namespace Core.Interfaces
{
    public interface IUnityOfWork : IDisposable
    {
        IRepository<User, string> UserRepository { get; }
        IRepository<UserProfile, string> UserProfileRepository { get; }
        IRepository<Order, int> OrdeRepository { get; }        
        
        void Save();
        void Commit();
        void Rollback();
    }
}