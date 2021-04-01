using Gene.Middleware.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Extensions
{
    public static class IdentityRelations
    {
        public static void AddIdentityRelations(this ModelBuilder builder)
        {
            builder.Entity<User>(user =>
            {
                user.HasMany(u => u.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired();

                user.HasMany(u => u.Notifications)
                    .WithOne(u => u.ReceiverUser)
                    .HasForeignKey(u => u.ReceiverUserId)
                    .IsRequired();

                user.HasIndex(u => u.Email).IsUnique();
            });

            builder.Entity<Role>(role =>
            {
                role.HasMany(r => r.UserRoles)
                    .WithOne(r => r.Role)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();

                role.HasMany(r => r.ControllerActionRoles)
                    .WithOne(r => r.Role)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();

                role.HasIndex(r => r.Name).IsUnique();
            });
        }

        public static void ToIdentityTables(this ModelBuilder builder)
        {
            builder.Entity<User>().ToTable(nameof(User)).HasKey(e => e.Id);
            builder.Entity<Role>().ToTable(nameof(Role)).HasKey(e => e.Id);
            builder.Entity<UserRole>().ToTable(nameof(UserRole)).HasKey(e => e.Id);
        }
    }
}