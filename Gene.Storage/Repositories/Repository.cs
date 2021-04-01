using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Gene.Middleware.Exceptions;
using Gene.Middleware.Extensions;
using Gene.Storage.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Repositories
{
    public class Repository<TContext, TKey, TEntity, TDto, TFilter> : IRepository<TKey, TEntity, TDto, TFilter> where TContext : DbContext where TEntity : Entity<TKey>, new() where TDto : BaseDto<TKey> where TFilter : BaseFilter
    {
        protected readonly TContext Context;

        protected Repository(TContext context)
        {
            Context = context;
        }

        public virtual async Task<TEntity> GetAsync(TKey id)
        {
            var entity = await Context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            return entity;
        }

        public virtual async Task<List<TEntity>> GetListAsync(TFilter filter)
        {
            return await Filter(Context.Set<TEntity>(), filter).ToListAsync();
        }

        public virtual async Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression)
        {
            var entity = await Context.Set<TEntity>().Select(selectExpression).FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            return entity;
        }

        public virtual async Task<List<TEntity>> GetListAsSelectedAsync(TFilter filter, Expression<Func<TEntity, TEntity>> selectExpression)
        {
            return await Filter(Context.Set<TEntity>().Select(selectExpression), filter).ToListAsync();
        }

        public virtual async Task<TEntity> CreateAsync(TDto dto)
        {
            var entity = dto.MapTo(new TEntity());
            entity.CreatedUserId = dto.UpdatedUserId;
            entity.CreatedDate = DateTimeOffset.UtcNow;
            entity.UpdatedDate = DateTimeOffset.UtcNow;
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TDto dto)
        {
            var oldEntity = await Context.Set<TEntity>().FindAsync(dto.Id);
            if (oldEntity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            Context.Attach(oldEntity);
            oldEntity = dto.MapTo(oldEntity);
            oldEntity.UpdatedDate = DateTimeOffset.UtcNow;
            await Context.SaveChangesAsync();
            return oldEntity;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await Context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual IQueryable<TEntity> Filter(IQueryable<TEntity> queryableSet, TFilter filter)
        {
            if (filter.DateBefore != null && filter.DateBefore != DateTimeOffset.MinValue)
            {
                queryableSet = queryableSet.Where(entity => entity.UpdatedDate.Value.Date < filter.DateBefore.Value.Date);
            }

            if (filter.DateAfter != null && filter.DateAfter != DateTimeOffset.MinValue)
            {
                queryableSet = queryableSet.Where(entity => entity.UpdatedDate.Value.Date > filter.DateAfter.Value.Date);
            }

            if (!string.IsNullOrEmpty(filter.UpdatedUserEmail))
            {
                queryableSet = queryableSet.Where(entity => entity.UpdatedUser.Email.ToLower().Contains(filter.UpdatedUserEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.CreatedUserEmail))
            {
                queryableSet = queryableSet.Where(entity => entity.CreatedUser.Email.ToLower().Contains(filter.CreatedUserEmail.ToLower()));
            }

            if (filter.Status != null)
            {
                queryableSet = queryableSet.Where(entity => entity.Status == filter.Status);
            }

            if (filter.IsRecentItems)
            {
                queryableSet = queryableSet.OrderByDescending(entity => entity.UpdatedDate);
            }

            filter.TotalCount = queryableSet.Count();
            if (filter.IsAllData)
            {
                return queryableSet;
            }

            if (filter.PageSize > 0 && filter.PageNumber > 0)
            {
                queryableSet = queryableSet.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            }

            return queryableSet;
        }
    }
}
