using System;

namespace Gene.Middleware.System
{
    public class Result
    {
        public bool IsSucceeded { get; set; }
        public string LogId { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }

        public void Error(Exception exception)
        {
            IsSucceeded = false;
            ExceptionMessage = exception.Message;
            ExceptionType = exception.GetType().Name;
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
        public Pagination Pagination { get; set; }

        public void Success(T data)
        {
            IsSucceeded = true;
            Data = data;
        }

        public void Success(T data, Pagination paginationInfo)
        {
            IsSucceeded = true;
            Pagination = paginationInfo;
            Data = data;
        }
    }

    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
    }
}
