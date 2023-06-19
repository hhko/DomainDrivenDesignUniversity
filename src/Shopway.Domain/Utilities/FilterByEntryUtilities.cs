﻿using Shopway.Domain.Common;

namespace Shopway.Domain.Utilities;

public static class FilterByEntryUtilities
{
    public static bool ContainsInvalidFilterProperty(this IList<FilterByEntry> filterProperties, IReadOnlyCollection<string> allowedFilterProperties)
    {
        return filterProperties
            .SelectMany(x => x.Predicates)
            .Select(x => x.PropertyName)
            .Except(allowedFilterProperties)
            .Any();
    }

    public static bool ContainsOnlyOperationsFrom(this IList<FilterByEntry> filterProperties, IReadOnlyCollection<string> allowedOperations, out IReadOnlyCollection<string> invalidOperations)
    {
        invalidOperations = filterProperties
            .SelectMany(x => x.Predicates)
            .Select(x => x.Operation)
            .Except(allowedOperations)
            .ToList()
            .AsReadOnly();

        return invalidOperations.Any();
    }
}