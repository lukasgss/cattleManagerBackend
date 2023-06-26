using System;
using Xunit;
using CattleManager.Application.Application.Services.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using FakeItEasy;

namespace CattleManager.Tests;

public class JwtTokenGeneratorTests
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly Guid _userId;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtTokenGeneratorTests()
    {
        _jwtTokenGenerator = new JwtTokenGenerator("issuer", "audience", "verySuperSecretKeyVeryLongBigEnough");
        _userId = Guid.NewGuid();
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    [Fact]
    public void Generates_Jwt_Token_And_Has_Correct_Claims()
    {
        var token = _jwtTokenGenerator.GenerateToken(_userId, "firstName", "lastName");
        var decodedToken = _tokenHandler.ReadJwtToken(token);
        var sub = decodedToken.Claims.First(claim => claim.Type == "sub").Value;
        var givenName = decodedToken.Claims.First(claim => claim.Type == "given_name").Value;
        var familyName = decodedToken.Claims.First(claim => claim.Type == "family_name").Value;
        var jti = decodedToken.Claims.First(claim => claim.Type == "jti").Value;
        var issuer = decodedToken.Claims.First(claim => claim.Type == "iss").Value;
        var audience = decodedToken.Claims.First(claim => claim.Type == "aud").Value;

        Assert.NotNull(token);
        Assert.Equal(_userId.ToString(), sub);
        Assert.Equal("firstName", givenName);
        Assert.Equal("lastName", familyName);
        Assert.NotNull(jti);
        Assert.Equal("issuer", issuer);
        Assert.Equal("audience", audience);
    }
}