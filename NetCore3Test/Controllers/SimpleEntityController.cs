using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dawn;
using LoggerService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NetCore3Test.Dtos.Commands;
using Service.Interfaces;
using Service.Services.Outputs;
using Swashbuckle.AspNetCore.Annotations;

namespace NetCore3Test.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SimpleEntityController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ISimpleService _simpleService;
        public SimpleEntityController(ILoggerManager logger, ISimpleService service)
        {
            _logger = logger;
            _simpleService = service;
        }

        [HttpGet("simpleEntity")]
        [SwaggerResponse(200, "Entity data was found", typeof(IEnumerable<SimpleEntityOutput>))]
        [SwaggerResponse(400, "The entity data is invalid")]
        [SwaggerResponse(404, "The entity data was not found")]
        public ActionResult<IEnumerable<SimpleEntityOutput>> SimpleEntityGetAll()
        {
            return Ok(_simpleService.GetAll());
        }

        [HttpPost]
        [Route("simpleEntity")]
        [SwaggerResponse(201, "The entity was created", typeof(SimpleEntityOutput))]
        [SwaggerResponse(400, "The entity data is invalid")]
        [SwaggerResponse(404, "The entity data was not found")]
        public async Task<ActionResult<SimpleEntityOutput>> SimpleEntityPost(SimpleEntityCreateCommand dto)
        {
            var validName = Guard.Argument(dto.Name, nameof(dto.Name)).NotWhiteSpace().NotNull();
            return Ok(await _simpleService.AddNewSimpleEntity(validName).ConfigureAwait(false));

        }

        [HttpGet("simpleEntitiesExtension")]
        [SwaggerResponse(200, "Entity data was found", typeof(IEnumerable<SimpleEntityOutput>))]
        [SwaggerResponse(400, "The entity data is invalid")]
        [SwaggerResponse(404, "The entity data was not found")]
        public IActionResult GetByExtensionMethod(string name, string otherName)
        {
            var validName = Guard.Argument(name, nameof(name)).NotWhiteSpace().NotNull();
            var validOtherName = Guard.Argument(otherName, nameof(otherName)).NotWhiteSpace().NotNull();
            return Ok(_simpleService.DemonstrateExtensionMethod(validName, validOtherName));
        }

        [HttpPatch]
        [Route("simpleEntity/{id}")]
        [SwaggerResponse(201, "The entity was created", typeof(SimpleEntityOutput))]
        [SwaggerResponse(400, "The entity data is invalid")]
        [SwaggerResponse(404, "The entity data was not found")]
        public async Task<ActionResult<SimpleEntityOutput>> SimpleEntityPatch(Guid id, SimpleEntityUpdateCommand dto)
        {
            var validId = Guard.Argument(id.ToString(), nameof(id)).Equal(dto.Id.ToString()).NotNull();
            var validName = Guard.Argument(dto.Name, nameof(dto.Name)).NotWhiteSpace().NotNull();

            return Ok(await _simpleService.UpdateSimpleEntity(Guid.Parse(validId), validName).ConfigureAwait(false));

        }

        [HttpGet("simpleEntity/{id}")]
        [SwaggerResponse(200, "Entity data was found", typeof(SimpleEntityOutput))]
        [SwaggerResponse(400, "The entity data is invalid")]
        [SwaggerResponse(404, "The entity data was not found")]
        public async Task<ActionResult<SimpleEntityOutput>> SimpleEntityGetById(Guid id)
        {
            return Ok(await _simpleService.GetSingleById(id).ConfigureAwait(false));
        }

    }
}
