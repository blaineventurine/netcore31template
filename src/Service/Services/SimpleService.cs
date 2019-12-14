using System;
using System.Linq;
using System.Threading.Tasks;
using Dawn;
using Domain.Models;
using LoggerService.Interfaces;
using Service.Extensions;
using Service.Interfaces;
using Service.Services.Outputs;

namespace Service.Services
{
    public class SimpleService : ISimpleService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public SimpleService(IUnitOfWork unitOfWork, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IQueryable<SimpleEntityOutput> GetAll()
        {
            var entities = _unitOfWork.GetRepository<SimpleEntity>().GetAll();
            var output = entities.Select(entity => new SimpleEntityOutput(entity));

            return output.AsQueryable();
        }

        public IQueryable<SimpleEntityOutput> DemonstrateExtensionMethod(string name, string otherName)
        {
            _logger.LogTrace($"Looking for entities with {name} and {otherName}");
            var entities = _unitOfWork.GetRepository<SimpleEntity>().GetSimpleEntitiesByName(name, otherName);
            _logger.LogTrace($"Found {entities.Count()} entities matching {name} and {otherName}");

            var output = entities.Select(entity => new SimpleEntityOutput(entity));

            return output;
        }

        public async Task<SimpleEntityOutput> GetSingleById(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<SimpleEntity>().GetSingle(x => x.Id == id);
            entity = Guard.Argument(entity).EntityExists(id);
            return new SimpleEntityOutput(entity);
        }

        public async Task<SimpleEntityOutput> AddNewSimpleEntity(string name)
        {
            var entity = new SimpleEntity(
                Guid.Empty,
                name
            );


            _logger.LogTrace($"Adding {entity}");

            var output = await _unitOfWork.GetRepository<SimpleEntity>().Add(entity);
            var response = _unitOfWork.SaveChanges();

            if (response == 1)
                _logger.LogTrace($"Success! Entity with name {name} added");
            else
                _logger.LogError($"Unable to save changes for {entity}");

            return new SimpleEntityOutput(output.Entity);
        }

        public async Task<SimpleEntityOutput> UpdateSimpleEntity(Guid id, string name)
        {
            var entity = await _unitOfWork.GetRepository<SimpleEntity>().GetSingle(x => x.Id == id);
            entity = Guard.Argument(entity).EntityExists(id);

            var updatedEntity = new SimpleEntity(
                entity.Id,
                name
                );

            _logger.LogTrace($"Updating {entity}");
            _unitOfWork.GetRepository<SimpleEntity>().Update(updatedEntity);
            var response = _unitOfWork.SaveChanges();

            if (response == 1)
                _logger.LogTrace($"Success! Entity with ID {id} updated");
            else
                _logger.LogError($"Unable to save changes for ID {id}");

            return new SimpleEntityOutput(await _unitOfWork.GetRepository<SimpleEntity>().GetSingle(x => x.Id == id));

        }
    }
}
