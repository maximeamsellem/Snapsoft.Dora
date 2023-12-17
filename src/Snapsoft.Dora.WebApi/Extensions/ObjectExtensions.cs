using Microsoft.AspNetCore.Mvc;
using Snapsoft.Dora.Domain.CommandsHandlers;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.WebApi.Dtos;

namespace Snapsoft.Dora.WebApi;

internal static class ObjectExtensions
{
    internal static SuccessResponseDto<TSuccessRespondeData> ToSuccessResponseDto<TSuccessRespondeData>(
        this TSuccessRespondeData responseData)
    {
        return new SuccessResponseDto<TSuccessRespondeData>
        {
            Data = responseData
        };
    }
}
