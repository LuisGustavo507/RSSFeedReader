using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RSSFeedReader.API.Models;

namespace RSSFeedReader.API.Tests.Integration;

// Each test instance gets a fresh WebApplicationFactory (and therefore a fresh
// SubscriptionService singleton) so tests are fully isolated from each other.
public class SubscriptionsEndpointTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SubscriptionsEndpointTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    // ── POST /api/subscriptions ──────────────────────────────────────────────

    [Fact]
    public async Task PostSubscription_WithValidUrl_Returns201AndSubscription()
    {
        var payload = new { url = "https://devblogs.microsoft.com/dotnet/feed/" };

        var response = await _client.PostAsJsonAsync("/api/subscriptions", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var subscription = await response.Content.ReadFromJsonAsync<Subscription>();
        Assert.NotNull(subscription);
        Assert.True(subscription!.Id > 0);
        Assert.Equal(payload.url, subscription.Url);
    }

    [Fact]
    public async Task PostSubscription_WithEmptyUrl_Returns400WithMessage()
    {
        var payload = new { url = string.Empty };

        var response = await _client.PostAsJsonAsync("/api/subscriptions", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(body);
        Assert.True(body!.ContainsKey("message"));
        Assert.False(string.IsNullOrWhiteSpace(body["message"]));
    }

    // ── GET /api/subscriptions ───────────────────────────────────────────────

    [Fact]
    public async Task GetSubscriptions_WhenEmpty_Returns200WithEmptyArray()
    {
        var response = await _client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var list = await response.Content.ReadFromJsonAsync<List<Subscription>>();
        Assert.NotNull(list);
        Assert.Empty(list!);
    }

    [Fact]
    public async Task GetSubscriptions_AfterPostingTwo_Returns200WithBothInOrder()
    {
        await _client.PostAsJsonAsync("/api/subscriptions", new { url = "https://first.example.com/rss" });
        await _client.PostAsJsonAsync("/api/subscriptions", new { url = "https://second.example.com/rss" });

        var response = await _client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var list = await response.Content.ReadFromJsonAsync<List<Subscription>>();
        Assert.NotNull(list);
        Assert.True(list!.Count >= 2);
        Assert.Contains(list, s => s.Url == "https://first.example.com/rss");
        Assert.Contains(list, s => s.Url == "https://second.example.com/rss");
    }
}
