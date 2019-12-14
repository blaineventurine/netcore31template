using System;
using System.Linq;
using Dawn;
using Domain.Interfaces;
using Service.Exceptions;

namespace Service.Extensions
{
    public static class GuardExtensions
    {
        public static ref readonly Guard.ArgumentInfo<T> EntityExists<T>(in this Guard.ArgumentInfo<T> argument, Guid id) where T : class, IEntity
        {
            if (!argument.HasValue())
            {
                throw new NotFoundException(id);
            }

            return ref argument;
        }
    }
}
