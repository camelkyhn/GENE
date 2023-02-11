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
using Microsoft.Extensions.Options;

namespace Gene.Storage.Initializers
{
    public class AppDatabaseInitializer
    {
        private DatabaseContext _dbContext;
        private AppConfiguration _configuration;
        private IMemoryCache _cache;

        public void Initialize(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _dbContext                   = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            _dbContext.IsHistoryDisabled = true;
            _cache                       = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            _configuration               = scope.ServiceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value;
            _dbContext.Database.Migrate();
            if (!_dbContext.Set<User>().Any())
            {
                CreateAdminWithRole();
                CreateAreas();
                CreateControllers();
                CreateActions();
                CreateControllerActions();
                CreateControllerActionRoles();
                CreateNotificationControllerActionRoles();
                CreateAccountControllerActionRoles();
                RemoveTemporaryEntitiesFromCache(_dbContext.Set<Area>().Count(), nameof(Area));
                RemoveTemporaryEntitiesFromCache(_dbContext.Set<Controller>().Count(), nameof(Controller));
                RemoveTemporaryEntitiesFromCache(_dbContext.Set<Middleware.Entities.Core.Action>().Count(), nameof(Middleware.Entities.Core.Action));
                RemoveTemporaryEntitiesFromCache(_dbContext.Set<ControllerAction>().Count(), nameof(ControllerAction));
            }
            else
            {
                LoadCacheValues();
            }
        }

        private void LoadCacheValues()
        {
            var adminUser = _dbContext.Set<User>().FirstOrDefault(u => u.Email == _configuration.AdminEmail);
            if (adminUser != null)
            {
                _cache.Set(Users.AdminKey, adminUser);
            }

            var adminRole = _dbContext.Set<Role>().FirstOrDefault(u => u.Name == Roles.AdminValue);
            if (adminRole != null)
            {
                _cache.Set(Roles.AdminKey, adminRole);
            }

            var memberRole = _dbContext.Set<Role>().FirstOrDefault(u => u.Name == Roles.MemberValue);
            if (memberRole != null)
            {
                _cache.Set(Roles.MemberKey, memberRole);
            }
        }

        private void CreateAdminWithRole()
        {
            var hasher        = new PasswordHasher<User>();
            var now           = DateTimeOffset.Now;
            var adminEmail    = _configuration.AdminEmail;
            var adminPassword = _configuration.AdminPassword;
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
            _dbContext.Add(adminUser);
            _dbContext.SaveChanges();
            _cache.Set(Users.AdminKey, adminUser);

            var adminRole = new Role
            {
                Name          = Roles.AdminValue,
                CreatedUserId = adminUser.Id,
                UpdatedUserId = adminUser.Id,
                Status        = Status.Active,
                CreatedDate   = now,
                UpdatedDate   = now
            };
            _dbContext.Add(adminRole);
            _dbContext.SaveChanges();
            _cache.Set(Roles.AdminKey, adminRole);

            var memberRole = new Role
            {
                Name          = Roles.MemberValue,
                CreatedUserId = adminUser.Id,
                UpdatedUserId = adminUser.Id,
                Status        = Status.Active,
                CreatedDate   = now,
                UpdatedDate   = now
            };
            _dbContext.Add(memberRole);
            _dbContext.SaveChanges();
            _cache.Set(Roles.MemberKey, memberRole);

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
            _dbContext.Add(adminUserRole);
            _dbContext.SaveChanges();

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
            _dbContext.Add(memberUserRole);
            _dbContext.SaveChanges();
        }

