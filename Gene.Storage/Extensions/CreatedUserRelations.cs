using Gene.Middleware.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Extensions
{
    public static class CreatedUserRelations
    {
        public static void AddCreatedUserRelations(this ModelBuilder builder)
        {
            builder.Entity<User>(user =>
            {
                user.HasMany(u => u.CreatedActions)
                    .WithOne(a => a.CreatedUser)
                    .HasForeignKey(a => a.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedAreas)
                    .WithOne(a => a.CreatedUser)
                    .HasForeignKey(a => a.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedControllers)
                    .WithOne(c => c.CreatedUser)
                    .HasForeignKey(c => c.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedControllerActions)
                    .WithOne(ca => ca.CreatedUser)
                    .HasForeignKey(ca => ca.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedControllerActionRoles)
                    .WithOne(car => car.CreatedUser)
                    .HasForeignKey(car => car.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedRoles)
                    .WithOne(r => r.CreatedUser)
                    .HasForeignKey(r => r.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedUsers)
                    .WithOne(u => u.CreatedUser)
                    .HasForeignKey(u => u.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedUserRoles)
                    .WithOne(ur => ur.CreatedUser)
                    .HasForeignKey(ur => ur.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.CreatedNotifications)
                    .WithOne(n => n.CreatedUser)
                    .HasForeignKey(n => n.CreatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}