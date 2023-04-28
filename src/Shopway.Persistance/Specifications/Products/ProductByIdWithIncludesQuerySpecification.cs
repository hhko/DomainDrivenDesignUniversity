﻿using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal sealed class ProductByIdWithIncludesQuerySpecification : SpecificationBase<Product, ProductId>
{
    private ProductByIdWithIncludesQuerySpecification() : base()
    {
    }

    internal static SpecificationBase<Product, ProductId> Create(ProductId productId, params Expression<Func<Product, object>>[] includes)
    {
        return new ProductByIdWithIncludesQuerySpecification()
            .AddFilters(product => product.Id == productId)
            .AddIncludes(includes)
            .UseSplitQuery();
    }
}