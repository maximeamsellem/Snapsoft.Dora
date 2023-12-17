using Microsoft.AspNetCore.Mvc;
using Snapsoft.Dora.Domain.CommandsHandlers;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.Domain.Read.Contracts.Repositories;
using Snapsoft.Dora.WebApi.Dtos;

namespace Snapsoft.Dora.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentDeploymentController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ComponentDeploymentController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpPost(Name = nameof(CreateComponentDeployment))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponseDto<ComponentDeploymentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UnprocessableEntityResponseDto))]
        public async Task<IActionResult> CreateComponentDeployment(
            [FromBody] CreateComponentDeploymentCommand command)
        {
            var handler = _serviceProvider.GetRequiredService<CreateComponentDeploymentCommandHandler>();
            var result = await handler.HandleAsync(command);

            return result switch
            {
                SuccessCommandResult s => base.Created(string.Empty, ToSuccessResponseDto((ComponentDeployment)s.Value)),
                UnprocessableCommandResult u when u.HasUnicityError => base.Conflict(ToUnprocessableEntityResponseDto(u)),
                UnprocessableCommandResult u when !u.HasUnicityError => base.UnprocessableEntity(ToUnprocessableEntityResponseDto(u)),
                _ => base.Problem()
            };
        }
                
        [HttpGet("{id:long}", Name = nameof(GetComponentDeploymentById))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponseDto<ComponentDeploymentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(UnprocessableEntityResponseDto))]
        public async Task<IActionResult> GetComponentDeploymentById([FromRoute] long id)
        {
            var repo = _serviceProvider.GetRequiredService<IComponentDeploymentRepository>();
            var componentDeployment = await repo.GetByIdWithComponentAsync(id);

            if(componentDeployment == null) return NotFound();

            return Ok(ToSuccessResponseDto(componentDeployment));
        }

        private static SuccessResponseDto<ComponentDeploymentDto> ToSuccessResponseDto(
            ComponentDeployment componentDeployment)
        {
            return new SuccessResponseDto<ComponentDeploymentDto>
            {
                Data = new ComponentDeploymentDto
                {
                    Id = componentDeployment.Id,
                    CommitId = componentDeployment.CommitId,
                    ComponentId = componentDeployment.ComponentId,
                    ComponentName = componentDeployment.Component?.Name ?? string.Empty,
                    Version = componentDeployment.Version,
                }
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