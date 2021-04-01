using Gene.Middleware.Bases;

namespace Gene.Middleware.Extensions
{
    public static class Conversion
    {
        #region Boolean

        public static string ToBooleanString(this bool value)
        {
            return value ? "Yes" : "No";
        }

        public static string ToBooleanString(this bool? value)
        {
            return value.GetValueOrDefault() ? "Yes" : "No";
        }

        public static string ToColorClass(this bool value)
        {
            return value ? "green-text text-darken-4" : "red-text text-darken-4";
        }

        public static string ToColorClass(this bool? value)
        {
            return value.GetValueOrDefault() ? "green-text text-darken-4" : "red-text text-darken-4";
        }

        #endregion

        #region Enum

        public static string ToColorClass(this Status status)
        {
            return status switch
            {
                Status.Active   => "green-text text-darken-4",
                Status.Inactive => "grey-text text-darken-4",
                Status.Deleted  => "red-text text-darken-4",
                _               => string.Empty
            };
        }

        public static string ToColorClass(this Status? status)
        {
            return status switch
            {
                Status.Active   => "green-text text-darken-4",
                Status.Inactive => "grey-text text-darken-4",
                Status.Deleted  => "red-text text-darken-4",
                _               => string.Empty
            };
        }

        #endregion
    }
}