using System;

namespace Service.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid id)
           : base($"Entity with ID {id} was not found")
        {
        }

        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ServiceInputException : Exception
    {
        public ServiceInputException()
           : base($"One or more validation errors occurred")
        {
        }

        public ServiceInputException(string message) : base(message)
        {
        }

        public ServiceInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
