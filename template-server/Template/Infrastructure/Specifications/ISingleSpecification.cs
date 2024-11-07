namespace Infrastructure.Specifications;

public interface ISingleSpecification<T>
    where T : class
{
    IQueryable<T> Apply(IQueryable<T> query);
}
