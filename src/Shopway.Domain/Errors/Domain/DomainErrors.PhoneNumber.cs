﻿using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class PhoneNumberError
    {
        public static readonly Error Empty = Error.New(
            $"{nameof(PhoneNumber)}.{nameof(Empty)}",
            $"{nameof(PhoneNumber)} is empty");

        public static readonly Error Invalid = Error.New(
            $"{nameof(PhoneNumber)}.{nameof(Invalid)}",
            $"{nameof(PhoneNumber)} must consist of 9 digits and cannot start from zero");
    }
}