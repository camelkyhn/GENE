using Gene.Middleware.System;

namespace Gene.Middleware.Extensions
{
    public static class Result
    {
        public static bool IsSuccess<TData>(this Result<TData> result)
        {
            return result != null && result.IsSucceeded;
        }

        public static bool IsFailed<TData>(this Result<TData> result)
        {
            return result == null || !result.IsSucceeded;
        }

        public static bool IsFailedWithData<TData>(this Result<TData> result)
        {
            return result != null && !result.IsSucceeded && result.Data != null;
        }
    }
}