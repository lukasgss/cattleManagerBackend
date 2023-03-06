using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace CattleManager.Application.Application.Services.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly string _secretKey;
    public JwtTokenGenerator(string issuer, string audience, string secretKey)
    {
        _issuer = issuer;
        _audience = audience;
        _secretKey = secretKey;
    }
    public string GenerateToken(Guid userId, string firstName, string lastName)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var securityToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}