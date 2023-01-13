﻿using Shopway.Domain.Abstractions.BaseTypes;

namespace Shopway.Domain.Abstractions;

public interface IFilter
{
}

public interface IFilter<TEntity> : IFilter
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}