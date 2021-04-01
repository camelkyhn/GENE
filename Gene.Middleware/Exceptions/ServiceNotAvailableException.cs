using System;

namespace Gene.Middleware.Exceptions
{
    public class ServiceNotAvailableException : Exception
    {
        public ServiceNotAvailableException(string service) : base($"Server is under maintenance, {service} service is unavailable for now!")
        {
        }
    }
}