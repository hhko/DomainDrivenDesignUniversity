﻿using Shopway.Application.CQRS;
using Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using static Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Application.Mappings;

public static class ProductMapping
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        (
            Id: product.Id.Value,
            ProductName: product.ProductName.Value,
            Revision: product.Revision.Value,
            Price: product.Price.Value,
            UomCode: product.UomCode.Value,
            Reviews: product.Reviews.ToResponses()
        );
    }

    public static UpdateProductResponse ToUpdateResponse(this Product productToUpdate)
    {
        return new UpdateProductResponse(productToUpdate.Id.Value);
    }

    public static RemoveProductResponse ToRemoveResponse(this Product productToRemove)
    {
        return new RemoveProductResponse(productToRemove.Id.Value);
    }

    public static CreateProductResponse ToCreateResponse(this Product productToCreate)
    {
        return new CreateProductResponse(productToCreate.Id.Value);
    }

    public static BatchUpsertProductResponse ToBatchUpsertResponse(this IList<BatchResponseEntry> batchResponseEntries)
    {
        return new BatchUpsertProductResponse(batchResponseEntries);
    }

    public static ProductKey MapFromRequestToProductKey(ProductBatchUpsertRequest productBatchRequest)
    {
        return ProductKey.Create(productBatchRequest.ProductKey.ProductName, productBatchRequest.ProductKey.Revision);
    }

    public static ProductKey MapFromProductToProductKey(Product product)
    {
        return ProductKey.Create(product.ProductName.Value, product.Revision.Value);
    }
}