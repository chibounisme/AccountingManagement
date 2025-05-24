using Microsoft.Extensions.Logging;
using AccountingManagement.Data;
using AccountingManagement.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using Microsoft.EntityFrameworkCore; // EF Core namespace
using AccountingManagement.LocalizationResources; // For AppStrings marker class

namespace AccountingManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            System.Diagnostics.Debug.WriteLine("[MauiProgram] CreateMauiApp START");
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                });
            System.Diagnostics.Debug.WriteLine("[MauiProgram] Fonts configured.");

            builder.Services.AddMauiBlazorWebView();
            System.Diagnostics.Debug.WriteLine("[MauiProgram] MauiBlazorWebView added.");

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            System.Diagnostics.Debug.WriteLine("[MauiProgram] BlazorWebViewDeveloperTools added.");
            builder.Logging.AddDebug(); // Adds default debug logger
            System.Diagnostics.Debug.WriteLine("[MauiProgram] Debug logging added.");
#endif

            // --- EF Core DbContext Configuration for In-Memory Database ---
            string inMemoryDatabaseName = "AccountingInMemoryDB_Runtime"; // Unique name for runtime instance
            System.Diagnostics.Debug.WriteLine($"[MauiProgram] Configuring In-Memory Database: {inMemoryDatabaseName}");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: inMemoryDatabaseName)
                       // Optional: Add .EnableSensitiveDataLogging() for debugging EF Core queries if needed,
                       // but remove it for production/release builds.
                       // .EnableSensitiveDataLogging() 
                       );

            System.Diagnostics.Debug.WriteLine("[MauiProgram] AppDbContext (EF Core In-Memory) registered.");
            // --- End EF Core DbContext Configuration ---


            System.Diagnostics.Debug.WriteLine("[MauiProgram] Registering Auth Services...");
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthService>();
            System.Diagnostics.Debug.WriteLine("[MauiProgram] Auth Services registered.");

            System.Diagnostics.Debug.WriteLine("[MauiProgram] Registering Business Services...");
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<CompanyService>();
            System.Diagnostics.Debug.WriteLine("[MauiProgram] Business Services registered.");

            System.Diagnostics.Debug.WriteLine("[MauiProgram] Registering Localization...");
            // Ensure your RESX files are in a folder named "LocalizationResources" at the project root
            // and their Build Action is "Embedded resource".
            builder.Services.AddLocalization(options => options.ResourcesPath = "LocalizationResources");
            System.Diagnostics.Debug.WriteLine("[MauiProgram] Localization registered.");

            System.Diagnostics.Debug.WriteLine("[MauiProgram] Building app...");
            var app = builder.Build();
            System.Diagnostics.Debug.WriteLine("[MauiProgram] App built.");

            // --- Initialize In-Memory Database (Schema and Seed Data) ---
            // This scope ensures DbContext is disposed properly after use.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    System.Diagnostics.Debug.WriteLine("[MauiProgram] Attempting to initialize in-memory database...");
                    var dbContext = services.GetRequiredService<AppDbContext>();

                    // For In-Memory database, EnsureCreated() is used.
                    // This creates the schema based on your model and applies HasData seeding.
                    // It does NOT use migrations.
                    bool databaseCreated = dbContext.Database.EnsureCreated();
                    System.Diagnostics.Debug.WriteLine(databaseCreated
                        ? "[MauiProgram] In-Memory Database.EnsureCreated() successfully created schema and seeded data."
                        : "[MauiProgram] In-Memory Database.EnsureCreated() found existing schema (or no action taken if already created).");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[MauiProgram] In-Memory Database initialization FAILED: {ex.ToString()}");
                    // Consider how to handle this critical failure (e.g., log to a file, show user message, exit).
                }
            }
            // --- End Initialize In-Memory Database ---

            // Set default culture
            var frenchCulture = new CultureInfo("fr-FR");
            CultureInfo.DefaultThreadCurrentCulture = frenchCulture;
            CultureInfo.DefaultThreadCurrentUICulture = frenchCulture;
            System.Diagnostics.Debug.WriteLine("[MauiProgram] Culture set to fr-FR.");

            System.Diagnostics.Debug.WriteLine("[MauiProgram] CreateMauiApp END");
            return app;
        }
    }
}