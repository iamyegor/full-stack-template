using Domain.OutboxMessages;
using Infrastructure.Data;
using Newtonsoft.Json;

namespace Infrastructure.Features.Outbox;

public class OutboxService
{
    private readonly ApplicationContext _context;
    private static readonly JsonSerializerSettings JsonSettings =
        new() { TypeNameHandling = TypeNameHandling.All, };

    public OutboxService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task SaveMessageAsync<T>(T message, CancellationToken ct = default)
        where T : class
    {
        OutboxMessage outboxMessage = OutboxMessage.Create(
            typeof(T).FullName!,
            JsonConvert.SerializeObject(message, JsonSettings)
        );

        _context.OutboxMessages.Add(outboxMessage);
        await _context.SaveChangesAsync(ct);
    }
}
