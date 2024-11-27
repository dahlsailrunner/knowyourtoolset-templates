using KnowYourToolset.BackEnd.Api.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace KnowYourToolset.BackEnd.Api.StartupServices;

public static class DbContextExtensions
{
    public static WebApplicationBuilder AddAppDbContext(this WebApplicationBuilder builder)
    {
#if POSTGRESQL
        builder.AddNpgsqlDbContext<AppDbContext>("DbConn", configureDbContextOptions: opts =>
        {
            opts.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
#endif
#if MSSQL
        builder.AddSqlServerDbContext<AppDbContext>("DbConn", configureDbContextOptions: opts =>
        {
            opts.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
#endif
#if SQLITE
        var dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                                "KnowYourToolset.BackEnd.sqlite");
        builder.Services.AddDbContext<AppDbContext>(opts => opts
            .UseSqlite($"Data Source={dbPath}")
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
#endif
        return builder;
    }

    public static void SetupDevelopmentDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
            dbContext.CreateSampleData(100);
        }
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
