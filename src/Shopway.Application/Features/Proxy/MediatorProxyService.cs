﻿using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Application.Features.Proxy.PageQuery;
using Shopway.Application.Features.Proxy.Query;
using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Domain.Common.Discriminators;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService(IValidator validator) : IMediatorProxyService
{
    private static readonly FrozenDictionary<PageQueryDiscriminator, Func<ProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyPageQueryCache =
        StrategyCacheFactory<PageQueryDiscriminator, Func<ProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<MediatorProxyService, PageQueryStrategyAttribute>();

    private static readonly FrozenDictionary<QueryDiscriminator, Func<ProxyQuery, IQuery<DataTransferObjectResponse>>> _strategyQueryCache =
        StrategyCacheFactory<QueryDiscriminator, Func<ProxyQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, QueryStrategyAttribute>();

    private static readonly FrozenDictionary<GenericPageQueryDiscriminator, Func<GenericProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>> _strategyGenericPageQueryCache =
        StrategyCacheFactory<GenericPageQueryDiscriminator, Func<GenericProxyPageQuery, IQuery<PageResponse<DataTransferObjectResponse>>>>
            .CreateFor<MediatorProxyService, GenericPageQueryStrategyAttribute>();

    private static readonly FrozenDictionary<GenericByIdQueryDiscriminator, Func<GenericProxyByIdQuery, IQuery<DataTransferObjectResponse>>> _strategyGenericQueryByIdCache =
        StrategyCacheFactory<GenericByIdQueryDiscriminator, Func<GenericProxyByIdQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, GenericByIdQueryStrategyAttribute>();

    private static readonly FrozenDictionary<GenericByKeyQueryDiscriminator, Func<GenericProxyByKeyQuery, IQuery<DataTransferObjectResponse>>> _strategyGenericQueryByKeyCache =
        StrategyCacheFactory<GenericByKeyQueryDiscriminator, Func<GenericProxyByKeyQuery, IQuery<DataTransferObjectResponse>>>
            .CreateFor<MediatorProxyService, GenericByKeyQueryStrategyAttribute>();

    private readonly IValidator _validator = validator;

    public Result<IQuery<DataTransferObjectResponse>> Map(ProxyQuery proxyQuery)
    {
        _validator
            .If(proxyQuery.Mapping is null || proxyQuery.Mapping.MappingEntries.IsEmpty(), Error.InvalidArgument("Mapping must be provided."));

        if (_validator.IsInvalid)
        {
            return Failure<DataTransferObjectResponse>();
        }

        var strategyKey = new QueryDiscriminator(proxyQuery.Entity);

        _validator
            .If(_strategyQueryCache.TryGetValue(strategyKey, out var @delegate) is false, Error.InvalidOperation($"Entity '{proxyQuery.Entity}' is not supported. Supported strategies: [{string.Join(", ", _strategyQueryCache.Keys.Select(x => x.Entity))}]"));

        if (_validator.IsInvalid)
        {
            return Failure<DataTransferObjectResponse>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<PageResponse<DataTransferObjectResponse>>> Map(ProxyPageQuery proxyQuery)
    {
        _validator
            .If(PageIsNotOffsetOrCursorPage(proxyQuery.Page), Error.InvalidArgument("Cursor or PageNumber must be provided."))
            .If(PageIsBothOffsetAndCursorPage(proxyQuery.Page), Error.InvalidArgument("Both Cursor and PageNumber cannot be provided."));

        if (_validator.IsInvalid)
        {
            return Failure<PageResponse<DataTransferObjectResponse>>();
        }

        var strategyKey = new PageQueryDiscriminator(proxyQuery.Entity, proxyQuery.Page.GetPageType());

        _validator
            .If(_strategyPageQueryCache.TryGetValue(strategyKey, out var @delegate) is false, Error.InvalidOperation($"Entity '{proxyQuery.Entity}' with page type '{proxyQuery.Page.GetPageType().Name}' is not supported. Supported strategies: [{string.Join(", ", _strategyPageQueryCache.Keys.Select(x => (x.Entity, x.PageType.Name)))}]"));

        if (_validator.IsInvalid)
        {
            return Failure<PageResponse<DataTransferObjectResponse>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<DataTransferObjectResponse>> GenericMap(GenericProxyByIdQuery proxyQuery)
    {
        _validator
            .If(proxyQuery.Mapping is null || proxyQuery.Mapping.MappingEntries.IsEmpty(), Error.InvalidArgument("Mapping must be provided."));

        if (_validator.IsInvalid)
        {
            return Failure<DataTransferObjectResponse>();
        }

        var strategyKey = new GenericByIdQueryDiscriminator(proxyQuery.Entity);

        _validator
            .If(_strategyGenericQueryByIdCache.TryGetValue(strategyKey, out var @delegate) is false, Error.InvalidOperation($"Entity '{proxyQuery.Entity}' is not supported. Supported strategies: [{string.Join(", ", _strategyGenericQueryByIdCache.Keys.Select(x => x.Entity))}]"));

        if (_validator.IsInvalid)
        {
            return Failure<DataTransferObjectResponse>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<DataTransferObjectResponse>> GenericMap(GenericProxyByKeyQuery proxyQuery)
    {
        _validator
            .If(proxyQuery.Mapping is null || proxyQuery.Mapping.MappingEntries.IsEmpty(), Error.InvalidArgument("Mapping must be provided."));

        if (_validator.IsInvalid)
        {
            return Failure<DataTransferObjectResponse>();
        }

        var strategyKey = new GenericByKeyQueryDiscriminator(proxyQuery.Entity);

        _validator
            .If(_strategyGenericQueryByKeyCache.TryGetValue(strategyKey, out var @delegate) is false, Error.InvalidOperation($"Entity '{proxyQuery.Entity}' is not supported. Supported strategies: [{string.Join(", ", _strategyGenericQueryByKeyCache.Keys.Select(x => x.Entity))}]"));

        if (_validator.IsInvalid)
        {
            return Failure<DataTransferObjectResponse>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    public Result<IQuery<PageResponse<DataTransferObjectResponse>>> GenericMap(GenericProxyPageQuery proxyQuery)
    {
        _validator
            .If(PageIsNotOffsetOrCursorPage(proxyQuery.Page), Error.InvalidArgument("Cursor or PageNumber must be provided."))
            .If(PageIsBothOffsetAndCursorPage(proxyQuery.Page), Error.InvalidArgument("Both Cursor and PageNumber cannot be provided."));

        if (_validator.IsInvalid)
        {
            return Failure<PageResponse<DataTransferObjectResponse>>();
        }

        var strategyKey = new GenericPageQueryDiscriminator(proxyQuery.Entity, proxyQuery.Page.GetPageType());

        _validator
            .If(_strategyGenericPageQueryCache.TryGetValue(strategyKey, out var @delegate) is false, Error.InvalidOperation($"Entity '{proxyQuery.Entity}' with page type '{proxyQuery.Page.GetPageType().Name}' is not supported. Supported strategies: [{string.Join(", ", _strategyGenericPageQueryCache.Keys.Select(x => (x.Entity, x.PageType.Name)))}]"));

        if (_validator.IsInvalid)
        {
            return Failure<PageResponse<DataTransferObjectResponse>>();
        }

        return Result.Success(@delegate!(proxyQuery));
    }

    private static bool PageIsNotOffsetOrCursorPage(OffsetOrCursorPage offsetOrCursorPage)
    {
        return offsetOrCursorPage.Cursor is null && offsetOrCursorPage.PageNumber is null;
    }

    private static bool PageIsBothOffsetAndCursorPage(OffsetOrCursorPage offsetOrCursorPage)
    {
        return offsetOrCursorPage.Cursor is not null && offsetOrCursorPage.PageNumber is not null;
    }

    private ValidationResult<IQuery<TValue>> Failure<TValue>()
        where TValue : class, IResponse
    {
        return ValidationResult<IQuery<TValue>>
            .WithErrors(_validator.Failure().ValidationErrors);
    }
}
