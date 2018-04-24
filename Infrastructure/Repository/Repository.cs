using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Model;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class Repository<T, TId> : IRepository<T, TId> where T : EntityBase<TId>
    {
        private readonly StudyContext _dbContext;

        public Repository(StudyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById(TId id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public virtual IEnumerable<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                .Where(predicate)
                .AsEnumerable();
        }

        public IEnumerable<T> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryWithExpressionIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));
 
            // modify the IQueryable to include any string-based include statements
            var queryWithStringIncludes = spec.IncludeStrings
                .Aggregate(queryWithExpressionIncludes,
                    (current, include) => current.Include(include));
            
            // modify the IQueryable to include any criteria statements
            var queryWithCriteria = spec.Criteria
                .Aggregate(queryWithStringIncludes,
                    (current, criteria) => current.Where(criteria));
 
            // return the result of the query using the specification's criteria expressions
            return queryWithCriteria
                .AsEnumerable();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}