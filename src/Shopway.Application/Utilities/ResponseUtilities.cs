﻿using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Results;

namespace Shopway.Application.Utilities;

public static class ResponseUtilities
{
    public static IResult<TResponse> ToResult<TResponse>(this TResponse response)
        where TResponse : class, IResponse
    {
        return Result.Create(response);
    }
}