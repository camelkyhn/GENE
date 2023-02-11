using AutoMapper;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Entities.Identity;

namespace Gene.Middleware.System;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Action, ActionDto>().ReverseMap();
        CreateMap<Area, AreaDto>().ReverseMap();
        CreateMap<Controller, ControllerDto>().ReverseMap();
        CreateMap<ControllerAction, ControllerActionDto>().ReverseMap();
        CreateMap<ControllerActionRole, ControllerActionRoleDto>().ReverseMap();

        CreateMap<Notification, NotificationDto>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
    }
}