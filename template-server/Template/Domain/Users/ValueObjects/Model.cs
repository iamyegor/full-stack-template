using Domain.Common;

namespace Domain.Users.ValueObjects;

public class Model : ValueObject
{
    private static Model Gpt4o = new Model("gpt-4o", [SubscriptionStatus.Plus]);
    private static Model Gpt4oMini = new Model(
        "gpt-4o-mini",
        [SubscriptionStatus.Free, SubscriptionStatus.Plus]
    );

    public string Name { get; }
    public IReadOnlyList<SubscriptionStatus> SubscriptionsWithAccess =>
        _subscriptionStatus.AsReadOnly();
    private readonly List<SubscriptionStatus> _subscriptionStatus;

    private Model(string name, List<SubscriptionStatus> subscriptionStatus)
    {
        Name = name;
        _subscriptionStatus = subscriptionStatus;
    }

    public static Model? GetByName(string name)
    {
        return new List<Model> { Gpt4o, Gpt4oMini }.SingleOrDefault(x => x.Name == name);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}
