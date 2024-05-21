﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Results.Abstractions;
using static Shopway.Application.Constants.Constants.ProblemDetails;
using static Shopway.Application.Utilities.ProblemDetailsUtilities;
using IResult = Shopway.Domain.Common.Results.IResult;

namespace Shopway.Presentation.Abstractions;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;
}

public static class ResultUtilities
{
    public static Ok<TValue> ToOkResult<TValue>(this IResult<TValue> result)
    {
        return TypedResults.Ok(result.Value);
    }

    public static BadRequest<TValue> ToBadRequestResult<TValue>(this IResult<TValue> result)
    {
        return TypedResults.BadRequest(result.Value);
    }

    public static Ok ToOkResult(this IResult result)
    {
        return TypedResults.Ok();
    }

    public static ProblemHttpResult ToProblemHttpResult(this IResult result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException("Result was successful"),

            IValidationResult validationResult => TypedResults.Problem
            (
                CreateProblemDetails
                (
                    ValidationError,
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    validationResult.ValidationErrors
                )
            ),

            _ => TypedResults.Problem
            (
                CreateProblemDetails
                (
                    InvalidRequest,
                    StatusCodes.Status400BadRequest,
                    result.Error
                )
            )
        };
    }
}