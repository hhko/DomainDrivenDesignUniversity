﻿using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Application.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Abstractions;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.CQRS.Users.Queries.GetUserByUsername;

internal sealed class GetUserByUsernameQueryHandler : IQueryHandler<GetUserByUsernameQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator _validator;

    public GetUserByUsernameQueryHandler(IUserRepository userRepository, IValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<IResult<UserResponse>> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken)
    {
        var usernameResult = Username.Create(query.Username);

        _validator
            .Validate(usernameResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<UserResponse>();
        }

        var user = await _userRepository
            .GetByUsernameAsync(usernameResult.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(NotFound<User>(query.Username));
        }

        return user
            .ToResponse()
            .ToResult();
    }
}