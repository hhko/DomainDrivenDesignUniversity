﻿using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Tests.Integration.Constants;
using System.Security.Claims;

namespace Shopway.Tests.Integration.Persistance;

/// <summary>
/// Test context service, used to set the "CreatedBy" field to the user name
/// </summary>
public sealed class TestContextService : IUserContextService
{
    public ClaimsPrincipal? User => null;

    public UserId? UserId => null;

    public string? Username => TestUser.Username;

    public CustomerId? CustomerId => null;
}