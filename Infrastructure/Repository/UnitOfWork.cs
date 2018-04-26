using System;
using Core.Interfaces;
using Core.Interfaces.RepositoryContracts;
using Core.Model;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repository
{
    public class UnitOfWork : IUnityOfWork
    {
        private readonly StudyContext _context;
        private readonly IDbContextTransaction _transaction;

        public UnitOfWork(StudyContext context)
        {
            _context = context;
            _transaction = context.Database.BeginTransaction();
        }

        public IRepository<User, string> UserRepository => new UserRepository(_context);
        public IUserProfileRepository UserProfileRepository => new UserProfileRepository(_context);
        public IRepository<Order, int> OrdeRepository => new Repository<Order, int>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}