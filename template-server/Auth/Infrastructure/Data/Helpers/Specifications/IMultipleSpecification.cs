namespace Infrastructure.Data.Helpers.Specifications;

public interface IMultipleSpecification<T>
{
    public IQueryable<T> Apply(IQueryable<T> query);
}
