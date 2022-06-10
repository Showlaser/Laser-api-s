using Auth_API.Dal;
using Auth_API.Interfaces.Dal;
using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.FromFrontend.User;
using Auth_API.Tests.IntegrationTests.Factories;
using Auth_API.Tests.IntegrationTests.TestModels;
using Auth_API.Tests.UnitTests.TestModels;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using System.Net;

namespace Auth_API.Tests.IntegrationTests
{
    public static class TestHelper
    {
        private static IServiceProvider? _services;
        private static readonly CookieContainerHandler Handler = new();
        public static HttpClient? Client;

        public static void SetupTestEnvironment()
        {
            if (_services != null && Client != null)
            {
                return;
            }

            SetEnvironmentVariables();
            SetupDependencyInjection();
            CreateTestUserIfItDoesNotExists();
            GetAuthorizationTokens();
        }

        private static void CreateTestUserIfItDoesNotExists()
        {
            IUserDal userDal = _services.GetService<IUserDal>();
            UserDto testUser = new TestUserDto().UserDto;
            bool testUserExists = userDal.Find(testUser.Username).Result != null;
            if (testUserExists)
            {
                return;
            }

            userDal.Add(testUser).Wait();
        }

        private static void GetAuthorizationTokens()
        {
            AuthFactory authFactory = new();
            CookieContainer cookieContainer = new();
            CookieContainerHandler handler = new(cookieContainer);

            HttpClient client = authFactory.CreateDefaultClient(handler);
            User user = new TestUser().User;

            HttpResponseMessage response = client.PostAsync("/user/login", new JsonContent<User>(user)).Result;
            response.EnsureSuccessStatusCode();
            foreach (Cookie cookie in cookieContainer.GetAllCookies())
            {
                Handler.Container.Add(cookie);
            }

            Client = authFactory.CreateDefaultClient(Handler);
        }

        private static void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("ARGON2SECRET", "jwoi320djkg40-8u92huyashdfkj");
            Environment.SetEnvironmentVariable("JWTSECRET", "fwhefwiufhawgh98g43hg98ahdfjig");
            Environment.SetEnvironmentVariable("SERVER", "localhost");
            Environment.SetEnvironmentVariable("DATABASE", "auth");
            Environment.SetEnvironmentVariable("PORT", "3306");
            Environment.SetEnvironmentVariable("USERNAME", "root");
            Environment.SetEnvironmentVariable("PASSWORD", "qwerty");
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTID", "123");
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTSECRET", "123");
        }

        private static void SetupDependencyInjection()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            builder.Services.AddScoped<UserLogic>();
            builder.Services.AddScoped<SpotifyTokenLogic>();
            builder.Services.AddScoped<IUserDal, UserDal>();
            builder.Services.AddScoped<IUserTokenDal, UserTokenDal>();
            builder.Services.AddScoped<ISpotifyTokenDal, SpotifyTokenDal>();
            builder.Services.AddScoped<IUserActivationDal, UserActivationDal>();
            builder.Services.AddScoped<IDisabledUserDal, DisabledUserDal>();

            string connectionString = GetConnectionString();
            builder.Services.AddDbContextPool<DataContext>(dbContextOptions => dbContextOptions
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            WebApplication app = builder.Build();
            _services = app.Services;
        }

        static string GetConnectionString()
        {
            // Uncomment string below when creating migrations
            return $"database=auth;keepalive=5;server=127.0.0.1;port=3306;user id=root;password=qwerty;connectiontimeout=5";

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
    }
}
