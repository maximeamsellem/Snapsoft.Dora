using Microsoft.AspNetCore.Mvc;
using Snapsoft.Dora.Domain.CommandsHandlers;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;
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

        [HttpPost(Name = "CreateComponent")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponseDto<ComponentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UnprocessableEntityResponseDto))]
        public async Task<IActionResult> CreateComponent(
            [FromBody] CreateComponentCommand createComponentCommand)
        {
            var handler = _serviceProvider.GetRequiredService<CreateComponentCommandHandler>();
            var result = await handler.HandleAsync(createComponentCommand);

            return result switch
            {
                SuccessCommandResult s => base.Created(string.Empty, ToSuccessResponseDto(s)),
                UnprocessableCommandResult u when u.HasUnicityError => base.Conflict(ToUnprocessableEntityResponseDto(u)),
                UnprocessableCommandResult u when !u.HasUnicityError => base.UnprocessableEntity(ToUnprocessableEntityResponseDto(u)),
                _ => base.Problem()
            };
        }
                
        [HttpGet("{id:long}", Name = "GetComponentById")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponseDto<ComponentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UnprocessableEntityResponseDto))]
        public async Task<IActionResult> GetComponentById([FromRoute] long id)
        {
            var repo = _serviceProvider.GetRequiredService<IRepository<Component>>();
            var component = await repo.GetByIdAsync(id);

            if(component == null) return NotFound();

            return Ok(new SuccessResponseDto<ComponentDto>
            {
                Data = new ComponentDto
                {
                    Id = component.Id,
                    Name = component.Name
                }
            });
        }

        private static SuccessResponseDto<Component> ToSuccessResponseDto(SuccessCommandResult s)
        {
            return new SuccessResponseDto<Component>
            {
                Data = s.Value as Component
            };
        }

        private static UnprocessableEntityResponseDto ToUnprocessableEntityResponseDto(UnprocessableCommandResult u)
        {
            return new UnprocessableEntityResponseDto
            {
                PropertyErrors = u.PropertyErrors,
            };
        }
    }
}