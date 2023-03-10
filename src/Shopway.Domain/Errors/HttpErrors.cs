﻿using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors;

public static class HttpErrors
{
    /// <summary>
    /// Create an Error describing that a password or an email are invalid
    /// </summary>
    public static readonly Error InvalidPasswordOrEmail = new($"{nameof(User)}.{nameof(InvalidPasswordOrEmail)}", $"Invalid {nameof(Password)} or {nameof(Email)}");

    /// <summary>
    /// Create an Error based on the entity type name and the id that was not found
    /// </summary>
    /// <param name="name">name of the entity type. Use "nameof(TValue)" syntax</param>
    /// <param name="id">id of the entity that was not found</param>
    /// <returns>NotFound error</returns>
    public static Error NotFound<TEntityId>(string name, IEntityId<TEntityId> id)
    {
        return new Error($"{name}.{nameof(NotFound)}", $"{name} with Id: '{id.Value}' was not found");
    }

    /// <summary>
    /// Create an Error based on the business key
    /// </summary>
    /// <param name="name">name of the product key type. Use "nameof(TValue)" syntax</param>
    /// <param name="key">business key of the entity that is already in the database</param>
    /// <returns>AlreadyExists error</returns>
    public static Error AlreadyExists<TBusinessKey>(string name, TBusinessKey key)
        where TBusinessKey : IBusinessKey
    {
        return new Error($"{name}.{nameof(AlreadyExists)}", $"{name} with key: '{key}' already exists");
    }

    /// <summary>
    /// Create an Error describing that the provided reference is invalid
    /// </summary>
    /// <returns>InvalidReference error</returns>
    public static Error InvalidReference(Guid reference, string entity)
    {
        return new Error($"{nameof(Error)}.{nameof(InvalidReference)}", $"Invalid reference {reference} for entity {entity}");
    }

    /// <summary>
    /// Create an Error describing that the collection is null or empty
    /// </summary>
    /// <returns>NullOrEmpty error</returns>
    public static Error NullOrEmpty(string collectionName)
    {
        return new($"{nameof(Error)}.{nameof(NullOrEmpty)}", $"{collectionName} is null or empty");
    }

    /// <summary>
    /// Create an Error describing that the collection is null or empty
    /// </summary>
    /// <returns>NullOrEmpty error</returns>
    public static Error InvalidBatchCommand(string batchCommand)
    {
        return new($"{nameof(Error)}.{nameof(InvalidBatchCommand)}", $"{batchCommand} is invalid");
    }

    /// <summary>
    /// Create an Error from the thrown exception
    /// </summary>
    /// <param name="exceptionMessage">Exception message</param>
    /// <returns>Error</returns>
    public static Error Exception(string exceptionMessage)
    {
        return new Error($"{nameof(Error)}.{nameof(Exception)}", exceptionMessage);
    }
}
