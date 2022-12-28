﻿using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.Primitives;

public abstract class Entity<TEntityId> : IEquatable<Entity<TEntityId>>
    where TEntityId : IEntityId, new()
{
    protected Entity(Guid id)
    {
        Id = new TEntityId()
        {
            Value = id
        };
    }

    protected Entity()
    {
    }

    public TEntityId Id { get; private init; }

    public static bool operator ==(Entity<TEntityId>? first, Entity<TEntityId>? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(Entity<TEntityId>? first, Entity<TEntityId>? second)
    {
        return !(first == second);
    }

    public bool Equals(Entity<TEntityId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id.Value == Id.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity<TEntityId> entity)
        {
            return false;
        }

        return entity.Id.Value == Id.Value;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}