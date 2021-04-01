using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gene.Middleware.Extensions
{
    public static class Property
    {
        public static object GetPropertyValue(this object entity, string path)
        {
            var type = entity.GetType();
            foreach (var propertyName in path.Split("."))
            {
                var propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    return null;
                }

                entity = propertyInfo.GetValue(entity, null);
                if (entity == null)
                {
                    return null;
                }

                type = entity.GetType();
            }

            return entity;
        }

        public static string GetCombinedPropertyValues(this object entity, string combinedPath)
        {
            var value        = string.Empty;
            var pathElements = combinedPath.Split("+");
            foreach (var path in pathElements)
            {
                value += GetPropertyValue(entity, path) + " ";
            }

            return value.TrimEnd();
        }

        public static T MapTo<T>(this object source, T target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();
            var sourceProperties = sourceType.GetProperties();
            var targetProperties = targetType.GetProperties();
            var commonProperties = new List<Mapper>();
            foreach (var sp in sourceProperties)
            {
                var targetProperty = targetProperties.FirstOrDefault(tp => tp.Name == sp.Name && tp.PropertyType == sp.PropertyType);
                if (targetProperty != null)
                {
                    commonProperties.Add(new Mapper { SourceProperty = sp, TargetProperty = targetProperty });
                }
            }

            foreach (var property in commonProperties)
            {
                property.TargetProperty.SetValue(target, property.SourceProperty.GetValue(source, null), null);
            }

            return target;
        }
    }

    internal class Mapper
    {
        public PropertyInfo SourceProperty { get; set; }
        public PropertyInfo TargetProperty { get; set; }
    }
}