        private void CreateAreas()
        {
            var now   = DateTimeOffset.UtcNow;
            var admin = _cache.Get<User>(Users.AdminKey);
            object[] array =
            {
                new Area {/*1*/Name = Areas.Core, DisplayName     = Labels.Core, IconText     = "extension", Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Area {/*2*/Name = Areas.Identity, DisplayName = Labels.Identity, IconText = "account_box", Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            _dbContext.AddRange(array);
            _dbContext.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "Area");
        }

        private void CreateControllers()
        {
            var admin        = _cache.Get<User>(Users.AdminKey);
            var coreArea     = _cache.Get<Area>("Area.1");
            var identityArea = _cache.Get<Area>("Area.2");
            var now          = DateTimeOffset.Now;
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
            _dbContext.AddRange(array);
            _dbContext.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "Controller");
        }

        private void CreateActions()
        {
            var admin = _cache.Get<User>(Users.AdminKey);
            var now   = DateTimeOffset.Now;
            object[] array =
            {
                new Middleware.Entities.Core.Action {/*1*/ Name  = Actions.Index, Status                  = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*2*/ Name  = Actions.Detail, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*3*/ Name  = Actions.Create, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*4*/ Name  = Actions.Edit, Status                   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*5*/ Name  = Actions.Delete, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*6*/ Name  = Actions.SendGlobalNotification, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*7*/ Name  = Actions.UpdateProfile, Status          = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*8*/ Name  = Actions.Logout, Status                 = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*9*/ Name  = Actions.ChangePassword, Status         = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*10*/ Name = Actions.AccessDenied, Status           = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*11*/ Name = Actions.Notifications, Status          = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*12*/ Name = Actions.GetNotification, Status        = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new Middleware.Entities.Core.Action {/*13*/ Name = Actions.DeleteNotification, Status     = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            _dbContext.AddRange(array);
            _dbContext.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "Action");
        }

        private void CreateControllerActions()
        {
            var admin = _cache.Get<User>(Users.AdminKey);
            var now   = DateTimeOffset.Now;
            var array = new object[40];
            var index = 0;
            for (var i = 1; i < 9; i++)
            {
                for (var j = 1; j < 6; j++)
                {
                    array[index] = new ControllerAction
                    {
                        ControllerId  = _cache.Get<Controller>("Controller." + i).Id,
                        ActionId      = _cache.Get<Middleware.Entities.Core.Action>("Action." + j).Id,
                        Status        = Status.Active,
                        CreatedDate   = now,
                        UpdatedDate   = now,
                        CreatedUserId = admin.Id,
                        UpdatedUserId = admin.Id
                    };
                    index++;
                }
            }
            _dbContext.AddRange(array);
            _dbContext.SaveChanges();
            LoadTemporaryEntitiesToCache(array, "ControllerAction");
        }

        private void CreateControllerActionRoles()
        {
            var admin = _cache.Get<User>(Users.AdminKey);
            var now   = DateTimeOffset.Now;
            var array = new object[40];
            for (var i = 1; i < 41; i++)
            {
                array[i - 1] = new ControllerActionRole
                {
                    ControllerActionId = _cache.Get<ControllerAction>("ControllerAction." + i).Id,
                    RoleId             = _cache.Get<Role>(Roles.AdminKey).Id,
                    Status             = Status.Active,
                    CreatedDate        = now,
                    UpdatedDate        = now,
                    CreatedUserId      = admin.Id,
                    UpdatedUserId      = admin.Id
                };
            }
            _dbContext.AddRange(array);
            _dbContext.SaveChanges();
        }

        private void CreateNotificationControllerActionRoles()
        {
            var admin = _cache.Get<User>(Users.AdminKey);
            var now   = DateTimeOffset.Now;
            var controllerActionArray = new List<ControllerAction>
            {
                new() {ControllerId = _cache.Get<Controller>("Controller.9").Id, ActionId = /*Index*/_cache.Get<Middleware.Entities.Core.Action>("Action.1").Id, Status  = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() {ControllerId = _cache.Get<Controller>("Controller.9").Id, ActionId = /*Detail*/_cache.Get<Middleware.Entities.Core.Action>("Action.2").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() {ControllerId = _cache.Get<Controller>("Controller.9").Id, ActionId = /*Create*/_cache.Get<Middleware.Entities.Core.Action>("Action.3").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() {ControllerId = _cache.Get<Controller>("Controller.9").Id, ActionId = /*Edit*/_cache.Get<Middleware.Entities.Core.Action>("Action.4").Id, Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() {ControllerId = _cache.Get<Controller>("Controller.9").Id, ActionId = /*Delete*/_cache.Get<Middleware.Entities.Core.Action>("Action.5").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() {ControllerId = _cache.Get<Controller>("Controller.9").Id, ActionId = /*SendGlobalNotification*/_cache.Get<Middleware.Entities.Core.Action>("Action.6").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            _dbContext.AddRange(controllerActionArray);
            _dbContext.SaveChanges();
            var controllerActionRoleArray = new List<ControllerActionRole> 
            {
                new() { ControllerActionId = controllerActionArray[0].Id, RoleId = _cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[1].Id, RoleId = _cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[2].Id, RoleId = _cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[3].Id, RoleId = _cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[4].Id, RoleId = _cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[5].Id, RoleId = _cache.Get<Role>(Roles.AdminKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            _dbContext.AddRange(controllerActionRoleArray);
            _dbContext.SaveChanges();
        }

        private void CreateAccountControllerActionRoles()
        {
            var admin = _cache.Get<User>(Users.AdminKey);
            var now   = DateTimeOffset.Now;
            var controllerActionArray = new List<ControllerAction>
            {
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*Index*/_cache.Get<Middleware.Entities.Core.Action>("Action.1").Id, Status            = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*UpdateProfile*/_cache.Get<Middleware.Entities.Core.Action>("Action.7").Id, Status    = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*Logout*/_cache.Get<Middleware.Entities.Core.Action>("Action.8").Id, Status           = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*ChangePassword*/_cache.Get<Middleware.Entities.Core.Action>("Action.9").Id, Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*AccessDenied*/_cache.Get<Middleware.Entities.Core.Action>("Action.10").Id, Status    = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*Notifications*/_cache.Get<Middleware.Entities.Core.Action>("Action.11").Id, Status   = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*GetNotification*/_cache.Get<Middleware.Entities.Core.Action>("Action.12").Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerId = _cache.Get<Controller>("Controller.10").Id, ActionId = /*DeleteNotification*/_cache.Get<Middleware.Entities.Core.Action>("Action.13").Id, Status         = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            _dbContext.AddRange(controllerActionArray);
            _dbContext.SaveChanges();
            var controllerActionRoleArray = new List<ControllerActionRole>
            {
                new() { ControllerActionId = controllerActionArray[0].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[1].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[2].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[3].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[4].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[5].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[6].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id},
                new() { ControllerActionId = controllerActionArray[7].Id, RoleId = _cache.Get<Role>(Roles.MemberKey).Id, Status = Status.Active, CreatedDate = now, UpdatedDate = now, CreatedUserId = admin.Id, UpdatedUserId = admin.Id}
            };
            _dbContext.AddRange(controllerActionRoleArray);
            _dbContext.SaveChanges();
        }

        private void LoadTemporaryEntitiesToCache(IReadOnlyList<object> array, string entityName)
        {
            for (var i = 1; i < array.Count + 1; i++)
            {
                _cache.Set(entityName + "." + i, array[i - 1]);
            }
        }

        private void RemoveTemporaryEntitiesFromCache(int count, string entityName)
        {
            for (var i = 1; i < count + 1; i++)
            {
                _cache.Remove(entityName + "." + i);
            }
        }
    }
}