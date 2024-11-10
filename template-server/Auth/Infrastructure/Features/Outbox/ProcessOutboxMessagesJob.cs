using Domain.OutboxMessages;
using Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.Features.Outbox;

public class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly ApplicationContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProcessOutboxMessagesJob(ApplicationContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _context
            .OutboxMessages.Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.CreatedAt)
            .Take(50)
            .ToListAsync();

        foreach (OutboxMessage message in messages)
        {
            message.MarkAsProcessed();
            await _context.SaveChangesAsync();

            object? eventMessage = JsonConvert.DeserializeObject(
                message.Content,
                Type.GetType(message.Type)!,
                JsonSettings
            );

            await _publishEndpoint.Publish(eventMessage!, context.CancellationToken);
        }

        // foreach (OutboxMessage message in messages)
        // {
        //     try
        //     {
        //         object? eventMessage = JsonConvert.DeserializeObject(
        //             message.Content,
        //             Type.GetType(message.Type)!,
        //             JsonSettings
        //         );
        //
        //         await _publishEndpoint.Publish(eventMessage!, context.CancellationToken);
        //         message.MarkAsProcessed();
        //     }
        //     catch (Exception ex)
        //     {
        //         Log.Error(ex, "Failed to process outbox message {MessageId}", message.Id);
        //         message.MarkAsFailed(ex.Message);
        //     }
        // }
        //
        // await _context.SaveChangesAsync();
    }
}
