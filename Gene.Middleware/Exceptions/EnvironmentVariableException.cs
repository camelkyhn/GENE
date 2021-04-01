using System;

namespace Gene.Middleware.Exceptions
{
    public class EnvironmentVariableException : Exception
    {
        public EnvironmentVariableException(string name) : base($"You must give {name} environment variable!")
        {
        }
    }
}