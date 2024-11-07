using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.User.Queries.IsAuthenticated;

public record IsAuthenticatedQuery(UserId UserId) : IRequest<Result<bool, Error>>;

public class IsAuthenticatedQueryHandler
    : IRequestHandler<IsAuthenticatedQuery, Result<bool, Error>>
{
    private readonly ApplicationContext _context;

    public IsAuthenticatedQueryHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Result<bool, Error>> Handle(
        IsAuthenticatedQuery query,
        CancellationToken cancellationToken
    )
    {
        Domain.User.User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.Id == query.UserId,
            cancellationToken
        );
        if (user == null)
        {
            return Errors.User.NotFound;
        }

        return user.IsEmailVerified;
    }
}
