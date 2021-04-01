using Gene.Middleware.Entities.Core;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Extensions
{
    public static class CoreRelations
    {
        public static void AddCoreRelations(this ModelBuilder builder)
        {
            builder.Entity<Action>(action =>
            {
                action.HasMany(a => a.ControllerActions)
                      .WithOne(a => a.Action)
                      .HasForeignKey(a => a.ActionId)
                      .IsRequired();

                action.HasIndex(a => a.Name).IsUnique();
            });

            builder.Entity<Controller>(controller =>
            {
                controller.HasMany(c => c.ControllerActions)
                          .WithOne(c => c.Controller)
                          .HasForeignKey(c => c.ControllerId)
                          .IsRequired();

                controller.HasIndex(c => c.Name).IsUnique();
            });

            builder.Entity<Area>(area =>
            {
                area.HasMany(a => a.Controllers)
                          .WithOne(c => c.Area)
                          .HasForeignKey(c => c.AreaId)
                          .IsRequired();

                area.HasIndex(a => a.Name).IsUnique();
            });

            builder.Entity<ControllerAction>(controllerAction =>
            {
                controllerAction.HasMany(ca => ca.ControllerActionRoles)
                                .WithOne(ca => ca.ControllerAction)
                                .HasForeignKey(ca => ca.ControllerActionId)
                                .IsRequired();
            });
        }

        public static void ToCoreTables(this ModelBuilder builder)
        {
            builder.Entity<Action>().ToTable(nameof(Action)).HasKey(e => e.Id);
            builder.Entity<Area>().ToTable(nameof(Area)).HasKey(e => e.Id);
            builder.Entity<Controller>().ToTable(nameof(Controller)).HasKey(e => e.Id);
            builder.Entity<ControllerAction>().ToTable(nameof(ControllerAction)).HasKey(e => e.Id);
            builder.Entity<ControllerActionRole>().ToTable(nameof(ControllerActionRole)).HasKey(e => e.Id);
        }
    }
}