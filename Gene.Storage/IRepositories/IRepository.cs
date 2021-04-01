using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gene.Middleware.Bases;

namespace Gene.Storage.IRepositories
{
    public interface IRepository<in TKey, TEntity, in TDto, in TFilter> where TEntity : Entity<TKey> where TFilter : BaseFilter
    {
        Task<TEntity> GetAsync(TKey id);
        Task<List<TEntity>> GetListAsync(TFilter filter);
        Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression);
        Task<List<TEntity>> GetListAsSelectedAsync(TFilter filter, Expression<Func<TEntity, TEntity>> selectExpression);
        IQueryable<TEntity> Filter(IQueryable<TEntity> queryableSet, TFilter filter);
        Task<TEntity> CreateAsync(TDto dto);
        Task<TEntity> UpdateAsync(TDto dto);
        Task DeleteAsync(TKey id);
    }
}
