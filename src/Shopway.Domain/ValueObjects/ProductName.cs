﻿using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;
using System.Net.Http.Headers;

namespace Shopway.Domain.ValueObjects;

public sealed class ProductName : ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    private ProductName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static ValidationResult<ProductName> Create(string productName)
    {
        var errors = Validate(productName);
        return errors.CreateValidationResult(() => new ProductName(productName));
    }

    public static List<Error> Validate(string productName)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(productName))
        {
            errors.Add(ProductNameError.Empty);
        }

        if (productName.Length > MaxLength)
        {
            errors.Add(ProductNameError.TooLong);
        }

        if (productName.ContainsIllegalCharacter())
        {
            errors.Add(ProductNameError.ContainsIllegalCharacter);
        }

        return errors;
    }
}