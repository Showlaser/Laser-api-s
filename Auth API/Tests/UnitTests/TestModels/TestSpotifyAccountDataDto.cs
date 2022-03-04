﻿using Auth_API.Models.Dto.User;

namespace Auth_API.Tests.TestModels
{
    public class TestSpotifyAccountDataDto
    {
        public readonly UserTokensDto UserTokensDto = new()
        {
            UserUuid = Guid.Parse("4a4a4847-e081-40c8-a020-b5c2d4ccc00d"),
            RefreshToken = "fh39238f2h9f832",
            Uuid = Guid.Parse("b6083dd5-c9d1-44d4-95c6-23a4b56b8656")
        };
    }
}