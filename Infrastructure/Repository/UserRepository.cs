using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Interfaces;
using Core.Model;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly StudyContext _dbContext;

        public UserRepository(StudyContext context)
        {
            _dbContext = context;
        }

        public User GetById(Guid id)
        {
            return _dbContext.Users.Find(id);
        }

        public virtual IEnumerable<User> List()
        {
            return _dbContext.Users.AsEnumerable();
        }

        public virtual IEnumerable<User> List(Expression<Func<User, bool>> predicate)
        {
            return _dbContext.Users
                .Where(predicate)
                .AsEnumerable();
        }

        public IEnumerable<User> List(ISpecification<User> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryWithExpressionIncludes = spec.Includes
                .Aggregate(_dbContext.Users.AsQueryable(),
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

        public void Add(User entity)
        {
            _dbContext.Users.Add(entity);
        }

        public void Update(User entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(User entity)
        {
            _dbContext.Users.Remove(entity);
        }
    }
}