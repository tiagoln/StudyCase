using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IRepository<T, in TId>
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