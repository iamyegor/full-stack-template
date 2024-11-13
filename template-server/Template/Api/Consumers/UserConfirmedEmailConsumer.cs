using Domain.Users;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using MassTransit;
using SharedKernel.Communication.Events;

namespace Api.Consumers;

public class UserConfirmedEmailConsumer : IConsumer<UserConfirmedEmailEvent>
{
    private readonly ApplicationContext _context;

    public UserConfirmedEmailConsumer(ApplicationContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<UserConfirmedEmailEvent> consumeContext)
    {
        Email email = Email.Create(consumeContext.Message.Email);
        User user = User.Create(consumeContext.Message.Id, email);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
