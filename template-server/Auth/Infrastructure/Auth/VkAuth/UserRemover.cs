using Domain.User;
using Infrastructure.Data;

namespace Infrastructure.Auth.VkAuth;

public class UserRemover
{
    private readonly ApplicationContext _context;

    public UserRemover(ApplicationContext context)
    {
        _context = context;
    }

    public async Task RemoveUserIfExists(User? existingUser, CancellationToken ct)
    {
        if (existingUser != null)
        {
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync(ct);
        }
    }
}
