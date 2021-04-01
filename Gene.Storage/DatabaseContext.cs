using Gene.Middleware.Entities.Core;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.System;
using Gene.Storage.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(EnvironmentVariable.DbConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region To Table

            builder.ToCoreTables();

            builder.ToIdentityTables();

            #endregion

            #region Relations

            builder.AddCoreRelations();

            builder.AddIdentityRelations();

            builder.AddCreatedUserRelations();

            builder.AddUpdatedUserRelations();

            #endregion
        }

        public DbSet<Action> Actions { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<ControllerAction> ControllerActions { get; set; }
        public DbSet<ControllerActionRole> ControllerActionRoles { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
