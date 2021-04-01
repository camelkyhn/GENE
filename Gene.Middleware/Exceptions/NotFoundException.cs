using System;

namespace Gene.Middleware.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string model) : base($"{model} model is not found!")
        {
        }
    }
}
