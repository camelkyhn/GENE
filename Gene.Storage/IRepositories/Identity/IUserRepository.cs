using System;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Storage.IRepositories.Identity
{
    public interface IUserRepository : IRepository<Guid?, Middleware.Entities.Identity.User, UserDto, UserFilter>
    {
        Task<Middleware.Entities.Identity.User> GetByEmailAsync(string email);
        Task IncreaseFailedAttemptsAsync(Middleware.Entities.Identity.User user);
        Task ClearFailedAttemptsAsync(Middleware.Entities.Identity.User user);
        bool IsEmailTaken(string email);
        bool IsEmailTaken(string email, Guid? userId);
    }
}
