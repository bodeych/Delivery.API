using System.Text;
using Delivery.API.Application.Interface;
using Delivery.API.Application.Repositories;
using Delivery.API.Application.Services;
using Delivery.API.Application.Settings;
using Delivery.API.Infrastructure;
using Delivery.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;



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
        
        return services;
    }
    
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(jwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);
        
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
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };
        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {   // for development only
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = tokenValidationParameters;
            });
        
        
        return services;
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IdentityService>();
        services.AddScoped<GenerateToken>();
        services.AddScoped<CustomerService>();
        services.AddMemoryCache();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IIdentityRepository, IdentityRepository>();
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        
        
        return services;
    }
}