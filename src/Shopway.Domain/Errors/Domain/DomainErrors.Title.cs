﻿using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class TitleError
    {
        public static readonly Error Empty = new(
            $"{nameof(Title)}.{nameof(Empty)}",
            $"{nameof(Title)} is empty");

        public static readonly Error TooLong = new(
            $"{nameof(Title)}.{nameof(TooLong)}",
            $"{nameof(Title)} needs to be at most {Title.MaxLength} characters long");
    }
}