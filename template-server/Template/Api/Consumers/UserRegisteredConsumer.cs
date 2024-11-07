using System.Reflection;
using Domain.Users;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using MassTransit;
using Serilog;
using SharedKernel.Communication.Events;

namespace Api.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly ApplicationContext _context;

    public UserRegisteredConsumer(ApplicationContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> consumeContext)
    {
        Email email = Email.Create(consumeContext.Message.Email);
        User user = User.Create(consumeContext.Message.Id, email);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        string? assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        Log.Information($"${assemblyName} Consumed user: {@user}", user);
    }
}
