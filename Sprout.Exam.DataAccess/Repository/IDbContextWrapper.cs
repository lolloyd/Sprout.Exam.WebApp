using Microsoft.EntityFrameworkCore;
using Sprout.Exam.DataAccess.Repository.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sprout.Exam.DataAccess.Repository
{
    public interface IDbContextWrapper
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : class;

        IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class;

        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

        Task SaveAsync();
        DbSet<Employee> Employee { get; }
    }
}
