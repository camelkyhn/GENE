using System;

namespace Gene.Middleware.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string operation) : base($"An error occured while processing the {operation}!")
        {
        }

        public OperationFailedException(string operation, string section) : base($"An error occured while processing the {operation} in the {section}!")
        {
        }
    }
}