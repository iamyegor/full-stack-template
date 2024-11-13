using Domain.Users;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedKernel.Communication.Events;

namespace Api.Consumers;

public class UserConfirmedEmailConsumer : IConsumer<UserConfirmedEmailEvent>
{
    private readonly ApplicationContext _context;

    public UserConfirmedEmailConsumer(ApplicationContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<UserConfirmedEmailEvent> eventContext)
    {
        UserId userId = new UserId(eventContext.Message.Id);
        User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.Id == userId,
            eventContext.CancellationToken
        );

        if (user == null)
        {
            Log.Error("Consuming UserConfirmedEmailEvent. User with id {UserId} not found", userId);
            return;
        }

        user.ConfirmEmail();
        await _context.SaveChangesAsync(eventContext.CancellationToken);
    }
}