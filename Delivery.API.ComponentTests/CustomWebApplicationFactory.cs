using Delivery.API.Application.Services;
using Delivery.API.Domain.Entities;
using Delivery.API.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.API.ComponentTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));

            services.Remove(dbContextDescriptor);

            services.AddDbContext<DataContext>(options =>
            {
                options
                    .UseInMemoryDatabase("Test-database");
            });

            using var scope = services.BuildServiceProvider().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            
            

            context.Users.Add(User.Create(Guid.NewGuid(), "existingUser", "12345", "Access", "Refresh"));
            context.SaveChanges();
        });

        builder.UseEnvironment("Development");
    }
}