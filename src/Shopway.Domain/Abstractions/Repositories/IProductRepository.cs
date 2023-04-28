﻿using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Domain.EntityIds;
using System.Linq.Expressions;

namespace Shopway.Domain.Abstractions.Repositories;

public interface IProductRepository
{
    Task<Product> GetByKeyAsync(ProductKey key, CancellationToken cancellationToken);

    Task<bool> AnyAsync(ProductKey key, CancellationToken cancellationToken);

    Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken);

    Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes);

    Task<(IList<TResponse> Responses, int TotalCount)> PageQuery<TResponse>(IFilter<Product>? filter, ISortBy<Product>? sortBy, IPage page, Expression<Func<Product, TResponse>>? select, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes);

    void Create(Product product);

    void Update(Product product);

    void Remove(Product product);
}