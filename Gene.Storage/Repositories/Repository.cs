using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Gene.Middleware.Exceptions;
using Gene.Middleware.Extensions;
using Gene.Middleware.System;
using Gene.Storage.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gene.Storage.Repositories
{
    public class Repository<TContext, TKey, TEntity, TDto, TFilter> : IRepository<TKey, TEntity, TDto, TFilter> where TContext : DbContext where TEntity : Entity<TKey>, new() where TDto : BaseDto<TKey> where TFilter : BaseFilter
    {
        protected readonly TContext Context;

        protected Repository(TContext context)
        {
            Context = context;
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(id, cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetListAsync(TFilter filter, CancellationToken cancellationToken = default)
        {
            return await Filter(Context.Set<TEntity>(), filter).ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<TEntity>().Select(selectExpression).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            return entity;
        }

        public virtual async Task<List<TEntity>> GetListAsSelectedAsync(TFilter filter, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default)
        {
            return await Filter(Context.Set<TEntity>().Select(selectExpression), filter).ToListAsync(cancellationToken: cancellationToken);
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

        public virtual async Task<TEntity> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.MapTo(new TEntity());
            entity.CreatedUserId = dto.UpdatedUserId;
            entity.CreatedDate   = DateTimeOffset.UtcNow;
            entity.UpdatedDate   = DateTimeOffset.UtcNow;
            await Context.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TDto dto, CancellationToken cancellationToken = default)
        {
            var oldEntity = await FindAsync(dto.Id, cancellationToken);
            Context.Attach(oldEntity);
            oldEntity             = dto.MapTo(oldEntity);
            oldEntity.UpdatedDate = DateTimeOffset.UtcNow;
            await Context.SaveChangesAsync(cancellationToken);
            return oldEntity;
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken);
            Delete(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }

        private void Delete(TEntity entity)
        {
            if (EntityConfiguration.SoftDeletableList.Contains(entity.GetType()))
            {
                Context.Attach(entity);
                entity.Status      = Status.Deleted;
                entity.UpdatedDate = DateTimeOffset.UtcNow;
            }
            else
            {
                Context.Remove(entity);
            }
        }

        private async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            return entity;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CreateSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default)
        {
            await transaction.CreateSavepointAsync(pointName, cancellationToken);
        }

        public async Task RollbackToSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default)
        {
            await transaction.RollbackToSavepointAsync(pointName, cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await Context.Database.CommitTransactionAsync(cancellationToken);
        }
    }
}