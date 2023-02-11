using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gene.Storage.IRepositories
{
    public interface IRepository<in TKey, TEntity, in TDto, in TFilter> where TEntity : Entity<TKey> where TFilter : BaseFilter
    {
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetListAsync(TFilter filter, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetListAsSelectedAsync(TFilter filter, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default);
        IQueryable<TEntity> Filter(IQueryable<TEntity> queryableSet, TFilter filter);
        Task<TEntity> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(TDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CreateSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);
        Task RollbackToSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }
}
