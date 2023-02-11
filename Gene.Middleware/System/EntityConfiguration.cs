using System;
using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Entities.Identity;

namespace Gene.Middleware.System;

public static class EntityConfiguration
{
    public static List<Type> TrackableList => new()
    {
        typeof(Role),
        typeof(User),
        typeof(UserRole)
    };

    public static List<Type> SoftDeletableList => new()
    {
        typeof(Role),
        typeof(User),
        typeof(UserRole)
    };

    public static List<string> IgnoredProperties => new()
    {
        nameof(Entity<object>.CreatedDate),
        nameof(Entity<object>.CreatedUser),
        nameof(Entity<object>.CreatedUserId),
        nameof(Entity<object>.UpdatedUser),
        nameof(Entity<object>.UpdatedDate),
        nameof(User.PasswordHash),
        nameof(User.SecurityStamp)
    };
}