namespace SharedKernel.Communication.Events;

public class UserRegisteredEvent
{
    public UserRegisteredEvent(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    public Guid Id { get; }
    public string Email { get; }
}