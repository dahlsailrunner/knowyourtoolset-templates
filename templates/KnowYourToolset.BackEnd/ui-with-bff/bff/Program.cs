using Duende.Bff;
using Duende.Bff.Yarp;
using KnowYourToolset.BackEnd.Bff;
using KnowYourToolset.BackEnd.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddSpaYarp();
builder.Services.AddAuthorization();

#if SQLITE
var filename = Path.Join(
           Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
           "Project.Name");
builder.Services.AddDbContext<LocalDbContext>(options => options
        .UseSqlite($"Data Source={filename}"));
#endif
#if POSTGRESQL
builder.AddNpgsqlDbContext<LocalDbContext>("DbConn");
#endif
#if MSSQL
builder.AddSqlServerDbContext<LocalDbContext>("DbConn");
#endif
builder.Services.AddDataProtection()
     .PersistKeysToDbContext<LocalDbContext>();

builder.Services.AddBff(o => o.ManagementBasePath = "/account")
  .AddRemoteApis()
  .AddServerSideSessions();

var authSettings = builder.Configuration.GetSection("Authentication").Get<AuthSettings>();
builder.Services.AddAuthentication(o =>
    {
        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(o =>
    {
        o.Cookie.Name = "__Host-kyt-ui-app";
        o.Cookie.SameSite = SameSiteMode.Strict;
        o.Events.OnRedirectToLogin = (context) =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = authSettings!.Authority;
        options.ClientId = authSettings!.ClientId;
        options.ClientSecret = authSettings!.ClientSecret;

        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.SaveTokens = true;

        options.Scope.Clear();
        foreach (var scope in authSettings.Scopes)
        {
            options.Scope.Add(scope);
        }
    });

var app = builder.Build();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LocalDbContext>();
    context.Database.Migrate();
}

app.UseStaticFiles();
app.UseBff();
app.UseAuthentication();
app.UseAuthorization();
app.MapBffManagementEndpoints();

var remoteApis = builder.Configuration.GetSection("RemoteApis").Get<Dictionary<string, string>>();
foreach (var path in remoteApis!.Keys)
{
    app.Logger.LogInformation($"Mapping remote API: /{path} => {remoteApis[path]}");
    app.MapRemoteBffApiEndpoint($"/{path}", remoteApis[path])
      .RequireAccessToken(TokenType.User);
}

app.UseMiddleware<UserScopeMiddleware>();
app.UseSpaYarp(); // only used in DEV
app.MapFallbackToFile("index.html");

app.Run();

public record AuthSettings(string Authority, string ClientId, string ClientSecret, List<string> Scopes);


