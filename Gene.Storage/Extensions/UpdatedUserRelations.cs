using Gene.Middleware.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Extensions
{
    public static class UpdatedUserRelations
    {
        public static void AddUpdatedUserRelations(this ModelBuilder builder)
        {
            builder.Entity<User>(user =>
            {
                user.HasMany(u => u.UpdatedActions)
                    .WithOne(a => a.UpdatedUser)
                    .HasForeignKey(a => a.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedAreas)
                    .WithOne(a => a.UpdatedUser)
                    .HasForeignKey(a => a.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedControllers)
                    .WithOne(c => c.UpdatedUser)
                    .HasForeignKey(c => c.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedControllerActions)
                    .WithOne(ca => ca.UpdatedUser)
                    .HasForeignKey(ca => ca.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedControllerActionRoles)
                    .WithOne(car => car.UpdatedUser)
                    .HasForeignKey(car => car.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedRoles)
                    .WithOne(r => r.UpdatedUser)
                    .HasForeignKey(r => r.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedUsers)
                    .WithOne(u => u.UpdatedUser)
                    .HasForeignKey(u => u.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedUserRoles)
                    .WithOne(ur => ur.UpdatedUser)
                    .HasForeignKey(ur => ur.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.HasMany(u => u.UpdatedNotifications)
                    .WithOne(n => n.UpdatedUser)
                    .HasForeignKey(n => n.UpdatedUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}