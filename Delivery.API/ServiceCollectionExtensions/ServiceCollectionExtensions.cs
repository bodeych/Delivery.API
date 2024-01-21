using System.Text;
using Delivery.API.Application;
using Delivery.API.Application.Interfaces;
using Delivery.API.Application.Services;
using Delivery.API.Application.Settings;
using Delivery.API.Domain;
using Delivery.API.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace Delivery.API.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<DataContext>((sp, options) =>
        {
            var settings = sp.GetRequiredService<DatabaseSettings>();

            options.UseNpgsql(settings.ConnectionString);
        });
        services.AddScoped<IDataContext, DataContext>();
        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<DataContext>();
        
        return services;
    }
    
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        
            services.AddScoped<OrderSettings>(_ =>
        {
            var orderSettings = new OrderSettings();

            configuration.GetSection(nameof(OrderSettings)).Bind(orderSettings);

            return orderSettings;
        });
        services.AddScoped<DatabaseSettings>(_ =>
        {
            var databaseSettings = new DatabaseSettings();

            configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            return databaseSettings;
        });
        
        return services;
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<OrderService>();
        services.AddScoped<DistanceCalculator>();
        services.AddScoped<CostCalculator>();
        
        return services;
    }
}