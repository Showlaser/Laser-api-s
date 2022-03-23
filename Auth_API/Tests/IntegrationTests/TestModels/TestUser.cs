﻿using Auth_API.Models.FromFrontend.User;

namespace Auth_API.Tests.IntegrationTests.TestModels
{
    public class TestUser
    {
        public readonly User User = new()
        {
            UserName = "TestUser",
            Password = "123", // 123 plaintext
        };
    }
}