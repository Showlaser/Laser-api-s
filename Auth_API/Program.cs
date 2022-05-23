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
builder.Services.AddScoped<SpotifyTokenLogic>();
builder.Services.AddTransient<ControllerResultHelper>();
builder.Services.AddScoped<IUserDal, UserDal>();
builder.Services.AddScoped<IUserTokenDal, UserTokenDal>();
builder.Services.AddScoped<ISpotifyTokenDal, SpotifyTokenDal>();
builder.Services.AddScoped<IUserActivationDal, UserActivationDal>();
builder.Services.AddScoped<IDisabledUserDal, DisabledUserDal>();

string connectionString = GetConnectionString();
builder.Services.AddDbContextPool<DataContext>(dbContextOptions => dbContextOptions
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

WebApplication app = builder.Build();
app.UseCors(b =>
{
    b.AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

app.UseAuthorization();
app.MapControllers();
CreateDatabaseIfNotExist(app);

app.Run();

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