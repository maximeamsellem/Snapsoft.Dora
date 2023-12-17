using Microsoft.AspNetCore.Mvc;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;
using Snapsoft.Dora.WebApi.Dtos;
using System.Net;

namespace Snapsoft.Dora.WebApi;

public static class ICommandResultExtensions
{
    public static IActionResult ToActionResult<TSuccessRespondeData>(
        this ICommandResult commandResult,
        Func<CreationSuccessCommandResult, TSuccessRespondeData> mapToSuccessResponseData)
    {
        return commandResult switch
        {
            CreationSuccessCommandResult s => 
                new ObjectResult(mapToSuccessResponseData(s).ToSuccessResponseDto())
                {
                    StatusCode = 201,
                },

            UnprocessableCommandResult u when u.HasUnicityError => 
                new ConflictObjectResult(ToUnprocessableEntityResponseDto(u)),

            UnprocessableCommandResult u when !u.HasUnicityError => 
                new UnprocessableEntityObjectResult(ToUnprocessableEntityResponseDto(u)),

            _ => new StatusCodeResult((int)HttpStatusCode.InternalServerError)
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
