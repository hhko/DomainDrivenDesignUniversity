﻿using Shopway.Domain.Common;

namespace Shopway.Domain.Utilities;

public static class SortByEntryUtilities
{
    public static bool AnyInvalidSortPropertyName(this IList<SortByEntry> sortProperties, IReadOnlyCollection<string> allowedSortProperties)
    {
        return sortProperties
            .Select(x => x.PropertyName)
            .Except(allowedSortProperties)
            .Any();
    }

    public static bool DuplicatedSortPriority(this IList<SortByEntry> sortProperties)
    {
        return sortProperties
            .ContainsDuplicates(x => x.SortPriority);
    }
}