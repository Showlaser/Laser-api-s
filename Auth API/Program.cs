using Auth_API.Dal;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

string connectionString = GetConnectionString();
if (string.IsNullOrEmpty(connectionString))
{
    throw new NoNullAllowedException("Connectionstring is empty set it using the CONNECTIONSTRING environment variable");
}

builder.Services.AddDbContextPool<DataContext>(dbContextOptions => dbContextOptions
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

static string GetConnectionString()
{
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