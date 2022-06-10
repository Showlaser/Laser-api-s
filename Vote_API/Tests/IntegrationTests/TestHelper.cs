using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Vote_API.Dal;
using Vote_API.Interfaces.Dal;
using Vote_API.Logic;
using Vote_API.Models.FromFrontend;
using Vote_API.Models.Helper;
using Vote_API.Tests.IntegrationTests.Factories;
using Vote_API.Tests.IntegrationTests.TestModels;

namespace Vote_API.Tests.IntegrationTests
{
    public static class TestHelper
    {
        private static IServiceProvider? _services;
        private static readonly CookieContainerHandler Handler = new();
        internal static HttpClient? Client;
        internal static VoteJoinData VoteJoinData { get; private set; }

        public static void SetupTestEnvironment()
        {
            if (_services != null && Client != null)
            {
                return;
            }

            SetEnvironmentVariables();
            SetupDependencyInjection();
            GetAuthorizationTokens();
            RemoveTestVoteDataIfExists();
            AddVoteData();
        }

        private static void RemoveTestVoteDataIfExists()
        {
            VoteLogic voteLogic = _services.GetService<VoteLogic>();
            voteLogic.Remove(new TestVoteData().VoteData.Uuid).Wait();
        }

        private static void AddVoteData()
        {
            VoteData voteData = new TestVoteData().VoteData;
            HttpResponseMessage postResponse = Client?.PostAsync("vote", new JsonContent<VoteData>(voteData)).Result;
            postResponse.EnsureSuccessStatusCode();
            VoteJoinData = postResponse.Content.ReadFromJsonAsync<VoteJoinData>().Result;
        }

        private static void GetAuthorizationTokens()
        {
            AuthFactory authFactory = new();
            string jwt = GenerateJwtToken(Guid.Parse("4a4a4847-e081-40c8-a020-b5c2d4ccc00d"));
            Handler.Container.Add(new Cookie("jwt", jwt, "/", "localhost"));
            Client = authFactory.CreateDefaultClient(Handler);
        }

        private static string GenerateJwtToken(Guid userUuid)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            string jwtSecret = Environment.GetEnvironmentVariable("JWTSECRET") ?? throw new NoNullAllowedException("Environment variable" +
                "JWTSECRET was empty. Set it using the JWTSECRET environment variable");

            byte[] jwtSecretKey = Encoding.ASCII.GetBytes(jwtSecret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("uuid", userUuid.ToString()),
                }),
                Audience = "auth",
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtSecretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("ARGON2SECRET", "fho938fh32oewfjfefj2398");
            Environment.SetEnvironmentVariable("JWTSECRET", "hwef9hewihsidhfkfdafew");
            Environment.SetEnvironmentVariable("SERVER", "localhost");
            Environment.SetEnvironmentVariable("DATABASE", "vote");
            Environment.SetEnvironmentVariable("PORT", "3306");
            Environment.SetEnvironmentVariable("USERNAME", "root");
            Environment.SetEnvironmentVariable("PASSWORD", "qwerty");
        }

        private static void SetupDependencyInjection()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            builder.Services.AddControllers();
            builder.Services.AddScoped<IVoteDal, VoteDal>();
            builder.Services.AddScoped<IPlaylistVoteDal, PlaylistVoteDal>();
            builder.Services.AddTransient<ControllerResultHelper>();
            builder.Services.AddScoped<VoteLogic>();
            builder.Services.AddSingleton<WebsocketVoteEventSubscriber>();

            string connectionString = GetConnectionString();
            builder.Services.AddDbContextPool<DataContext>(dbContextOptions => dbContextOptions
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            WebApplication app = builder.Build();
            _services = app.Services;
        }

        static string GetConnectionString()
        {
            // Uncomment string below when creating migrations
            return $"database=vote;keepalive=5;server=127.0.0.1;port=3306;user id=root;password=qwerty;connectiontimeout=5";

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
