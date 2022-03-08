using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using Vote_API.Dal;
using Vote_API.Interfaces.Dal;
using Vote_API.Logic;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IVoteDal, VoteDal>();
builder.Services.AddScoped<VoteLogic>();

string connectionString = GetConnectionString();
builder.Services.AddDbContextPool<DataContext>(dbContextOptions => dbContextOptions
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

WebApplication? app = builder.Build();
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseAuthorization();
app.MapControllers();
CreateDatabaseIfNotExist(app);
app.Run();

static string GetConnectionString()
{
    // Uncomment string below when creating migrations
    //return $"database=vote;keepalive=5;server=127.0.0.1;port=3306;user id=root;password=qwerty;connectiontimeout=5";

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

/// <summary>
/// Creates and database if it does not exists
/// </summary>
/// <param name="app">IApplicationBuilder object</param>
static void CreateDatabaseIfNotExist(IApplicationBuilder app)
{
    IServiceScope serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    DataContext? context = serviceScope.ServiceProvider.GetService<DataContext>();
    context.Database.Migrate();
}