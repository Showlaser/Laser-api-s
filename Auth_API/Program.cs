using Auth_API.Dal;
using Auth_API.Interfaces.Dal;
using Auth_API.Logic;
using Auth_API.Models.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<UserLogic>();
// Typed client: IHttpClientFactory manages the underlying connection pool, preventing
// socket exhaustion that "new HttpClient()" per scope would cause.
builder.Services.AddHttpClient<SpotifyTokenLogic>();
builder.Services.AddTransient<ControllerResultHelper>();
builder.Services.AddScoped<IUserDal, UserDal>();
builder.Services.AddScoped<IUserTokenDal, UserTokenDal>();
builder.Services.AddScoped<ISpotifyTokenDal, SpotifyTokenDal>();
builder.Services.AddScoped<IUserActivationDal, UserActivationDal>();
builder.Services.AddScoped<IDisabledUserDal, DisabledUserDal>();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

string connectionString = GetConnectionString();
builder.Services.AddDbContextPool<DataContext>(dbContextOptions => dbContextOptions
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

WebApplication app = builder.Build();
string[] allowedOrigins = GetAllowedOrigins();
app.UseCors(b =>
{
    // Reflecting every origin while allowing credentials lets any website make authenticated
    // requests with the user's cookies. Restrict to an explicit allow-list instead.
    b.WithOrigins(allowedOrigins)
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseAuthorization();
app.MapControllers();
CreateDatabaseIfNotExist(app);

app.Run();

static string[] GetAllowedOrigins()
{
    // Explicit, comma-separated list takes precedence (e.g. "https://app.example.com,https://localhost:3000")
    string? configured = Environment.GetEnvironmentVariable("ALLOWEDORIGINS");
    if (!string.IsNullOrWhiteSpace(configured))
    {
        return configured.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    // Fall back to the origin (scheme + host + port) derived from FRONTENDURL
    string? frontEndUrl = Environment.GetEnvironmentVariable("FRONTENDURL");
    if (!string.IsNullOrWhiteSpace(frontEndUrl) && Uri.TryCreate(frontEndUrl, UriKind.Absolute, out Uri? uri))
    {
        return [uri.GetLeftPart(UriPartial.Authority)];
    }

    throw new NoNullAllowedException("No CORS origins configured. Set the ALLOWEDORIGINS environment variable " +
                                     "(comma-separated) or provide an absolute FRONTENDURL.");
}

static string GetConnectionString()
{
    // Uncomment string below when creating migrations
    //return $"database=auth;keepalive=5;server=127.0.0.1;port=3306;user id=root;password=qwerty;connectiontimeout=5";

    IDictionary variables = Environment.GetEnvironmentVariables();
    string? server = variables["SERVER"]?.ToString();
    string? database = variables["DATABASE"]?.ToString();
    int port = Convert.ToInt32(variables["PORT"]);
    string? username = variables["USERNAME"]?.ToString();
    string? password = variables["PASSWORD"]?.ToString();

    if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(database) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        throw new NoNullAllowedException("Environment variable or variables where null. Please enter include the following environment variables:" +
                                         "SERVER, DATABASE, PORT, USERNAME and Password");
    }

    return $"database={database};keepalive=5;server={server};port={port};user id={username};password={password};connectiontimeout=5";
}

static void CreateDatabaseIfNotExist(IApplicationBuilder app)
{
    IServiceScope serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    DataContext? context = serviceScope.ServiceProvider.GetService<DataContext>();
    context?.Database.Migrate();
}