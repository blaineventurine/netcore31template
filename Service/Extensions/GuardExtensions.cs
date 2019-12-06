using Dawn;
using Domain.Interfaces;
using Service.Exceptions;

namespace Service.Extensions
{
    public static class GuardExtensions
    {
        public static ref readonly Guard.ArgumentInfo<T> EntityExists<T>(
            in this Guard.ArgumentInfo<T> argument, string? message = null)
            where T : class, IEntity
        {
            if (!argument.HasValue())
            {
                throw new NotFoundException();
            }

            return ref argument;
        }
    }
}
