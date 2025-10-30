using Application.Interfaces.Services;
using Domain.Interfaces;
using Infrastructure.Config;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Filters;
using Infrastructure.Middleware;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configurações
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<SerilogSettings>(configuration.GetSection("SerilogSettings"));

        // MongoDB
        services.AddSingleton<MongoDbContext>();

        // Repositórios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISellerProfileRepository, SellerProfileRepository>();
        services.AddScoped<ICustomerProfileRepository, CustomerProfileRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Serviços
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Autenticação JWT
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        // Autorização
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("Seller", policy => policy.RequireRole("Seller"));
            options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
            options.AddPolicy("SellerOrAdmin", policy => policy.RequireRole("Seller", "Admin"));
        });

        // Filters
        services.AddScoped<ValidationFilter>();

        // Serilog
        ConfigureSerilog(services, configuration);

        return services;
    }

    private static void ConfigureSerilog(IServiceCollection services, IConfiguration configuration)
    {
        var serilogSettings = configuration.GetSection("SerilogSettings").Get<SerilogSettings>();

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console();

        if (serilogSettings.WriteToFile)
        {
            loggerConfiguration.WriteTo.File(
                serilogSettings.LogPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7);
        }

        Log.Logger = loggerConfiguration.CreateLogger();
        services.AddSingleton(Log.Logger);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        // Middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Autenticação & Autorização
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
