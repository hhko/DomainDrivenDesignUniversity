﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Orders;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class OrderHeaderIdConverter : ValueConverter<OrderHeaderId, string>
{
    public OrderHeaderIdConverter() : base(id => id.Value.ToString(), ulid => OrderHeaderId.Create(Ulid.Parse(ulid))) { }
}

public sealed class OrderHeaderIdComparer : ValueComparer<OrderHeaderId>
{
    public OrderHeaderIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}
