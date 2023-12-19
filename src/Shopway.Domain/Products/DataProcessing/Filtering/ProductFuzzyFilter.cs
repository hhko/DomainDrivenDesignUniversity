﻿using System.Linq.Expressions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Domain.Products.DataProcessing.Filtering;

public sealed record ProductFuzzyFilter : IFilter<Product>
{
    public required Expression<Func<Product, bool>> FuzzyFilter { get; set; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Where(FuzzyFilter);
    }
}