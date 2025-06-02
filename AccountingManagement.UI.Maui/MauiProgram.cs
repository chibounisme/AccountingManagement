using System.Globalization;
using AccountingManagement.Application.Abstractions;
using AccountingManagement.Application.Companies.Services;
using AccountingManagement.Application.Users.Services;
using AccountingManagement.Domain.Interfaces;
using AccountingManagement.Infrastructure.Persistence;
using AccountingManagement.Infrastructure.Persistence.Repositories;
using AccountingManagement.Infrastructure.Services;
using AccountingManagement.UI.Maui.Authentication;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountingManagement.UI.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Infrastructure Services
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // EF Core DbContext
        string inMemoryDatabaseName = "AccountingInMemoryDB_DDD";
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: inMemoryDatabaseName)
                   .EnableSensitiveDataLogging()
        );
        builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());


        // Repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

        // Application Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICompanyService, CompanyService>();
        
        // Authentication
        builder.Services.AddScoped<CurrentUserService>();
        builder.Services.AddScoped<ICurrentUserService>(provider => provider.GetRequiredService<CurrentUserService>());
        builder.Services.AddScoped<CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        builder.Services.AddAuthorizationCore();
        
        builder.Services.AddI18nText();
        
        var app = builder.Build();
        
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated(); 
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MauiProgram] In-Memory Database initialization FAILED: {ex}");
        }

        return app;
    }
}