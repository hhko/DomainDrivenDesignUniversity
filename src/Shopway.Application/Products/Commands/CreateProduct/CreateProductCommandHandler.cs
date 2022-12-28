﻿using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Result<ProductName> productNameResult = ProductName.Create(command.ProductName);
        Result<Price> priceResult = Price.Create(command.Price);
        Result<UomCode> uomCodeResult = UomCode.Create(command.UomCode);
        Result<Revision> revisionResult = Revision.Create(command.Revision);

        Error error = ErrorHandler.FirstValueObjectErrorOrErrorNone(productNameResult, priceResult, uomCodeResult, revisionResult);

        if (error != Error.None)
        {
            return Result.Failure<Guid>(error);
        }

        var productId = new ProductId()
        {
            Value = Guid.NewGuid()
        };

        var product = Product.Create(
            productId,
            productNameResult.Value,
            priceResult.Value,
            uomCodeResult.Value,
            revisionResult.Value);

        _productRepository.Create(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id.Value;
    }
}