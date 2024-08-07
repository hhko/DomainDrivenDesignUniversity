﻿using Shopway.Domain.Common.BaseTypes;
using System.Text.Json;

namespace Shopway.Domain.Products.Events;

public sealed record ReviewAddedDomainEvent(Ulid Id, ReviewId ReviewId, ProductId ProductId) : DomainEvent(Id)
{
    public static ReviewAddedDomainEvent New(ReviewId ReviewId, ProductId ProductId)
    {
        return new ReviewAddedDomainEvent(Ulid.NewUlid(), ReviewId, ProductId);
    }
}
