namespace Domain.Common;

public class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    protected AggregateRoot(TId id)
        : base(id) { }
}
