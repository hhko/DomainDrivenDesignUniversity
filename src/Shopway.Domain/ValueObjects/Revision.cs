﻿using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Revision : ValueObject
{
    public const int MaxLength = 10;

    public string Value { get; }

    private Revision(string value)
    {
        Value = value;
    }

    //For EF Core
    private Revision()
    {
    }

    public static ValidationResult<Revision> Create(string revision)
    {
        var errors = Validate(revision);
        return errors.CreateValidationResult(() => new Revision(revision));
    }

    public static List<Error> Validate(string revision)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(revision))
        {
            errors.Add(RevisionError.Empty);
        }

        if (revision.Length > MaxLength)
        {
            errors.Add(RevisionError.TooLong);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

