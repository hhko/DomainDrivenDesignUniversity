﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Products.Commands.AddReview;
using Shopway.Application.Features.Products.Commands.RemoveReview;
using Shopway.Application.Features.Products.Commands.UpdateReview;
using Shopway.Domain.Enums;
using Shopway.Domain.Products;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;

namespace Shopway.Presentation.Controllers;

partial class ProductsController
{
    public const string Reviews = nameof(Reviews);

    [HttpPost($"{{productId}}/{Reviews}")]
    [RequiredPermissions(Domain.Common.Enums.LogicalOperation.Or, Permission.Review_Add, Permission.INVALID_PERMISSION)] //For demo purposes
    [ProducesResponseType<AddReviewResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AddReviewResponse>, ProblemHttpResult>> AddReview
    (
        [FromRoute] ProductId productId,
        [FromBody] AddReviewCommand.AddReviewRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new AddReviewCommand(productId, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpPatch($"{{productId}}/{Reviews}/{{reviewId}}")]
    [RequiredPermissions(Permission.Review_Update)]
    [ProducesResponseType<UpdateReviewResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<UpdateReviewResponse>, ProblemHttpResult>> UpdateReview
    (
        [FromRoute] ProductId productId,
        [FromRoute] ReviewId reviewId,
        [FromBody] UpdateReviewCommand.UpdateReviewRequestBody body,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateReviewCommand(productId, reviewId, body);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }

    [HttpDelete($"{{productId}}/{Reviews}/{{reviewId}}")]
    [RequiredPermissions(Permission.Review_Remove)]
    [RequiredRoles(Role.Administrator)]
    [ProducesResponseType<RemoveReviewResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<RemoveReviewResponse>, ProblemHttpResult>> RemoveReview
    (
        [FromRoute] RemoveReviewCommand command, 
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return TypedResults.Ok(result.Value);
    }
}