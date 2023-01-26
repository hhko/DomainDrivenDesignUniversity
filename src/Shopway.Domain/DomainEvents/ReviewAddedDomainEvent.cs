﻿using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record ReviewAddedDomainEvent(Guid Id, ReviewId ReviewId, ProductId ProductId) : DomainEvent(Id)
{
    public static ReviewAddedDomainEvent New(ReviewId ReviewId, ProductId ProductId)
    {
        return new ReviewAddedDomainEvent(Guid.NewGuid(), ReviewId, ProductId);
    }
}