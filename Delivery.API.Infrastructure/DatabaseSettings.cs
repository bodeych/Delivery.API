using Microsoft.Extensions.Configuration;

namespace Delivery.API.Infrastructure;

public class DatabaseSettings
{
    public string? ConnectionString { get; init; }
}