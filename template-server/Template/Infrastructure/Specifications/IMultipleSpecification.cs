namespace Infrastructure.Specifications;

public interface IMultipleSpecification<T>
{
    public IQueryable<T> Apply(IQueryable<T> query);
}
