using System;

namespace Gene.Middleware.Exceptions
{
    public class NotPossessedException : Exception
    {
        public NotPossessedException(string model) : base($"This {model} is not possessed by you!")
        {
        }
    }
}