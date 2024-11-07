namespace Domain.Common;

public abstract class Entity<TId>
    where TId : notnull
{
    public TId Id { get; }

    public Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> entity)
        {
            return false;
        }
        else if (GetRealType() != entity.GetRealType())
        {
            return false;
        }

        return Id.Equals(entity.Id);
    }

    private Type GetRealType()
    {
        Type type = GetType();
        if (type.ToString().Contains("Castle.Proxies."))
        {
            return type.BaseType!;
        }

        return type;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        return Equals(a, b);
    }

    public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
    {
        return !(a == b);
    }
}
