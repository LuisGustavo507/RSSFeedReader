using RSSFeedReader.API.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS — origins read from configuration (no wildcard allowed per Constitution Principle II)
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .WithMethods("GET", "POST")
              .WithHeaders("Content-Type");
    });
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddSingleton<SubscriptionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// ── Endpoints ────────────────────────────────────────────────────────────────

app.MapPost("/api/subscriptions", (SubscriptionRequest request, SubscriptionService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Url))
        return Results.BadRequest(new { message = "A URL não pode ser vazia." });

    var subscription = service.AddSubscription(request.Url);
    return Results.Created($"/api/subscriptions/{subscription.Id}", subscription);
});

app.MapGet("/api/subscriptions", (SubscriptionService service) =>
{
    return Results.Ok(service.GetAll());
});

app.Run();

record SubscriptionRequest(string Url);

// Expose Program for integration tests (WebApplicationFactory<Program>)
public partial class Program { }
