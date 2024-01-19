﻿using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.Users;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using System.Security.Claims;

namespace Shopway.Presentation.Authentication.Services;

public sealed class UserAuthorizationService(IAuthorizationRepository authorizationRepository) : IUserAuthorizationService
{
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;

    public Result<UserId> GetUserId(AuthorizationHandlerContext context)
    {
        string? userIdentifier = context
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (Ulid.TryParse(userIdentifier, out Ulid userUlid) is false)
        {
            return Result.Failure<UserId>(Error.ParseFailure<Ulid>(nameof(ClaimTypes.NameIdentifier)));
        }

        return UserId.Create(userUlid);
    }

    public async Task<bool> HasPermissionsAsync(UserId userId, params Domain.Enums.Permission[] permissions)
    {
        return await _authorizationRepository
            .HasPermissionsAsync(userId, permissions);
    }

    public async Task<bool> HasRolesAsync(UserId userId, params Domain.Enums.Role[] roles)
    {
        return await _authorizationRepository
            .HasRolesAsync(userId, roles);
    }
}