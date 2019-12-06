using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IService<T> where T : IOutput
    {
        IQueryable<T> GetAll();
        Task<T> GetSingleById(Guid id);
    }
}
