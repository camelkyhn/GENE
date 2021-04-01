using System;
using System.Collections.Generic;
using System.Linq;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Gene.Middleware.Extensions
{
    public static class DatabaseInitializer
    {
        public static void Initialize<TContext>(this IApplicationBuilder app, IMemoryCache cache) where TContext : DbContext
        {
            using var scope   = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var       context = scope.ServiceProvider.GetRequiredService<TContext>();
            context.Database.Migrate();
            if (!context.Set<User>().Any())
            {
                context.CreateAdminWithRole(cache);
                context.CreateAreas(cache);
                context.CreateControllers(cache);
                context.CreateActions(cache);
                context.CreateControllerActions(cache);
                context.CreateControllerActionRoles(cache);
                context.CreateNotificationControllerActionRoles(cache);
                context.CreateAccountControllerActionRoles(cache);
                RemoveTemporaryEntitiesFromCache(context.Set<Area>().Count(), nameof(Area), cache);
                RemoveTemporaryEntitiesFromCache(context.Set<Controller>().Count(), nameof(Controller), cache);
                RemoveTemporaryEntitiesFromCache(context.Set<Entities.Core.Action>().Count(), nameof(Entities.Core.Action), cache);
                RemoveTemporaryEntitiesFromCache(context.Set<ControllerAction>().Count(), nameof(ControllerAction), cache);
            }
            else
            {
                context.LoadCacheValues(cache);
            }
        }

        private static void LoadCacheValues<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var adminUser = context.Set<User>().FirstOrDefault(u => u.Email == EnvironmentVariable.AdminEmail);
            if (adminUser != null)
            {
                cache.Set(Users.AdminKey, adminUser);
            }

            var adminRole = context.Set<Role>().FirstOrDefault(u => u.Name == Roles.AdminValue);
            if (adminRole != null)
            {
                cache.Set(Roles.AdminKey, adminRole);
            }

            var memberRole = context.Set<Role>().FirstOrDefault(u => u.Name == Roles.MemberValue);
            if (memberRole != null)
            {
                cache.Set(Roles.MemberKey, memberRole);
            }
        }

        private static void CreateAdminWithRole<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var hasher        = new PasswordHasher<User>();
            var now           = DateTime.Now;
            var adminEmail    = EnvironmentVariable.AdminEmail;
            var adminPassword = EnvironmentVariable.AdminPassword;
            var adminUser = new User
            {
                PhoneNumber            = "5555555555",
                Email                  = adminEmail,
                IsEmailConfirmed       = true,
                IsPhoneNumberConfirmed = true,
                IsEmailEnabled         = true,
                IsSmsEnabled           = false,
                IsLockoutEnabled       = false,
                SecurityStamp          = Guid.NewGuid().ToString(),
                Status                 = Status.Active,
                CreatedDate            = now,
                UpdatedDate            = now
            };
            adminUser.CreatedUser  = adminUser;
            adminUser.UpdatedUser  = adminUser;
            adminUser.PasswordHash = hasher.HashPassword(adminUser, adminPassword);
            context.Add(adminUser);
            context.SaveChanges();
            cache.Set(Users.AdminKey, adminUser);

            var adminRole = new Role
            {
                Name          = Roles.AdminValue,
                CreatedUserId = adminUser.Id,
                UpdatedUserId = adminUser.Id,
                Status        = Status.Active,
                CreatedDate   = now,
                UpdatedDate   = now
            };
            context.Add(adminRole);
            context.SaveChanges();
            cache.Set(Roles.AdminKey, adminRole);

            var memberRole = new Role
            {
                Name          = Roles.MemberValue,
                CreatedUserId = adminUser.Id,
                UpdatedUserId = adminUser.Id,
                Status        = Status.Active,
                CreatedDate   = now,
                UpdatedDate   = now
            };
            context.Add(memberRole);
            context.SaveChanges();
            cache.Set(Roles.MemberKey, memberRole);

            var adminUserRole = new UserRole
            {
                UserId        = adminUser.Id,
                RoleId        = adminRole.Id,
                CreatedUserId = adminUser.Id,
                UpdatedUserId = adminUser.Id,
                Status        = Status.Active,
                CreatedDate   = now,
                UpdatedDate   = now
            };
            context.Add(adminUserRole);
            context.SaveChanges();

            var memberUserRole = new UserRole
            {
                UserId        = adminUser.Id,
                RoleId        = memberRole.Id,
                CreatedUserId = adminUser.Id,
                UpdatedUserId = adminUser.Id,
                Status        = Status.Active,
                CreatedDate   = now,
                UpdatedDate   = now
            };
            context.Add(memberUserRole);
            context.SaveChanges();
        }

        private static void CreateAreas<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var now   = DateTimeOffset.UtcNow;
            var admin = cache.Get<User>(Users.AdminKey);
            object[] array =
            {
                new Area {/*1*/Name = Areas.Core, DisplayName     = Labels.Core, IconText     = "extension", Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Area {/*2*/Name = Areas.Identity, DisplayName = Labels.Identity, IconText = "account_box", Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            context.AddRange(array);
            context.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "Area", cache);
        }

        private static void CreateControllers<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var admin        = cache.Get<User>(Users.AdminKey);
            var coreArea     = cache.Get<Area>("Area.1");
            var identityArea = cache.Get<Area>("Area.2");
            var now          = DateTime.Now;
            object[] array =
            {
                new Controller {/*1*/ Name  = Controllers.Action, DisplayName               = Labels.Action, IconText               = "call_to_action", AreaId          = coreArea.Id, Status     = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*2*/ Name  = Controllers.Area, DisplayName                 = Labels.Area, IconText                 = "place", AreaId                   = coreArea.Id, Status     = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*3*/ Name  = Controllers.Controller, DisplayName           = Labels.Controller, IconText           = "control_point", AreaId           = coreArea.Id, Status     = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*4*/ Name  = Controllers.ControllerAction, DisplayName     = Labels.ControllerAction, IconText     = "control_point_duplicate", AreaId = coreArea.Id, Status     = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*5*/ Name  = Controllers.ControllerActionRole, DisplayName = Labels.ControllerActionRole, IconText = "accessibility", AreaId           = coreArea.Id, Status     = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*6*/ Name  = Controllers.Role, DisplayName                 = Labels.Role, IconText                 = "perm_identity", AreaId           = identityArea.Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*7*/ Name  = Controllers.User, DisplayName                 = Labels.User, IconText                 = "person", AreaId                  = identityArea.Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*8*/ Name  = Controllers.UserRole, DisplayName             = Labels.UserRole, IconText             = "person_pin", AreaId              = identityArea.Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*9*/ Name  = Controllers.Notification, DisplayName         = Labels.Notification, IconText         = "notifications", AreaId           = identityArea.Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Controller {/*10*/ Name = Controllers.Account, DisplayName              = Labels.Account, IconText              = "account_circle", AreaId          = identityArea.Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            context.AddRange(array);
            context.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "Controller", cache);
        }

        private static void CreateActions<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var admin = cache.Get<User>(Users.AdminKey);
            var now   = DateTime.Now;
            object[] array =
            {
                new Entities.Core.Action {/*1*/ Name  = Actions.Index, Status                  = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*2*/ Name  = Actions.Detail, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*3*/ Name  = Actions.Create, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*4*/ Name  = Actions.Edit, Status                   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*5*/ Name  = Actions.Delete, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*6*/ Name  = Actions.SendGlobalNotification, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*7*/ Name  = Actions.UpdateProfile, Status          = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*8*/ Name  = Actions.Logout, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*9*/ Name  = Actions.ChangePassword, Status         = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*10*/ Name = Actions.AccessDenied, Status           = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*11*/ Name = Actions.Notifications, Status          = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*12*/ Name = Actions.GetNotification, Status        = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Entities.Core.Action {/*13*/ Name = Actions.DeleteNotification, Status     = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            context.AddRange(array);
            context.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "Action", cache);
        }

        private static void CreateControllerActions<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var admin = cache.Get<User>(Users.AdminKey);
            var now   = DateTime.Now;
            var array = new object[40];
            var index = 0;
            for (var i = 1; i < 9; i++)
            {
                for (var j = 1; j < 6; j++)
                {
                    array[index] = new ControllerAction
                    {
                        ControllerId  = cache.Get<Controller>("Controller." + i).Id,
                        ActionId      = cache.Get<Entities.Core.Action>("Action." + j).Id,
                        Status        = Status.Active,
                        CreatedDate   = now,
                        UpdatedDate   = now,
                        CreatedUserId = admin.Id,
                        UpdatedUserId = admin.Id
                    };
                    index++;
                }
            }
            context.AddRange(array);
            context.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "ControllerAction", cache);
        }

        private static void CreateControllerActionRoles<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var admin = cache.Get<User>(Users.AdminKey);
            var now   = DateTime.Now;
            var array = new object[40];
            for (var i = 1; i < 41; i++)
            {
                array[i - 1] = new ControllerActionRole
                {
                    ControllerActionId = cache.Get<ControllerAction>("ControllerAction." + i).Id,
                    RoleId             = cache.Get<Role>(Roles.AdminKey).Id,
                    Status             = Status.Active,
                    CreatedDate        = now,
                    UpdatedDate        = now,
                    CreatedUserId      = admin.Id,
                    UpdatedUserId      = admin.Id
                };
            }
            context.AddRange(array);
            context.SaveChanges();
        }

        private static void CreateNotificationControllerActionRoles<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var admin = cache.Get<User>(Users.AdminKey);
            var now   = DateTime.Now;
            var controllerActionArray = new List<ControllerAction>
            {
                new ControllerAction {ControllerId = cache.Get<Controller>("Controller.9").Id, ActionId = /*Index*/cache.Get<Entities.Core.Action>("Action.1").Id, Status  = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction {ControllerId = cache.Get<Controller>("Controller.9").Id, ActionId = /*Detail*/cache.Get<Entities.Core.Action>("Action.2").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction {ControllerId = cache.Get<Controller>("Controller.9").Id, ActionId = /*Create*/cache.Get<Entities.Core.Action>("Action.3").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction {ControllerId = cache.Get<Controller>("Controller.9").Id, ActionId = /*Edit*/cache.Get<Entities.Core.Action>("Action.4").Id, Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction {ControllerId = cache.Get<Controller>("Controller.9").Id, ActionId = /*Delete*/cache.Get<Entities.Core.Action>("Action.5").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction {ControllerId = cache.Get<Controller>("Controller.9").Id, ActionId = /*SendGlobalNotification*/cache.Get<Entities.Core.Action>("Action.6").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            context.AddRange(controllerActionArray);
            context.SaveChanges();
            var controllerActionRoleArray = new List<ControllerActionRole> 
            {
                new ControllerActionRole { ControllerActionId = controllerActionArray[0].Id, RoleId = cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[1].Id, RoleId = cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[2].Id, RoleId = cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[3].Id, RoleId = cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[4].Id, RoleId = cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[5].Id, RoleId = cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            context.AddRange(controllerActionRoleArray);
            context.SaveChanges();
        }

        private static void CreateAccountControllerActionRoles<TContext>(this TContext context, IMemoryCache cache) where TContext : DbContext
        {
            var admin = cache.Get<User>(Users.AdminKey);
            var now   = DateTime.Now;
            var controllerActionArray = new List<ControllerAction>
            {
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*Index*/cache.Get<Entities.Core.Action>("Action.1").Id, Status            = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*UpdateProfile*/cache.Get<Entities.Core.Action>("Action.7").Id, Status    = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*Logout*/cache.Get<Entities.Core.Action>("Action.8").Id, Status           = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*ChangePassword*/cache.Get<Entities.Core.Action>("Action.9").Id, Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*AccessDenied*/cache.Get<Entities.Core.Action>("Action.10").Id, Status    = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*Notifications*/cache.Get<Entities.Core.Action>("Action.11").Id, Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*GetNotification*/cache.Get<Entities.Core.Action>("Action.12").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerAction { ControllerId = cache.Get<Controller>("Controller.10").Id, ActionId = /*DeleteNotification*/cache.Get<Entities.Core.Action>("Action.13").Id, Status            = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            context.AddRange(controllerActionArray);
            context.SaveChanges();
            var controllerActionRoleArray = new List<ControllerActionRole>
            {
                new ControllerActionRole { ControllerActionId = controllerActionArray[0].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[1].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[2].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[3].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[4].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[5].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[6].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new ControllerActionRole { ControllerActionId = controllerActionArray[7].Id, RoleId = cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            context.AddRange(controllerActionRoleArray);
            context.SaveChanges();
        }

        private static void LoadTemporaryEntitiesToCache(IReadOnlyList<object> array, string entityName, IMemoryCache cache)
        {
            for (var i = 1; i < array.Count + 1; i++)
            {
                cache.Set(entityName + "." + i, array[i - 1]);
            }
        }

        private static void RemoveTemporaryEntitiesFromCache(int count, string entityName, IMemoryCache cache)
        {
            for (var i = 1; i < count + 1; i++)
            {
                cache.Remove(entityName + "." + i);
            }
        }
    }
}