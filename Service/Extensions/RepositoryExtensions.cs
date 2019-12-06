using System.Linq;
using Domain.Models;
using Service.Interfaces;

namespace Service.Extensions
{
    public static class OtherRepositoryExtensions
    {
    }

    public static class SimpleEntityRepositoryExtensions
    {
        // Trivial example meant to show that we use extension methods on the generic repository instead of implementing individual repos for one-off situations
        public static IQueryable<SimpleEntity> GetSimpleEntitiesByName(this IRepository<SimpleEntity> repository,
            string name, string otherName)
        {
            return repository.GetAllWhere(x => x.Name == name && x.Name == otherName);
        }
    }
}
