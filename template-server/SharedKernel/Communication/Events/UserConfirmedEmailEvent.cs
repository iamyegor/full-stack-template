namespace SharedKernel.Communication.Events;

public class UserConfirmedEmailEvent
{
    public UserConfirmedEmailEvent(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    public Guid Id { get; }
    public string Email { get; }
}