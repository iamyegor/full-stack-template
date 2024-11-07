using Domain.Common;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public class User : Entity<Guid>
{
    public Email Email { get; private set; }

    protected User()
        : this(Guid.NewGuid()) { }

    private User(Guid? id = null)
        : base(id ?? Guid.NewGuid()) { }

    public static User Create(Guid id, Email email) => new User(id) { Email = email };
}
