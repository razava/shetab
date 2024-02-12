﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    private readonly List<DomainEvent> _domainEvents = new();
    [NotMapped]
    public ICollection<DomainEvent> DomainEvents => _domainEvents;

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private init; }

    public static bool operator ==(Entity? left, Entity? right)
    {
        return (left is not null && right is not null && left.Equals(right));
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if(obj.GetType() != GetType()) 
            return false;
        if (obj is not Entity entity) 
            return false;
        return entity.Id == Id;
    }

    public bool Equals(Entity? other)
    {
        if(other is null) 
            return false;
        if(other.GetType() != GetType()) 
            return false;
        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected void Raise(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
