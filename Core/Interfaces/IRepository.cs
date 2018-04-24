using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Model;

namespace Core.Interfaces
{
    public interface IRepository<T, in TId> where T : EntityBase<TId>
    {
        T GetById(TId id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        IEnumerable<T> List(ISpecification<T> spec);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}