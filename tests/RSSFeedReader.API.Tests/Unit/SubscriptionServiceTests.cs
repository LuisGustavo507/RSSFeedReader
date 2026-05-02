using RSSFeedReader.API.Services;

namespace RSSFeedReader.API.Tests.Unit;

public class SubscriptionServiceTests
{
    private readonly SubscriptionService _sut = new();

    [Fact]
    public void AddSubscription_WithValidUrl_ReturnsSubscriptionWithIdAndUrl()
    {
        var url = "https://devblogs.microsoft.com/dotnet/feed/";

        var result = _sut.AddSubscription(url);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(url, result.Url);
    }

    [Fact]
    public void AddSubscription_SequentialIds_IncrementCorrectly()
    {
        _sut.AddSubscription("https://feed1.example.com/rss");
        var second = _sut.AddSubscription("https://feed2.example.com/rss");

        Assert.Equal(2, second.Id);
    }

    [Fact]
    public void AddSubscription_WithEmptyUrl_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.AddSubscription(string.Empty));
    }

    [Fact]
    public void AddSubscription_WithWhitespaceUrl_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.AddSubscription("   "));
    }

    [Fact]
    public void GetAll_WhenEmpty_ReturnsEmptyList()
    {
        var result = _sut.GetAll();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_AfterAddingSubscriptions_ReturnsAllInInsertionOrder()
    {
        _sut.AddSubscription("https://first.example.com/rss");
        _sut.AddSubscription("https://second.example.com/rss");
        _sut.AddSubscription("https://third.example.com/rss");

        var result = _sut.GetAll().ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal("https://first.example.com/rss", result[0].Url);
        Assert.Equal("https://second.example.com/rss", result[1].Url);
        Assert.Equal("https://third.example.com/rss", result[2].Url);
    }
}
