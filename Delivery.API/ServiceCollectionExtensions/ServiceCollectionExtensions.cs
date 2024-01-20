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
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
            services.AddSingleton(tokenValidationParameters);

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
        services.AddScoped<JwtSettings>(_ =>
        {
            var jwtSettings = new JwtSettings();

            configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

            return jwtSettings;
        });
        
        return services;
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<OrderService>();
        services.AddScoped<DistanceCalculator>();
        services.AddScoped<CostCalculator>();
        services.AddScoped<IdentityService>();
        
        return services;
    }
}