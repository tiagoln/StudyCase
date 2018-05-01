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

        public IRepository<User, Guid> UserRepository => new UserRepository(_context);
        public IUserProfileRepository UserProfileRepository => new UserProfileRepository(_context);
        public IRepository<Order, int> OrdeRepository => new Repository<Order, int>(_context);

        public void Save()
        {
//            try
//            {
            _context.SaveChanges();
//            }
//            catch (DbUpdateConcurrencyException ex)
//            {
//                foreach (var entry in ex.Entries)
//                {
//                    var proposedValues = entry.CurrentValues;
//                    var databaseValues = entry.GetDatabaseValues();
//
//                    foreach (var property in proposedValues.Properties)
//                    {
//                        var proposedValue = proposedValues[property];
//                        var databaseValue = databaseValues[property];
//
//                        // TODO: decide which value should be written to database
//                        // proposedValues[property] = <value to be saved>;
//                    }
//
//                    // Refresh original values to bypass next concurrency check
//                    entry.OriginalValues.SetValues(databaseValues);
//
//                    throw new NotSupportedException(
//                        "Don't know how to handle concurrency conflicts for "
//                        + entry.Metadata.Name);
//                }
//            }
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