using System.Net.Http.Json;

namespace RSSFeedReader.UI.Services;

public class SubscriptionApiClient
{
    private readonly HttpClient _http;

    public SubscriptionApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<Subscription?> AddSubscriptionAsync(string url)
    {
        var response = await _http.PostAsJsonAsync("/api/subscriptions", new { url });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Subscription>();
    }

    public async Task<List<Subscription>> GetSubscriptionsAsync()
    {
        return await _http.GetFromJsonAsync<List<Subscription>>("/api/subscriptions")
               ?? new List<Subscription>();
    }
}

public class Subscription
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
}
