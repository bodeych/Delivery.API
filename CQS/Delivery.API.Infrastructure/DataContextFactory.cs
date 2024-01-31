using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Delivery.API.Infrastructure;

internal sealed class DeliveryGatewayDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        const string localPostgresConnectionString = "stub";

        var dbContextBuilder = new DbContextOptionsBuilder<DataContext>()
            .UseNpgsql("Server=localhost;Port=5432;Database=orders;User ID=postgres;Password=pass123");

        return new DataContext(dbContextBuilder.Options);
    }
}