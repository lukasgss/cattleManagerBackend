using System;
using Xunit;
using CattleManager.Application.Application.Services.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CattleManager.Tests;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void ShouldGenerateJwtTokenAndHaveCorrectClaims()
    {
        var jwtTokenGeneratorMock = new JwtTokenGenerator("issuer", "audience", "superSecretKeyVeryLong");

        var userId = Guid.NewGuid();
        var token = jwtTokenGeneratorMock.GenerateToken(userId, "firstName", "lastName");
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(token);
        var sub = decodedToken.Claims.First(claim => claim.Type == "sub").Value;
        var givenName = decodedToken.Claims.First(claim => claim.Type == "given_name").Value;
        var familyName = decodedToken.Claims.First(claim => claim.Type == "family_name").Value;
        var jti = decodedToken.Claims.First(claim => claim.Type == "jti").Value;
        var issuer = decodedToken.Claims.First(claim => claim.Type == "iss").Value;
        var audience = decodedToken.Claims.First(claim => claim.Type == "aud").Value;

        Assert.NotNull(token);
        Assert.Equal(userId.ToString(), sub);
        Assert.Equal("firstName", givenName);
        Assert.Equal("lastName", familyName);
        Assert.NotNull(jti);
        Assert.Equal("issuer", issuer);
        Assert.Equal("audience", audience);
    }
}