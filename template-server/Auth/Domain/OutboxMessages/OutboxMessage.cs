using Domain.Common;

namespace Domain.OutboxMessages;

public class OutboxMessage : Entity<Guid>
{
    public string Type { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public string? Error { get; private set; }

    private OutboxMessage(Guid id, string type, string content)
        : base(id)
    {
        Type = type;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public static OutboxMessage Create(string type, string content) =>
        new OutboxMessage(Guid.NewGuid(), type, content);

    public void MarkAsProcessed() => ProcessedAt = DateTime.UtcNow;

    public void MarkAsFailed(string error)
    {
        Error = error;
        ProcessedAt = null;
    }
}
