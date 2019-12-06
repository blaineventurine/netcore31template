using System;
using System.Linq;
using System.Threading.Tasks;
using Service.Services.Outputs;

namespace Service.Interfaces
{
    public interface ISimpleService : IService<SimpleEntityOutput>
    {
        Task<SimpleEntityOutput> AddNewSimpleEntity(string name);
        Task<SimpleEntityOutput> UpdateSimpleEntity(Guid id, string name);
        IQueryable<SimpleEntityOutput> DemonstrateExtensionMethod(string name, string otherName);
    }
}
