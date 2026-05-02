using RSSFeedReader.API.Models;

namespace RSSFeedReader.API.Services;

public class SubscriptionService
{
    private readonly List<Subscription> _subscriptions = new();
    private int _nextId = 1;

    public Subscription AddSubscription(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("A URL não pode ser vazia.", nameof(url));

        var subscription = new Subscription { Id = _nextId++, Url = url };
        _subscriptions.Add(subscription);
        return subscription;
    }

    public IEnumerable<Subscription> GetAll() => _subscriptions.AsReadOnly();
}
