using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks().AddAsyncCheck("URL", async () =>
{
    using var httpClient = new HttpClient();
    using var request = new HttpRequestMessage(HttpMethod.Head, Environment.GetEnvironmentVariable("CHECK_URL"));
    var response = await httpClient.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
    return response.IsSuccessStatusCode ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
});
var app = builder.Build();

app.MapHealthChecks("/health");
app.MapGet("/", () => $"Hello World! ({Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString()})");

app.Run();
