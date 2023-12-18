using Microsoft.AspNetCore.Mvc;
using Snapsoft.Dora.Domain.CommandsHandlers;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.WebApi;
using Snapsoft.Dora.WebApi.Dtos;

namespace Snapsoft.Dora.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ComponentController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpPost(Name = nameof(CreateComponent))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponseDto<ComponentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UnprocessableEntityResponseDto))]
        public async Task<IActionResult> CreateComponent(
            [FromBody] CreateComponentCommand createComponentCommand)
        {
            var handler = _serviceProvider.GetRequiredService<CreateComponentCommandHandler>();
            var result = await handler.HandleAsync(createComponentCommand);
            return result.ToActionResult(successResult => ToComponentDto((Component)successResult.Value));            
        }
                
        [HttpGet("{id:long}", Name = nameof(GetComponentById))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponseDto<ComponentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UnprocessableEntityResponseDto))]
        public async Task<IActionResult> GetComponentById([FromRoute] long id)
        {
            var repo = _serviceProvider.GetRequiredService<IRepository<Component>>();
            var component = await repo.GetByIdAsync(id);

            if(component == null) return NotFound();

            var dto = ToComponentDto(component);
            return Ok(dto.ToSuccessResponseDto());
        }

        private static ComponentDto ToComponentDto(Component component) => new ComponentDto
        {
            Id = component.Id,
            Name = component.Name,
        };
    }
}