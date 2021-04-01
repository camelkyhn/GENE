using System;
using System.Linq;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Exceptions;
using Gene.Middleware.Filters.Identity;
using Gene.Storage.IRepositories.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Repositories.Identity
{
    public class UserRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Identity.User, UserDto, UserFilter>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Identity.User> Filter(IQueryable<Middleware.Entities.Identity.User> queryableSet, UserFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Email))
            {
                queryableSet = queryableSet.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            if (filter.IsEmailConfirmed != null)
            {
                queryableSet = queryableSet.Where(u => u.IsEmailConfirmed == filter.IsEmailConfirmed);
            }

            if (filter.IsEmailEnabled != null)
            {
                queryableSet = queryableSet.Where(u => u.IsEmailEnabled == filter.IsEmailEnabled);
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public async Task<Middleware.Entities.Identity.User> GetByEmailAsync(string email)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new NotFoundException(nameof(Middleware.Entities.Identity.User));
            }

            return user;
        }

        public override async Task<Middleware.Entities.Identity.User> CreateAsync(UserDto dto)
        {
            dto.SecurityStamp = Guid.NewGuid().ToString();
            return await base.CreateAsync(dto);
        }

        public override async Task<Middleware.Entities.Identity.User> UpdateAsync(UserDto dto)
        {
            dto.SecurityStamp = Guid.NewGuid().ToString();
            return await base.UpdateAsync(dto);
        }

        public async Task IncreaseFailedAttemptsAsync(Middleware.Entities.Identity.User user)
        {
            user.UpdatedDate = DateTimeOffset.UtcNow;
            user.AccessFailedCount++;
            Context.Entry(user).Property(u => u.UpdatedDate).IsModified       = true;
            Context.Entry(user).Property(u => u.AccessFailedCount).IsModified = true;
            if (user.AccessFailedCount >= 5)
            {
                user.LockoutEnd                                            = DateTimeOffset.UtcNow.AddMinutes(5 * (user.AccessFailedCount - 4));
                Context.Entry(user).Property(u => u.LockoutEnd).IsModified = true;
            }

            await Context.SaveChangesAsync();
        }

        public async Task ClearFailedAttemptsAsync(Middleware.Entities.Identity.User user)
        {
            user.UpdatedDate                                                  = DateTimeOffset.UtcNow;
            user.AccessFailedCount                                            = 0;
            user.LockoutEnd                                                   = null;
            Context.Entry(user).Property(u => u.UpdatedDate).IsModified       = true;
            Context.Entry(user).Property(u => u.AccessFailedCount).IsModified = true;
            Context.Entry(user).Property(u => u.LockoutEnd).IsModified        = true;
            await Context.SaveChangesAsync();
        }

        public bool IsEmailTaken(string email)
        {
            return Context.Users.Any(u => u.Email.ToLower() == email.ToLower());
        }

        public bool IsEmailTaken(string email, Guid? userId)
        {
            return Context.Users.Any(u => u.Id != userId && u.Email.ToLower() == email.ToLower());
        }
    }
}